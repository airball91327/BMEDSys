using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using EDIS.Areas.BMED.Data;
using EDIS.Areas.BMED.Models.DeliveryModels;
using EDIS.Areas.BMED.Models.KeepModels;
using EDIS.Areas.BMED.Models.RepairModels;
using EDIS.Models.Identity;
using EDIS.Repositories;
using EDIS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EDIS.Areas.BMED.Controllers
{
    [Area("BMED")]
    [Authorize]
    public class DelivFlowController : Controller
    {
        private readonly BMEDDbContext _context;
        private readonly IRepository<AppUserModel, int> _userRepo;
        private readonly IRepository<DepartmentModel, string> _dptRepo;
        private readonly IEmailSender _emailSender;
        private readonly CustomUserManager userManager;
        private readonly CustomRoleManager roleManager;

        public DelivFlowController(BMEDDbContext context,
                                   IRepository<AppUserModel, int> userRepo,
                                   IRepository<DepartmentModel, string> dptRepo,
                                   IEmailSender emailSender,
                                   CustomUserManager customUserManager,
                                   CustomRoleManager customRoleManager)
        {
            _context = context;
            _userRepo = userRepo;
            _dptRepo = dptRepo;
            _emailSender = emailSender;
            userManager = customUserManager;
            roleManager = customRoleManager;
        }

        // GET: /BMED/DelivFlow/
        public IActionResult Index()
        {
            return View(_context.DelivFlows.ToList());
        }

        // GET: /BMED/DelivFlow/Details/5
        public IActionResult NextFlow(string id = null)
        {
            var docId = id;
            DelivFlowModel rf = _context.DelivFlows.Where(df => df.DocId == docId && df.Status == "?")
                                                   .ToList().FirstOrDefault();
            if (rf != null)
            {
                rf.DocId = id;
                List<SelectListItem> listItem = new List<SelectListItem>();
                listItem.Add(new SelectListItem { Text = "申請者", Value = "申請者" });
                //listItem.Add(new SelectListItem { Text = "採購人員", Value = "採購人員" });
                //listItem.Add(new SelectListItem { Text = "單位主管", Value = "單位主管" });
                listItem.Add(new SelectListItem { Text = "設備工程師", Value = "設備工程師" });
                listItem.Add(new SelectListItem { Text = "設備主管", Value = "設備主管" });
                listItem.Add(new SelectListItem { Text = "得標廠商", Value = "得標廠商" });
                //listItem.Add(new SelectListItem { Text = "使用單位", Value = "使用單位" });
                listItem.Add(new SelectListItem { Text = "設備經辦", Value = "設備經辦" });
                //listItem.Add(new SelectListItem { Text = "採購主管", Value = "採購主管" });
                if (rf.Cls == "設備主管")
                    listItem.Add(new SelectListItem { Text = "結案", Value = "結案" });
                ViewData["Item"] = new SelectList(listItem, "Value", "Text", "");
                //
                List<SelectListItem> listItem2 = new List<SelectListItem>();
                listItem2.Add(new SelectListItem { Text = "", Value = "" });
                ViewData["Item2"] = new SelectList(listItem2, "Value", "Text", "");
                rf.Cls = "";
                //
                //DeliveryModel ra = _context.Deliveries.Find(id);
                //if (ra != null)
                //{
                //    string cid = _context.AppUsers.Find(ra.UserId).DptId;
                //    string gid = _context.Departments.Find(cid).GroupId;
                //    if (gid != null)
                //    {
                //        rf.FlowHint = db.Groups.Find(gid).FlowHint4;
                //    }
                //}
                rf.SelOpin = "同意";
            }
            return PartialView(rf);
        }

        // POST: /BMED/DelivFlow/NextFlow/5
        [HttpPost]
        public IActionResult NextFlow(DelivFlowModel DelivFlow)
        {
            if (ModelState.IsValid)
            {
                // Get Login User's details.
                var ur = _userRepo.Find(usr => usr.UserName == User.Identity.Name).FirstOrDefault();

                if (DelivFlow.SelOpin == "其他" && string.IsNullOrEmpty(DelivFlow.Opinions))
                {
                    throw new Exception("請填寫意見欄!!");
                }
                DelivFlowModel rf = _context.DelivFlows.Where(df => df.DocId == DelivFlow.DocId)
                                                       .Where(df => df.Status == "?").ToList().FirstOrDefault();
                if (rf == null)
                {
                    throw new Exception("查無流程!");
                }
                if (rf.Cls == "得標廠商")
                {
                    if (_context.BMEDAssets.Where(a => a.Docid == DelivFlow.DocId).Count() <= 0)
                    {
                        throw new Exception("沒有設備紀錄! 請新增設備!");
                    }
                }
                List<DelivFlowModel> rflist = _context.DelivFlows.Where(df => df.DocId == DelivFlow.DocId).ToList();
                DeliveryModel r = _context.Deliveries.Find(DelivFlow.DocId);
                AppUserModel u;
                Tmail mail;
                string body = "";
                if (DelivFlow.Cls == "結案")
                {
                    string sto = "";
                    //KeepDtl rd = db.KeepDtls.Find(KeepFlow.Docid);
                    rf.Opinions = DelivFlow.SelOpin + Environment.NewLine + DelivFlow.Opinions;
                    rf.Status = "2";
                    rf.Rtt = DateTime.Now;
                    rf.Rtp = ur.Id;
                    _context.Entry(rf).State = EntityState.Modified;
                    _context.SaveChanges();
                    //
                    mail = new Tmail();
                    u = _context.AppUsers.Find(ur.Id);
                    mail.from = new System.Net.Mail.MailAddress(u.Email); //u.Email
                    foreach (DelivFlowModel rr in rflist)
                    {
                        u = _context.AppUsers.Find(rr.UserId);
                        sto += u.Email + ",";
                    }
                    mail.sto = sto.TrimEnd(new char[] { ',' });
                    //mail.cc = new System.Net.Mail.MailAddress("99242@cch.org.tw");
                    mail.message.Subject = "醫療儀器管理資訊系統[驗收案-結案通知]：採購案號： " + r.PurchaseNo;
                    body += "<p>申請人：" + r.UserName + "</p>";
                    body += "<p>合約號碼：" + r.ContractNo + "</p>";
                    body += "<p>採購案號：" + r.PurchaseNo + "</p>";
                    body += "<p><a href='http://dms.cch.org.tw/MvcMedEngMgr'>前往網站檢視</a></p>";
                    body += "<br/>";
                    body += "<h3>this is a inform letter from system manager.Do not reply for it.</h3>";
                    mail.message.Body = body;
                    mail.message.IsBodyHtml = true;
                    //mail.SendMail();
                    return new JsonResult(DelivFlow)
                    {
                        Value = new { success = true, error = "" },
                    };
                }
                DelivFlow.StepId = rf.StepId + 1;
                DelivFlow.Rtt = DateTime.Now;
                switch (DelivFlow.Cls)
                {
                    case "申請者":
                        DelivFlow.UserId = r.UserId;
                        break;
                }
                DelivFlow.Status = "?";
                u = _context.AppUsers.Find(DelivFlow.UserId);
                DelivFlow.Role = roleManager.GetRolesForUser(u.Id).FirstOrDefault();
                rf.Opinions = DelivFlow.SelOpin + Environment.NewLine + DelivFlow.Opinions;
                rf.Status = "1";
                rf.Rtp = ur.Id;
                DelivFlow.Opinions = null;
                _context.Entry(rf).State = EntityState.Modified;
                _context.DelivFlows.Add(DelivFlow);
                _context.SaveChanges();
                //
                mail = new Tmail();
                body = "";
                u = _context.AppUsers.Find(ur.Id);
                mail.from = new System.Net.Mail.MailAddress(u.Email); //u.Email
                u = _context.AppUsers.Find(DelivFlow.UserId);
                mail.to = new System.Net.Mail.MailAddress(u.Email); //u.Email
                //mail.cc = new System.Net.Mail.MailAddress("99242@cch.org.tw");
                mail.message.Subject = "醫療儀器管理資訊系統[驗收案]：採購案號： " + r.PurchaseNo;
                body += "<p>申請人：" + r.UserName + "</p>";
                body += "<p>合約號碼：" + r.ContractNo + "</p>";
                body += "<p>採購案號：" + r.PurchaseNo + "</p>";
                body += "<p><a href='http://dms.cch.org.tw/MvcMedEngMgr'>處理案件</a></p>";
                body += "<br/>";
                body += "<p>若有任何問題，請與驗收工程師(" + _context.AppUsers.Find(r.EngId).FullName + ")聯絡</p>";
                body += "<h3>this is a inform letter from system manager.Do not reply for it.</h3>";
                mail.message.Body = body;
                mail.message.IsBodyHtml = true;
                //mail.SendMail();
                return new JsonResult(DelivFlow)
                {
                    Value = new { success = true, error = "" }
                };
            }
            else
            {
                string msg = "";
                foreach (var error in ViewData.ModelState.Values.SelectMany(modelState => modelState.Errors))
                {
                    msg += error.ErrorMessage + Environment.NewLine;
                }
                throw new Exception(msg);
            }

        }

        // GET: /BMED/DelivFlow/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /BMED/DelivFlow/Create
        [HttpPost]
        public IActionResult Create(DelivFlowModel delivflow)
        {
            if (ModelState.IsValid)
            {
                _context.DelivFlows.Add(delivflow);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(delivflow);
        }

        // GET: /BMED/DelivFlow/Edit/5
        public IActionResult Edit(string id = null)
        {
            DelivFlowModel delivflow = _context.DelivFlows.Find(id);
            if (delivflow == null)
            {
                return NotFound();
            }
            return View(delivflow);
        }

        //
        // POST: /BMED/DelivFlow/Edit/5

        [HttpPost]
        public IActionResult Edit(DelivFlowModel delivflow)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(delivflow).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(delivflow);
        }

        //
        // GET: /BMED/DelivFlow/Delete/5

        public IActionResult Delete(string id = null)
        {
            DelivFlowModel delivflow = _context.DelivFlows.Find(id);
            if (delivflow == null)
            {
                return NotFound();
            }
            return View(delivflow);
        }

        //
        // POST: /BMED/DelivFlow/Delete/5

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(string id)
        {
            DelivFlowModel delivflow = _context.DelivFlows.Find(id);
            _context.DelivFlows.Remove(delivflow);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult GetNextEmp(string cls = null, string docid = null, string vendor = null)
        {
            // Get Login User's details.
            var ur = _userRepo.Find(usr => usr.UserName == User.Identity.Name).FirstOrDefault();

            List<SelectListItem> list = new List<SelectListItem>();
            List<string> s;
            SelectListItem li;
            DeliveryModel r = _context.Deliveries.Find(docid);
            //string c = db.UserProfiles.Find(r.UserId).CustId;
            string c = _context.AppUsers.Find(ur.Id).DptId;
            //string g = _context.CustOrgans.Find(c).GroupId;
            AppUserModel u;
            List<AppUserModel> uv;
            switch (cls)
            {
                case "設備工程師":
                    s = roleManager.GetUsersInRole("MedEngineer").ToList();
                    foreach (string l in s)
                    {
                        u = _context.AppUsers.Where(usr => usr.UserName == l).FirstOrDefault();
                        if (u != null)
                        {
                            li = new SelectListItem();
                            li.Text = "(" + u.UserName + ")" + u.FullName;
                            li.Value = u.Id.ToString();
                            if (li.Value == Convert.ToString(r.EngId))
                                list.Insert(0, li);
                            else
                                list.Add(li);
                        }
                    }
                    //u = db.UserProfiles.Find(r.EngId);
                    //if (u != null)
                    //{
                    //    li = new ListItem();
                    //    li.Text = u.FullName;
                    //    li.Value = u.UserId.ToString();
                    //    list.Add(li);
                    //}
                    break;
                case "設備主管":
                    s = roleManager.GetUsersInRole("MedMgr").ToList();
                    foreach (string l in s)
                    {
                        u = _context.AppUsers.Where(usr => usr.UserName == l).FirstOrDefault();
                        if (u != null)
                        {
                            if (u.Status == "Y")
                            {
                                li = new SelectListItem();
                                li.Text = "(" + u.UserName + ")" + u.FullName;
                                li.Value = u.Id.ToString();
                                list.Add(li);
                            }
                        }
                    }
                    break;
                case "單位主管":
                    s = roleManager.GetUsersInRole("Manager").ToList();
                    string dptid = _context.AppUsers.Find(r.UserId).DptId;
                    list = new List<SelectListItem>();
                    foreach (string l in s)
                    {
                        u = _context.AppUsers.Where(usr => usr.UserName == l).FirstOrDefault();
                        if (u != null)
                        {
                            if (u.DptId == dptid)
                            {
                                li = new SelectListItem();
                                li.Text = "(" + u.UserName + ")" + u.FullName;
                                li.Value = u.Id.ToString();
                                list.Add(li);
                            }
                        }
                    }
                    break;
                case "申請者":
                    if (r != null)
                    {
                        u = _context.AppUsers.Find(r.UserId);
                        if (u != null)
                        {
                            list = new List<SelectListItem>();
                            li = new SelectListItem();
                            li.Text = "(" + u.UserName + ")" + r.UserName;
                            li.Value = r.UserId.ToString();
                            list.Add(li);
                        }
                    }
                    break;
                case "採購人員":
                    if (r != null)
                    {

                        li = new SelectListItem();
                        li.Text = _context.AppUsers.Find(r.PurchaserId).FullName;
                        li.Value = r.PurchaserId.ToString();
                        list.Add(li);
                    }
                    break;
                case "得標廠商":
                    List<VendorModel> vv = _context.BMEDVendors.Where(v => v.UniteNo == r.VendorId).ToList();
                    foreach (VendorModel v in vv)
                    {
                        uv = _context.AppUsers.Where(u2 => u2.VendorId == v.VendorId).ToList();
                        foreach (AppUserModel u3 in uv)
                        {
                            li = new SelectListItem ();
                            li.Text = u3.FullName;
                            li.Value = u3.Id.ToString();
                            list.Add(li);
                        }
                    }
                    break;
                case "使用單位":
                    DelivFlowModel df = _context.DelivFlows.Where(d => d.DocId == r.DocId)
                                                           .Where(d => d.Cls == "使用單位").FirstOrDefault();
                    if (df != null)
                    {
                        u = _context.AppUsers.Find(df.UserId);
                        if (u != null)
                        {
                            li = new SelectListItem();
                            li.Text = "(" + u.UserName + ")" + u.FullName;
                            li.Value = u.Id.ToString();
                            list.Add(li);
                        }
                    }
                    break;
                case "設備經辦":
                    s = roleManager.GetUsersInRole("MedToDo").ToList();
                    foreach (string l in s)
                    {
                        u = _context.AppUsers.Where(usr => usr.UserName == l).FirstOrDefault();
                        if (u != null)
                        {
                            li = new SelectListItem();
                            li.Text = "(" + u.UserName + ")" + u.FullName;
                            li.Value = u.Id.ToString();
                            list.Add(li);
                        }
                    }
                    break;
                case "採購主管":
                    s = roleManager.GetUsersInRole("BuyerMgr").ToList();
                    foreach (string l in s)
                    {
                        u = _context.AppUsers.Where(usr => usr.UserName == l).FirstOrDefault();
                        if (u != null)
                        {
                            li = new SelectListItem();
                            li.Text = "(" + u.UserName + ")" + u.FullName;
                            li.Value = u.Id.ToString();
                            list.Add(li);
                        }
                    }
                    break;
                default:
                    break;
            }
            return Json(list);
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}