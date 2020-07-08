using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using EDIS.Areas.BMED.Data;
using EDIS.Areas.BMED.Models.BuyEvaluateModels;
using EDIS.Models.Identity;
using EDIS.Repositories;
using EDIS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;
using EDIS.Areas.BMED.Models;
using System.Data.SqlClient;

namespace EDIS.Areas.BMED.Controllers
{
    [Area("BMED")]
    [Authorize]
    public class BuyFlowController : Controller
    {
        private BMEDDbContext _context;
        private readonly IRepository<AppUserModel, int> _userRepo;
        private readonly IRepository<DepartmentModel, string> _dptRepo;
        private readonly IEmailSender _emailSender;
        private readonly CustomUserManager userManager;
        private readonly CustomRoleManager roleManager;

        public BuyFlowController(BMEDDbContext context,
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
        //
        // GET: /BuyFlow/

        public ActionResult Index()
        {
            return View(_context.BuyFlows.ToList());
        }

        public ActionResult GetList(string id = null, string sid = null)
        {
            List<BuyFlowModel> rf = _context.BuyFlows.Where(f => f.DocId == id).ToList();
            AppUserModel p;
            foreach (BuyFlowModel f in rf)
            {
                p = _context.AppUsers.Find(f.UserId);
                f.UserNam = p.UserName + "(" + p.FullName + ")";
                if (f.Status == "?")
                    ViewData["cls_now"] = f.Cls;
            }
            if (sid != null)
            {
                List<BuyFlowModel> rf2 = _context.BuyFlows.Where(f => f.DocId == sid).ToList();
                foreach (BuyFlowModel f in rf2)
                {
                    p = _context.AppUsers.Find(f.UserId);
                    f.UserNam = p.UserName + "(" + p.FullName + ")";
                    if (f.Status == "?")
                        ViewData["cls_now"] = f.Cls;
                    rf.Add(f);
                }
            }
            return PartialView(rf);
        }

        public ActionResult GetSList(string id = null)
        {
            List<BuySFlowModel> sf = _context.BuySFlows.Where(f => f.DocId == id).ToList();
            List<BuyFlowModel> rf = new List<BuyFlowModel>();
            List<BuyFlowModel> rf2;
            AppUserModel p;
            foreach (BuySFlowModel f in sf)
            {
                rf2 = _context.BuyFlows.Where(bf => bf.DocId == f.DocSid).ToList();
                foreach (BuyFlowModel f2 in rf2)
                {
                    p = _context.AppUsers.Find(f2.UserId);
                    f2.UserNam = p.UserName + "(" + p.FullName + ")";
                    rf.Add(f2);
                }
            }
  
            return PartialView(rf);
        }

        //
        // GET: /BuyFlow/Details/5

        public ActionResult Details(string id = null)
        {
            BuyFlowModel buyflow = _context.BuyFlows.Find(id);
            if (buyflow == null)
            {
                return NotFound();
            }
            return View(buyflow);
        }

        //
        // GET: /BuyFlow/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /BuyFlow/Create

        [HttpPost]
        public ActionResult Create(BuyFlowModel buyflow)
        {
            if (ModelState.IsValid)
            {
                _context.BuyFlows.Add(buyflow);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(buyflow);
        }

        //
        // GET: /BuyFlow/Edit/5

        public ActionResult Edit(string id = null)
        {
            BuyFlowModel buyflow = _context.BuyFlows.Find(id);
            if (buyflow == null)
            {
                return NotFound();
            }
            return View(buyflow);
        }

        //
        // POST: /BuyFlow/Edit/5

        [HttpPost]
        public ActionResult Edit(BuyFlowModel buyflow)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(buyflow).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(buyflow);
        }

        //
        // GET: /BuyFlow/Delete/5

        public ActionResult Delete(string id = null)
        {
            BuyFlowModel buyflow = _context.BuyFlows.Find(id);
            if (buyflow == null)
            {
                return NotFound();
            }
            return View(buyflow);
        }

        //
        // POST: /BuyFlow/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            BuyFlowModel buyflow = _context.BuyFlows.Find(id);
            _context.BuyFlows.Remove(buyflow);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult InEvaluate(string docid = null)
        {
            BuyFlowModel buyflow = _context.BuyFlows.Where(b => b.DocId == docid)
                                                    .Where(b => b.Status == "?").FirstOrDefault();
            if (buyflow == null)
            {
                return NotFound();
            }
            else
            {
                buyflow.Status = "E";
                buyflow.Rtt = DateTime.Now;
                _context.Entry(buyflow).State = EntityState.Modified;
                //
                BuyEvaluateModel e = _context.BuyEvaluates.Find(docid);
                BuySFlowModel s = new BuySFlowModel();
                s.DocId = docid;
                s.DocSid = buyflow.DocId + "_1";
                s.StepId = 2;
                s.Status = "?";
                s.Rtp = buyflow.UserId;
                s.Rtt = DateTime.Now;
                _context.BuySFlows.Add(s);
                BuySFlowModel s2 = new BuySFlowModel();
                s2.DocId = docid;
                s2.DocSid = buyflow.DocId + "_2";
                s2.StepId = 2;
                s2.Status = "?";
                s2.Rtp = buyflow.UserId;
                s2.Rtt = DateTime.Now;
                _context.BuySFlows.Add(s2);
                //
                BuyFlowModel bf = new BuyFlowModel();
                BuyFlowModel bf2 = new BuyFlowModel();
                bf.DocId = buyflow.DocId + "_1";
                bf2.DocId = buyflow.DocId + "_2";
                bf.StepId = 1;
                bf2.StepId = 1;
                bf.UserId = buyflow.UserId;
                bf2.UserId = buyflow.UserId;
                bf.Status = "1";
                bf2.Status = "1";
                bf.Cls = "採購人員";
                bf2.Cls = "採購人員";
                bf.Rtp = buyflow.UserId;
                bf2.Rtp = buyflow.UserId;
                bf.Rtt = DateTime.Now;
                bf2.Rtt = DateTime.Now;
                _context.Entry(buyflow).State = EntityState.Modified;
                _context.BuyFlows.Add(bf);
                _context.BuyFlows.Add(bf2);
                //                
                bf.StepId = 2;
                bf2.StepId = 2;
                bf.UserId = e.UserId;
                bf2.UserId = e.EngId;
                bf.Status = "?";
                bf2.Status = "?";
                bf.Cls = "申請者";
                bf2.Cls = "維修工程師";
                bf.Rtp = buyflow.UserId;
                bf2.Rtp = buyflow.UserId;
                bf.Rtt = DateTime.Now;
                bf2.Rtt = DateTime.Now;
                _context.BuyFlows.Add(bf);
                _context.BuyFlows.Add(bf2);
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        public ActionResult NextFlow(string id = null, string sid = null)
        {
            BuyFlowModel rf;
            if (sid != null)
                rf = _context.BuyFlows.Where(f => f.DocId == sid && f.Status == "?").FirstOrDefault();
            else
                rf = _context.BuyFlows.Where(f => f.DocId == id && f.Status == "?").FirstOrDefault();
            //rf.Docid = id;
            List<SelectListItem> listItem = new List<SelectListItem>();
            if (sid == null)
            {
                listItem.Add(new SelectListItem { Text = "設備工程師", Value = "設備工程師" });
                listItem.Add(new SelectListItem { Text = "申請者", Value = "申請者" });
                //listItem.Add(new SelectListItem { Text = "採購人員", Value = "採購人員" });
                listItem.Add(new SelectListItem { Text = "評估工程師", Value = "評估工程師" });
                listItem.Add(new SelectListItem { Text = "設備主管", Value = "設備主管" });
                listItem.Add(new SelectListItem { Text = "設備經辦", Value = "設備經辦" });
                //listItem.Add(new SelectListItem { Text = "採購主管", Value = "採購主管" });
                if (rf.Cls == "設備經辦")
                    listItem.Add(new SelectListItem { Text = "結案", Value = "結案" });
            }
            else
            {
                if (rf.Cls == "申請者")
                {
                    listItem.Add(new SelectListItem { Text = "設備工程師", Value = "設備工程師" });
                    listItem.Add(new SelectListItem { Text = "廢除", Value = "廢除" });
                }
                else if (rf.Cls == "設備工程師")
                    listItem.Add(new SelectListItem { Text = "設備主管", Value = "設備主管" });
                else if (rf.Cls == "單位主管")
                {
                    listItem.Add(new SelectListItem { Text = "申請者", Value = "申請者" });
                    //listItem.Add(new SelectListItem { Text = "結案", Value = "結案" });
                }
                else if (rf.Cls == "設備主管")
                {
                    listItem.Add(new SelectListItem { Text = "設備工程師", Value = "設備工程師" });
                    listItem.Add(new SelectListItem { Text = "設備經辦", Value = "設備經辦" });
                    //listItem.Add(new SelectListItem { Text = "結案", Value = "結案" });
                }
                else if (rf.Cls == "設備經辦")
                {
                    listItem.Add(new SelectListItem { Text = "設備主管", Value = "設備主管" });
                    listItem.Add(new SelectListItem { Text = "結案", Value = "結案" });
                }
            }
            ViewData["Item"] = new SelectList(listItem, "Value", "Text", "");
            //
            List<SelectListItem> listItem2 = new List<SelectListItem>();
            listItem2.Add(new SelectListItem { Text = "", Value = "" });
            ViewData["Item2"] = new SelectList(listItem2, "Value", "Text", "");
            rf.Cls = "";
            //
            BuyEvaluateModel ra = _context.BuyEvaluates.Find(id);
            if (ra != null)
            {
                string cid = _context.AppUsers.Find(ra.UserId).DptId;
                //string gid = _context.Departments.Find(cid).GroupId;
                //if (gid != null)
                //{
                //    rf.FlowHint = _context.Groups.Find(gid).FlowHint3;
                //}
            }
            rf.SelOpin = "同意";

            return PartialView(rf);
        }

        [HttpPost]
        public ActionResult NextFlow(BuyFlowModel BuyFlow)
        {
            // Get Login User's details.
            var loginUser = _userRepo.Find(ur => ur.UserName == User.Identity.Name).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (BuyFlow.SelOpin == "其他" && string.IsNullOrEmpty(BuyFlow.Opinions))
                {
                    throw new Exception("請填寫意見欄!!");
                }
                BuyFlowModel rf = _context.BuyFlows.Where(f => f.DocId == BuyFlow.DocId && f.Status == "?").FirstOrDefault();
                List<BuyFlowModel> rflist = _context.BuyFlows.Where(f => f.DocId == BuyFlow.DocId).ToList();
                BuyEvaluateModel r;
                BuySFlowModel s = _context.BuySFlows.Where(b => b.DocSid == BuyFlow.DocId).FirstOrDefault();
                if (s != null)
                    r = _context.BuyEvaluates.Find(s.DocId);
                else
                    r = _context.BuyEvaluates.Find(BuyFlow.DocId);
                AppUserModel u;
                Tmail mail;
                string body = "";
                string sto = "";
                if (BuyFlow.Cls == "結案")
                {
                    rf.Opinions = BuyFlow.SelOpin + Environment.NewLine + BuyFlow.Opinions;
                    rf.Status = "2";
                    rf.Rtt = DateTime.Now;
                    rf.Rtp = loginUser.Id;
                    if (s != null)
                    {
                        s.Status = "2";
                        s.Rtt = DateTime.Now;
                        s.Rtp = loginUser.Id;
                        _context.Entry(s).State = EntityState.Modified;
                    }
                    _context.Entry(rf).State = EntityState.Modified;
                    _context.SaveChanges();
                    //
                    if (s != null) //會簽
                    {
                        List<BuySFlowModel> sf = _context.BuySFlows.Where(bs => bs.DocId == s.DocId)
                            .Where(bs => bs.Status == "?").ToList();
                        if (sf.Count() == 0)
                        {
                            BuyFlowModel f = _context.BuyFlows.Where(bf => bf.DocId == s.DocId)
                                .Where(bf => bf.Status == "E").FirstOrDefault();
                            if (f != null)
                            {
                                f.Status = "?";
                                _context.Entry(f).State = EntityState.Modified;
                                _context.SaveChanges();
                                //
                                mail = new Tmail();
                                u = _context.AppUsers.Find(loginUser.Id);
                                mail.from = new System.Net.Mail.MailAddress(u.Email); //u.Email
                                mail.to = new System.Net.Mail.MailAddress(_context.AppUsers.Find(f.UserId).Email);
                                //mail.cc = new System.Net.Mail.MailAddress("99242@cch.org.tw");
                                mail.message.Subject = "醫療儀器管理資訊系統[採購評估案-會簽完成通知]：儀器名稱： " + r.PlantCnam;
                                body += "<p>申請人：" + r.UserName + "</p>";
                                body += "<p>儀器名稱：" + r.PlantCnam + "</p>";
                                body += "<br/>";
                                body += "<h3>this is a inform letter from system manager.Do not reply for it.</h3>";
                                mail.message.Body = body;
                                mail.message.IsBodyHtml = true;
                                //mail.SendMail();
                            }
                        }
                    }
                    else
                    {
                        mail = new Tmail();
                        u = _context.AppUsers.Find(loginUser.Id);
                        mail.from = new System.Net.Mail.MailAddress(u.Email); //u.Email
                        foreach (BuyFlowModel rr in rflist)
                        {
                            u = _context.AppUsers.Find(rr.UserId);
                            sto += u.Email + ",";
                        }
                        mail.sto = sto.TrimEnd(new char[] { ',' });
                        //mail.cc = new System.Net.Mail.MailAddress("99242@cch.org.tw");
                        mail.message.Subject = "醫療儀器管理資訊系統[採購評估案-結案通知]：儀器名稱： " + r.PlantCnam;
                        body += "<p>申請人：" + r.UserName + "</p>";
                        body += "<p>儀器名稱：" + r.PlantCnam + "</p>";
                        body += "<p><a href='http://dms.cch.org.tw/MvcMedEngMgr'>前往網站檢視</a></p>";
                        body += "<br/>";
                        body += "<h3>this is a inform letter from system manager.Do not reply for it.</h3>";
                        mail.message.Body = body;
                        mail.message.IsBodyHtml = true;
                        //mail.SendMail();
                    }
                    return new JsonResult(BuyFlow)
                    {
                        Value = new { success = true, error = "" },
                    };
                }
                else if (BuyFlow.Cls == "廢除")
                {
                    rf.Opinions = BuyFlow.SelOpin + Environment.NewLine + BuyFlow.Opinions;
                    rf.Status = "3";
                    rf.Rtt = DateTime.Now;
                    rf.Rtp = loginUser.Id;
                    _context.Entry(rf).State = EntityState.Modified;
                    _context.SaveChanges();
                    //
                    mail = new Tmail();
                    u = _context.AppUsers.Find(loginUser.Id);
                    mail.from = new System.Net.Mail.MailAddress(u.Email); //u.Email
                    foreach (BuyFlowModel rr in rflist)
                    {
                        u = _context.AppUsers.Find(rr.UserId);
                        sto += u.Email + ",";
                    }
                    mail.sto = sto.TrimEnd(new char[] { ',' });
                    //mail.cc = new System.Net.Mail.MailAddress("99242@cch.org.tw");
                    mail.message.Subject = "醫療儀器管理資訊系統[採購評估案-廢除通知]：儀器名稱： " + r.PlantCnam;
                    body += "<p>申請人：" + r.UserName + "</p>";
                    body += "<p>儀器名稱：" + r.PlantCnam + "</p>";
                    body += "<br/>";
                    body += "<h3>this is a inform letter from system manager.Do not reply for it.</h3>";
                    mail.message.Body = body;
                    mail.message.IsBodyHtml = true;
                    //mail.SendMail();
                    return new JsonResult(BuyFlow)
                    {
                        Value = new { success = true, error = "" },
                    };
                }
                BuyFlow.StepId = rf.StepId + 1;
                BuyFlow.Rtt = DateTime.Now;
                switch (BuyFlow.Cls)
                {
                    case "申請者":
                        BuyFlow.UserId = r.UserId;
                        break;
                    case "設備經辦":
                        
                        break;
                }
                BuyFlow.Status = "?";
                u = _context.AppUsers.Find(BuyFlow.UserId);
                BuyFlow.Role = roleManager.GetRolesForUser(u.Id).FirstOrDefault();
                rf.Opinions = BuyFlow.SelOpin + Environment.NewLine + BuyFlow.Opinions;
                rf.Status = "1";
                rf.Rtp = loginUser.Id;
                BuyFlow.Opinions = null;
                _context.Entry(rf).State = EntityState.Modified;
                _context.BuyFlows.Add(BuyFlow);
                _context.SaveChanges();
                //
                mail = new Tmail();
                body = "";
                u = _context.AppUsers.Find(loginUser.Id);
                mail.from = new System.Net.Mail.MailAddress(u.Email); //u.Email
                u = _context.AppUsers.Find(BuyFlow.UserId);
                mail.to = new System.Net.Mail.MailAddress(u.Email); //u.Email
                //mail.cc = new System.Net.Mail.MailAddress("99242@cch.org.tw");
                mail.message.Subject = "醫療儀器管理資訊系統[採購評估案]：儀器名稱： " + r.PlantCnam;
                body += "<p>申請人：" + r.UserName + "</p>";
                body += "<p>儀器名稱：" + r.PlantCnam + "</p>";
                body += "<p><a href='http://dms.cch.org.tw/MvcMedEngMgr'>處理案件</a></p>";
                body += "<br/>";
                body += "<h3>this is a inform letter from system manager.Do not reply for it.</h3>";
                mail.message.Body = body;
                mail.message.IsBodyHtml = true;
                //mail.SendMail();

                return new JsonResult(BuyFlow)
                {
                    Value = new { success = true, error = "" },
                };
            }

            return View(BuyFlow);
        }

        public JsonResult GetNextEmp(string cls = null, string docid = null, string vendor = null)
        {
            // Get Login User's details.
            var loginUser = _userRepo.Find(ur => ur.UserName == User.Identity.Name).FirstOrDefault();
            List<SelectListItem> list = null;
            List<string> s;
            SelectListItem li;
            AppUserModel u;
            BuyEvaluateModel r = _context.BuyEvaluates.Find(docid);
            string c = _context.AppUsers.Find(loginUser.Id).DptId;
            //string g = _context.Departments.Find(c).GroupId;
            //string g = _context.CustOrgans.Find(_context.UserProfiles.Find(r.UserId).CustId).GroupId;
            switch (cls)
            {
                case "設備工程師":
                    //s = Roles.GetUsersInRole("Engineer").ToList();
                    list = new List<SelectListItem>();
                    AppUserModel u2 = _context.AppUsers.Find(r.EngId);
                    li = new SelectListItem();
                    li.Text = u2.FullName;
                    li.Value = u2.Id.ToString();
                    list.Add(li);
                    break;
                case "評估工程師":
                    list = new List<SelectListItem>();
                    s = roleManager.GetUsersInRole("MedEngineer").ToList();
                    foreach (string l in s)
                    {
                        u = _context.AppUsers.Where(ur => ur.UserName == l).FirstOrDefault();
                        if (u != null)
                        {
                            li = new SelectListItem();
                            li.Text = u.FullName;
                            li.Value = u.Id.ToString();
                            list.Add(li);
                        }
                    }
                    break;
                case "設備主管":
                    list = new List<SelectListItem>();
                    s = roleManager.GetUsersInRole("MedMgr").ToList();
                    foreach (string l in s)
                    {
                        u = _context.AppUsers.Where(ur => ur.UserName == l).FirstOrDefault();
                        if (u != null)
                        {
                            if (u.Status == "Y")
                            {
                                li = new SelectListItem();
                                li.Text = u.FullName;
                                li.Value = u.Id.ToString();
                                list.Add(li);
                            }
                        }
                    }
                    break;
                case "採購主管":
                    s = roleManager.GetUsersInRole("BuyerMgr").ToList();
                    list = new List<SelectListItem>();
                    foreach (string l in s)
                    {
                        u = _context.AppUsers.Where(ur => ur.UserName == l).FirstOrDefault();
                        if (u != null)
                        {
                            li = new SelectListItem();
                            li.Text = u.FullName;
                            li.Value = u.Id.ToString();
                            list.Add(li);
                        }
                    }
                    break;
                case "單位主管":
                    s = roleManager.GetUsersInRole("Manager").ToList();
                    c = _context.AppUsers.Find(r.UserId).DptId;
                    list = new List<SelectListItem>();
                    foreach (string l in s)
                    {
                        u = _context.AppUsers.Where(ur => ur.UserName == l).FirstOrDefault();
                        if (u != null)
                        {
                            DepartmentModel o = _context.Departments.Find(u.DptId);
                            if (u.DptId == c)
                            {
                                li = new SelectListItem();
                                li.Text = u.FullName;
                                li.Value = u.Id.ToString();
                                list.Add(li);
                            }
                        }
                    }
                    break;
                case "設備經辦":
                    s = roleManager.GetUsersInRole("MedToDo").ToList();
                    list = new List<SelectListItem>();
                    foreach (string l in s)
                    {
                        u = _context.AppUsers.Where(ur => ur.UserName == l).FirstOrDefault();
                        if ( u != null)
                        {
                            if (u.Status == "Y")
                            {
                                li = new SelectListItem();
                                li.Text = u.FullName;
                                li.Value = u.Id.ToString();
                                list.Add(li);
                            }
                        }
                    }
                    break;
                case "申請者":
                    if (r != null)
                    {
                        list = new List<SelectListItem>();
                        li = new SelectListItem();
                        li.Text = r.UserName;
                        li.Value = r.UserId.ToString();
                        list.Add(li);
                    }
                    break;
                case "採購人員":
                    if (r != null)
                    {
                        list = new List<SelectListItem>();
                        li = new SelectListItem();
                        li.Text = r.PurchaserName;
                        li.Value = r.PurchaserId.ToString();
                        list.Add(li);
                    }
                    break;
                default:
                    list = new List<SelectListItem>();
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