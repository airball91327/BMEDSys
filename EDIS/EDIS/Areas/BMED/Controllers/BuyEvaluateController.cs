using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using EDIS.Areas.BMED.Data;
using EDIS.Areas.BMED.Models.DeliveryModels;
using EDIS.Areas.BMED.Models.BuyEvaluateModels;
using EDIS.Areas.BMED.Models.RepairModels;
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
using OfficeOpenXml;

namespace EDIS.Areas.BMED.Controllers
{
    [Area("BMED")]
    [Authorize]
    public class BuyEvaluateController : Controller
    {
        private BMEDDbContext _context;
        private readonly IRepository<AppUserModel, int> _userRepo;
        private readonly IRepository<DepartmentModel, string> _dptRepo;
        private readonly IEmailSender _emailSender;
        private readonly CustomUserManager userManager;
        private readonly CustomRoleManager roleManager;

        public BuyEvaluateController(BMEDDbContext context,
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
        // GET: /BuyEvaluate/
        public IActionResult Index()
        {
            // Get Login User's details.
            var user = _userRepo.Find(ur => ur.UserName == User.Identity.Name).FirstOrDefault();

            List<BuyEvaluateModel> rv = new List<BuyEvaluateModel>();
            List<BuyFlowModel> rf2;
            BuyEvaluateModel b;
            rf2 = _context.BuyFlows.Where(bf => bf.Status == "?")
                                   .Where(m => m.UserId != user.Id).ToList();
            foreach (BuyFlowModel f in rf2)
            {
                b = _context.BuyEvaluates.Find(f.DocId);
                if (b != null)
                    rv.Add(b);
            }
            return View(rv);
        }

        //
        // GET: /BuyEvaluate/Details/5
        public IActionResult Details(string id = null)
        {
            BuyEvaluateModel buyevaluate = _context.BuyEvaluates.Find(id);
            if (buyevaluate == null)
            {
                return NotFound();
            }
            buyevaluate.EngName = _context.AppUsers.Find(buyevaluate.EngId).FullName;
            var dpt = _context.Departments.Find(buyevaluate.AccDpt);
            if (dpt != null)
            {
                buyevaluate.AccDptNam = dpt.Name_C;
            }
            return View(buyevaluate);
        }

        //
        // GET: /BuyEvaluate/Create
        public IActionResult Create()
        {
            // Get Login User's details.
            var u = _userRepo.Find(ur => ur.UserName == User.Identity.Name).FirstOrDefault();
            BuyEvaluateModel r = new BuyEvaluateModel();
            DepartmentModel d = _context.Departments.Find(u.DptId);
            VendorModel v = _context.BMEDVendors.Find(u.VendorId);
            r.DocId = r.GetID(ref _context);
            r.UserId = u.Id;
            r.UserName = u.FullName;
            r.Company = d.DptId == null ? "" : d.Name_C;
            r.AccDpt = d.DptId == null ? "" : d.DptId;
            r.AccDptNam = d.DptId == null ? "" : d.Name_C;
            r.Contact = u.Mobile;
            r.PlantType = "醫療儀器";
            r.Place = r.AccDptNam;
            r.Rtt = DateTime.Now;
            _context.BuyEvaluates.Add(r);
            _context.SaveChanges();
            List<SelectListItem> listItem = new List<SelectListItem>();
            List<SelectListItem> listItem2 = new List<SelectListItem>();
            List<SelectListItem> listItem3 = new List<SelectListItem>();
            listItem3.Add(new SelectListItem { Text = "醫療儀器", Value = "醫療儀器" });
            listItem3.Add(new SelectListItem { Text = "資訊設備", Value = "資訊設備" });
            ViewData["PTYPE"] = new SelectList(listItem3, "Value", "Text", "醫療儀器");
            string[] eng = roleManager.GetUsersInRole("MedEngineer");
            string[] buyer = roleManager.GetUsersInRole("Buyer");
            AppUserModel p;
            foreach (string s in eng)
            {
                p = _context.AppUsers.Where(ur => ur.UserName == s).FirstOrDefault();
                if (p != null)
                {
                    listItem.Add(new SelectListItem { Text = "(" + p.UserName + ")" + p.FullName, Value = p.Id.ToString() });
                }
            }
            ViewData["ENG"] = new SelectList(listItem, "Value", "Text");
            //
            foreach (string s2 in buyer)
            {
                p = _context.AppUsers.Where(ur => ur.UserName == s2).FirstOrDefault();
                if (p != null)
                {
                    listItem2.Add(new SelectListItem { Text = "(" + p.UserName + ")" + p.FullName, Value = p.Id.ToString() });
                }
            }
            ViewData["PUR"] = new SelectList(listItem2, "Value", "Text");
            return View(r);
        }

        //
        // POST: /BuyEvaluate/Create
        [HttpPost]
        public IActionResult Create(BuyEvaluateModel buyevaluate)
        {
            if (ModelState.IsValid)
            {
                // Get Login User's details.
                var user = _userRepo.Find(ur => ur.UserName == User.Identity.Name).FirstOrDefault();
                DepartmentModel dpt = _context.Departments.Where(d => d.Name_C == buyevaluate.AccDptNam).FirstOrDefault();
                if (dpt != null)
                    buyevaluate.AccDpt = dpt.DptId;
                else
                {
                    ModelState.AddModelError("", "成本中心無此資料!!");
                    return View(buyevaluate);
                }
                buyevaluate.Rtp = user.Id;
                buyevaluate.Rtt = DateTime.Now;
                _context.Entry(buyevaluate).State = EntityState.Modified;
                //
                BuyFlowModel rf = new BuyFlowModel();
                rf.DocId = buyevaluate.DocId;
                rf.StepId = 1;
                rf.UserId = user.Id;
                rf.Status = "1";
                rf.Role = roleManager.GetRolesForUser(user.Id).GetValue(0).ToString();
                rf.Rtp = user.Id;
                rf.Rdt = null;
                rf.Rtt = DateTime.Now;
                rf.Cls = "申請者";
                _context.BuyFlows.Add(rf);
                //
                rf = new BuyFlowModel();
                rf.DocId = buyevaluate.DocId;
                rf.StepId = 2;
                rf.UserId = buyevaluate.EngId;
                rf.Status = "?";
                AppUserModel u = _context.AppUsers.Find(buyevaluate.EngId);
                rf.Role = roleManager.GetRolesForUser(u.Id).FirstOrDefault();
                rf.Rtp = null;
                rf.Rdt = null;
                rf.Rtt = DateTime.Now;
                rf.Cls = "設備工程師";
                _context.BuyFlows.Add(rf);
                //
                _context.SaveChanges();
                //
                Tmail mail = new Tmail();
                string body = "";
                u = _context.AppUsers.Find(user.Id);
                mail.from = new System.Net.Mail.MailAddress(u.Email); //u.Email
                u = _context.AppUsers.Find(buyevaluate.PurchaserId);
                mail.to = new System.Net.Mail.MailAddress(u.Email); //u.Email
                //mail.cc = new System.Net.Mail.MailAddress("99242@cch.org.tw");
                mail.message.Subject = "醫療儀器管理資訊系統[採購評估案]：儀器名稱： " + buyevaluate.PlantCnam;
                body += "<p>申請人：" + buyevaluate.UserName + "</p>";
                body += "<p>儀器名稱：" + buyevaluate.PlantCnam + "</p>";
                body += "<p><a href='http://dms.cch.org.tw/MvcMedEngMgr'>處理案件</a></p>";
                body += "<br/>";
                body += "<h3>this is a inform letter from system manager. Do not reply for it.</h3>";
                mail.message.Body = body;
                mail.message.IsBodyHtml = true;
                //mail.SendMail();
                return RedirectToAction("Index", "Home", new { Area = "" });
            }

            return View(buyevaluate);
        }

        //
        [HttpPost]
        public string UpdStandard(string docid, string standard)
        {
            if (standard != null)
            {
                try
                {
                    BuyEvaluateModel buyeval = _context.BuyEvaluates.Find(docid);
                    buyeval.Standard = standard;
                    _context.Entry(buyeval).State = EntityState.Modified;
                    _context.SaveChanges();
                    return "儲存成功!";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
            return "無規格資料!";
        }

        public IActionResult UpdAgreeDate(string docid, bool isagree)
        {
            try
            {
                BuyEvaluateModel buyeval = _context.BuyEvaluates.Find(docid);
                if (isagree)
                    buyeval.AgreeDate = DateTime.Now;
                else
                    buyeval.AgreeDate = null;
                _context.Entry(buyeval).State = EntityState.Modified;
                _context.SaveChanges();
                return new JsonResult(buyeval)
                {
                    Value = new { success = true, error = "" },
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //
        public IActionResult Update(string id)
        {
            BuyEvaluateModel buyevaluate = _context.BuyEvaluates.Find(id);
            if (buyevaluate == null)
            {
                return NotFound();
            }
            DepartmentModel d = _context.Departments.Find(buyevaluate.AccDpt);
            if (d != null)
                buyevaluate.AccDptNam = d.Name_C;
            AppUserModel u = _context.AppUsers.Find(buyevaluate.EngId);
            if (u != null)
                buyevaluate.EngName = u.FullName;
            List<SelectListItem> listItem = new List<SelectListItem>();
            List<SelectListItem> listItem2 = new List<SelectListItem>();
            string[] eng = roleManager.GetUsersInRole("Engineer");
            string[] buyer = roleManager.GetUsersInRole("Buyer");
            AppUserModel p;
            foreach (string s in eng)
            {
                p = _context.AppUsers.Where(ur => ur.UserName == s).FirstOrDefault();
                if (p != null)
                {
                    listItem.Add(new SelectListItem { Text = "(" + p.UserName + ")" + p.FullName, Value = p.Id.ToString() });
                }
            }
            ViewData["ENG"] = new SelectList(listItem, "Value", "Text");
            //
            listItem2.Add(new SelectListItem { Text = "醫療儀器", Value = "醫療儀器" });
            listItem2.Add(new SelectListItem { Text = "資訊設備", Value = "資訊設備" });
            ViewData["PTYPE"] = new SelectList(listItem2, "Value", "Text", "醫療儀器");
            //
            return View(buyevaluate);
        }

        [HttpPost]
        public IActionResult Update(BuyEvaluateModel buyevaluate)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(buyevaluate).State = EntityState.Modified;
                _context.SaveChanges();
                return new JsonResult(buyevaluate)
                {
                    Value = new { success = true, error = "" },
                };
            }
            return View(buyevaluate);
        }

        //
        // GET: /BuyEvaluate/Edit/5
        public IActionResult Edit(string id = null, string sid = null)
        {
            BuyEvaluateModel buyevaluate = _context.BuyEvaluates.Find(id);
            if (buyevaluate == null)
            {
                return NotFound();
            }
            buyevaluate.DocSid = sid;
            if (buyevaluate.AgreeDate != null)
                buyevaluate.IsAgree = true;
            DepartmentModel d = _context.Departments.Find(buyevaluate.AccDpt);
            if (d != null)
                buyevaluate.AccDptNam = d.Name_C;
            AppUserModel u = _context.AppUsers.Find(buyevaluate.EngId);
            if (u != null)
                buyevaluate.EngName = u.FullName;
            BudgetModel bgt = _context.Budgets.Find(buyevaluate.BudgetId);
            if (bgt != null)
            {
                buyevaluate.Opinion = bgt.Opinion;
                buyevaluate.GrpOpin = bgt.GrpOpin;
            }

            return View(buyevaluate);
        }

        //
        // POST: /BuyEvaluate/Edit/5
        [HttpPost]
        public IActionResult Edit(BuyEvaluateModel buyevaluate)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(buyevaluate).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(buyevaluate);
        }

        //
        // GET: /BuyEvaluate/Delete/5
        public IActionResult Delete(string id = null)
        {
            BuyEvaluateModel buyevaluate = _context.BuyEvaluates.Find(id);
            if (buyevaluate == null)
            {
                return NotFound();
            }

            return View(buyevaluate);
        }

        //
        // POST: /BuyEvaluate/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(string id)
        {
            BuyEvaluateModel buyevaluate = _context.BuyEvaluates.Find(id);
            if (buyevaluate == null)
            {
                return NotFound();
            }
            _context.BuyEvaluates.Remove(buyevaluate);
            List<BuyFlowModel> bf = _context.BuyFlows.Where(f => f.DocId == id).ToList();
            foreach (BuyFlowModel f in bf)
            {
                _context.BuyFlows.Remove(f);
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Members");
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }

        public IActionResult BuyEvaluateList()
        {
            QueryTerms qry = new QueryTerms();
            qry.flowtyp = "待處理";
            return PartialView("_BuyEvaluateList", GetList(qry, ""));
        }

        public IActionResult BuyEvaluateListIndex()
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            listItem.Add(new SelectListItem { Text = "待處理", Value = "待處理" });
            listItem.Add(new SelectListItem { Text = "已處理", Value = "已處理" });
            listItem.Add(new SelectListItem { Text = "已結案", Value = "已結案" });
            ViewData["Item"] = new SelectList(listItem, "Value", "Text", "待處理");
            return PartialView("_BuyEvaluateListIndex", GetList("待處理"));
        }

        public IActionResult ExcelStandard(string sd, string ed)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("院區");
            dt.Columns.Add("表單編號");
            dt.Columns.Add("儀器中文名稱");
            dt.Columns.Add("成本中心代碼");
            dt.Columns.Add("成本中心");
            dt.Columns.Add("申請人部門代號");
            dt.Columns.Add("申請人部門");
            dt.Columns.Add("申請");
            dt.Columns.Add("通過");
            dt.Columns.Add("通過金額");
            dt.Columns.Add("工程師");
            dt.Columns.Add("小組意見");
            dt.Columns.Add("規格");
            dt.Columns.Add("規格日期");

            //
            List<BuyEvaluateModel> vm = _context.BuyEvaluates.Where(f => f.AgreeDate != null).ToList();
            if (!string.IsNullOrEmpty(sd))
            {
                sd = sd.Replace("-", "/");
                vm = vm
                   .Where(f => f.AgreeDate.Value.Date.CompareTo(DateTime.ParseExact(sd, "yyyy/MM/dd", null)) >= 0).ToList();
            }
            if (!string.IsNullOrEmpty(ed))
            {
                ed = ed.Replace("-", "/");
                vm = vm
                   .Where(f => f.AgreeDate.Value.Date.CompareTo(DateTime.ParseExact(ed, "yyyy/MM/dd", null)) <= 0).ToList();
            }
            BudgetModel bg;
            AppUserModel u;
            DepartmentModel c;
            DepartmentModel o;
            DataRow dw;
            foreach (BuyEvaluateModel m in vm)
            {
                bg = _context.Budgets.Find(m.BudgetId);
                u = _context.AppUsers.Find(m.UserId);
                c = _context.Departments.Find(u.DptId);
                dw = dt.NewRow();
                if (bg != null)
                {
                    dw[0] = bg.Loc;
                    dw[1] = m.BudgetId;
                    dw[2] = m.PlantCnam;
                    dw[3] = m.AccDpt;
                    dw[4] = (o = _context.Departments.Find(m.AccDpt)) == null ? "" : o.Name_C;
                    dw[5] = u.DptId;
                    dw[6] = c.Name_C == null ? "" : c.Name_C;
                    dw[7] = bg.Amt;
                    dw[8] = bg.Amt;
                    dw[9] = bg.TotalPrice;
                    dw[10] = bg.EngName;
                    dw[11] = bg.GrpOpin == null ? "" : bg.GrpOpin;
                    dw[12] = m.Standard == null ? "" : m.Standard;
                    dw[13] = m.AgreeDate.Value.ToString("yyyy/MM/dd");
                    dt.Rows.Add(dw);

                }
                else
                {
                    dw[0] = "總院";
                    dw[1] = m.BudgetId;
                    dw[2] = m.PlantCnam;
                    dw[3] = m.AccDpt;
                    dw[4] = (o = _context.Departments.Find(m.AccDpt)) == null ? "" : o.Name_C;
                    dw[5] = u.DptId;
                    dw[6] = c.Name_C == null ? "" : c.Name_C;
                    dw[7] = "";
                    dw[8] = "";
                    dw[9] = "";
                    dw[10] = "";
                    dw[11] = "";
                    dw[12] = m.Standard == null ? "" : m.Standard;
                    dw[13] = m.AgreeDate.Value.ToString("yyyy/MM/dd");
                    dt.Rows.Add(dw);
                }
            }
            //
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            workSheet.Cells[1, 1].LoadFromDataTable(dt, true);

            // Generate the Excel, convert it into byte array and send it back to the controller.
            byte[] fileContents;
            fileContents = excel.GetAsByteArray();

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: "EvaluateStandard.xlsx"
            );
        }

        [HttpPost]
        public IActionResult BuyEvaluateListIndex(IFormCollection form)
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            listItem.Add(new SelectListItem { Text = "待處理", Value = "待處理" });
            listItem.Add(new SelectListItem { Text = "已處理", Value = "已處理" });
            listItem.Add(new SelectListItem { Text = "已結案", Value = "已結案" });
            ViewData["Item"] = new SelectList(listItem, "Value", "Text", form["qtyFLOWTYP"]);
            //
            List<BuyEvaluateListVModel> vm;
            QueryTerms qry = new QueryTerms();
            if (form["qtyDOCID"] != "")
            {
                qry.docid = form["qtyDOCID"];
            }
            if (form["qtyACCNO"] != "")
            {
                qry.accno = form["qtyACCNO"];
            }
            //if (form["qtyASSETNO"] != "")
            //{
            //    qry.assetno = form["qtyASSETNO"];
            //}
            if (form["qtyCNAME"] != "")
            {
                qry.cname = form["qtyCNAME"];
            }
            if (form["qtyDPTID"] != "")
            {
                qry.applydpt = form["qtyDPTID"];
            }
            if (form["qtyFLOWTYP"] != "")
            {
                qry.flowtyp = form["qtyFLOWTYP"];
            }
            if (form["qtyBUDGETID"] != "")
            {
                qry.budgetid = form["qtyBUDGETID"];
            }
            if (form["qtyAGREEDATE1"] != "")
            {
                qry.agreedate1 = form["qtyAGREEDATE1"];
                qry.agreedate1 = qry.agreedate1.Replace("-", "/");
                ViewData["Sdate"] = qry.agreedate1;
            }
            if (form["qtyAGREEDATE2"] != "")
            {
                qry.agreedate2 = form["qtyAGREEDATE2"];
                qry.agreedate2 = qry.agreedate2.Replace("-", "/");
                ViewData["Edate"] = qry.agreedate2;
            }
            if (userManager.IsInRole(User, "BuyerMgr"))
                vm = GetList(qry, "BuyerMgr");
            else
                vm = GetList(qry, "");
            string c = form["qtySTANDARD"];
            if (Convert.ToBoolean(c.Split(new char[] { ',' }).GetValue(0)))
            {
                vm = vm.Where(v => v.IsAgree == true).ToList();
            }
            //
            string[] s = new string[] { "?", "2" };
            vm.ForEach(v =>
            {
                v.DelivCount = _context.Deliveries.Where(d => d.PurchaseNo == v.DocId)
                .Join(_context.DelivFlows, d => d.DocId, f => f.DocId,
                (d, f) => f).Where(f => s.Contains(f.Status)).Count();
            });

            return PartialView("_BuyEvaluateList", vm);
        }

        public List<BuyEvaluateListVModel> GetList(string cls = null)
        {
            // Get Login User's details.
            var user = _userRepo.Find(ur => ur.UserName == User.Identity.Name).FirstOrDefault();
            List<BuyEvaluateListVModel> rv = new List<BuyEvaluateListVModel>();
            List<BuyFlowModel> rf = new List<BuyFlowModel>();
            List<BuyFlowModel> rf2;
            switch (cls)
            {
                case "已處理":
                    rf2 = _context.BuyFlows.Where(bf => bf.Status == "?")
                                           .Where(m => m.UserId != user.Id).ToList();
                    foreach (BuyFlowModel f in rf2)
                    {
                        if (_context.BuyFlows.Where(m => m.DocId == f.DocId).Where(m => m.UserId == user.Id).Count() > 0)
                        {
                            rf.Add(f);
                        }
                    }
                    break;
                case "已結案":
                    rf2 = _context.BuyFlows.Where(bf => bf.Status == "2").ToList();
                    foreach (BuyFlowModel f in rf2)
                    {
                        if (_context.BuyFlows.Where(m => m.DocId == f.DocId).Where(m => m.UserId == user.Id).Count() > 0)
                        {
                            rf.Add(f);
                        }
                    }
                    break;
                case "查詢":
                    rf2 = _context.BuyFlows.Where(bf => bf.Status == "?").ToList();
                    AppUserModel p = user;
                    DepartmentModel c = _context.Departments.Find(p.DptId);
                    BuyEvaluateModel r;
                    foreach (BuyFlowModel f in rf2)
                    {
                        r = _context.BuyEvaluates.Find(f.DocId);
                        rf.Add(f);
                    }
                    break;
                default:
                    rf = _context.BuyFlows.Where(bf => bf.Status == "?")
                                          .Where(m => m.UserId == user.Id).ToList();
                    break;
            }
            rf.OrderByDescending(m => m.Rtt);
            BuyEvaluateListVModel i;
            BuyEvaluateModel r2;
            foreach (BuyFlowModel f in rf)
            {
                i = new BuyEvaluateListVModel();
                BuySFlowModel s = _context.BuySFlows.Where(bs => bs.DocSid == f.DocId).FirstOrDefault();
                if (s != null)
                {
                    r2 = _context.BuyEvaluates.Find(s.DocId);
                    i.DocSid = f.DocId;
                }
                else
                {
                    r2 = _context.BuyEvaluates.Find(f.DocId);
                }
                AppUserModel p = _context.AppUsers.Find(r2.UserId);
                DepartmentModel c = _context.Departments.Find(p.DptId);
                i.DocType = "評估";
                i.DocId = r2.DocId;
                i.UserId = r2.UserId;
                i.UserName = r2.UserName;
                if (p != null && p.DptId != null)
                {
                    i.CustId = p.DptId;
                    i.CustNam = c.Name_C;
                }
                i.PlantCnam = r2.PlantCnam;
                i.PlantEnam = r2.PlantEnam;
                i.PlantType = r2.PlantType;
                i.Days = DateTime.Now.Subtract(r2.Rtt.GetValueOrDefault()).Days;
                if (f.Status == "2")
                {
                    i.Flg = "2";
                    List<DeliveryModel> dv = _context.Deliveries.Where(v => v.PurchaseNo == f.DocId).ToList();
                    List<DelivFlowModel> df;
                    DelivFlowModel w;
                    foreach (DeliveryModel d in dv)
                    {
                        df = _context.DelivFlows.Where(g => g.DocId == d.DocId).ToList();
                        w = df.Where(g => g.Status == "?").FirstOrDefault();
                        if (w != null)
                            i.Flg = "E";
                        w = df.Where(g => g.Status == "2").FirstOrDefault();
                        if (w != null)
                            i.Flg = "E";
                    }
                }
                else
                {
                    i.Flg = f.Status;
                }
                i.FlowUid = f.UserId;
                i.FlowUname = _context.AppUsers.Find(f.UserId).FullName;
                rv.Add(i);
            }
            return rv;
        }

        public List<BuyEvaluateListVModel> GetList(QueryTerms qry, string role)
        {
            // Get Login User's details.
            var user = _userRepo.Find(ur => ur.UserName == User.Identity.Name).FirstOrDefault();
            List<BuyEvaluateListVModel> rv = new List<BuyEvaluateListVModel>();
            List<BuyFlowModel> rf = new List<BuyFlowModel>();
            List<string> rs = new List<string>();
            AppUserModel p = _context.AppUsers.Find(user.Id);
            DepartmentModel d = _context.Departments.Find(p.DptId);

            switch (qry.flowtyp)
            {
                case "已處理":
                    rf = _context.BuyFlows.Where(m => m.Status == "?")
                        .ToList();

                    break;
                case "已結案":
                    rf = _context.BuyFlows.Where(m => m.Status == "2")
                       .ToList();
                    break;

                default:
                    rf = _context.BuyFlows.Where(m => m.Status == "?")
                        .Where(m => m.UserId == user.Id)
                            .ToList();
                    break;
            }
            if (!string.IsNullOrEmpty(qry.docid))
                rf = rf.Where(m => m.DocId == qry.docid).ToList();
            rf = rf.OrderByDescending(m => m.Rtt).ToList();

            if (role == "BuyerMgr")
            {
                rs = rf.Join(_context.BuyEvaluates, m => m.DocId, f => f.DocId,
                    (m, f) => new
                    {
                        m.DocId,
                        f.AccDpt,
                        m.Rtt
                    })
                    .OrderByDescending(f => f.Rtt)
                    .Select(f => f.DocId).Distinct().ToList();
            }
            else
            {
                if (qry.flowtyp == "已處理")
                {
                    rs = rf.Join(_context.BuyFlows, m => m.DocId, f => f.DocId,
                    (m, f) => f)
                    .Where(f => f.UserId == user.Id)
                    .Where(f => f.Status == "1")
                    .Select(f => f.DocId).Distinct().ToList();
                }
                else
                {
                    rs = rf.Select(m => m.DocId).Distinct().ToList();
                }
            }

            var data = rs.Join(_context.BuyEvaluates, m => m, f => f.DocId,
                (m, f) => new
                {
                    f.DocId,
                    f.UserId,
                    f.UserName,
                    f.AccDpt,
                    f.AccDptNam,
                    f.PlantCnam,
                    f.PlantEnam,
                    f.PlantType,
                    f.EngId,
                    f.EngName,
                    f.BudgetId,
                    f.AgreeDate,
                    f.Rtt
                }).Join(_context.AppUsers, f => f.UserId, u => u.Id,
                (f, u) => new
                {
                    f.DocId,
                    f.UserId,
                    f.UserName,
                    f.AccDpt,
                    f.AccDptNam,
                    f.PlantCnam,
                    f.PlantEnam,
                    f.PlantType,
                    f.EngId,
                    f.EngName,
                    f.BudgetId,
                    f.AgreeDate,
                    f.Rtt,
                    u.DptId
                });
            if (!string.IsNullOrEmpty(qry.accno))
            {
                data = data.Where(f => f.AccDpt == qry.accno);
            }
            //if (!string.IsNullOrEmpty(qry.assetno))
            //{
            //    data = data.Where(f => f.AssetNo == qry.assetno);
            //}
            if (!string.IsNullOrEmpty(qry.cname))
            {
                data = data.Where(f => f.PlantCnam.Contains(qry.cname));
            }
            if (!string.IsNullOrEmpty(qry.applydpt))
            {
                data = data.Where(f => f.AccDpt == qry.applydpt);
            }
            if (!string.IsNullOrEmpty(qry.budgetid))
            {
                data = data.Where(f => f.BudgetId == qry.budgetid);
            }
            if (!string.IsNullOrEmpty(qry.agreedate1))
            {
                data = data.Where(f => f.AgreeDate != null)
                    .Where(f => f.AgreeDate.Value.CompareTo(DateTime.ParseExact(qry.agreedate1, "yyyy/MM/dd", null)) >= 0);
            }
            if (!string.IsNullOrEmpty(qry.agreedate2))
            {
                data = data.Where(f => f.AgreeDate != null)
                .Where(f => f.AgreeDate.Value.Date.CompareTo(DateTime.ParseExact(qry.agreedate2, "yyyy/MM/dd", null)) <= 0);
            }
            data = data.Take((qry.pagecnt + 1) * 100);
            string status = "";
            if (qry.flowtyp == "已結案")
                status = "2";
            else
                status = "?";
            var s = new[] { "?", "2" };
            data.Join(_context.BuyFlows, m => m.DocId, f => f.DocId,
                (m, f) => new
                {
                    buyevaluate = m,
                    flow = f
                }).Where(bf => bf.flow.Status == status)
                .ToList()
                .ForEach(j => rv.Add(new BuyEvaluateListVModel
                {
                    DocType = "評估",
                    DocId = j.buyevaluate.DocId,
                    BudgetId = j.buyevaluate.BudgetId,
                    UserId = j.buyevaluate.UserId,
                    UserName = j.buyevaluate.UserName,
                    CustId = j.buyevaluate.AccDpt,
                    CustNam = (d = _context.Departments.Find(j.buyevaluate.AccDpt)) == null ? ""
                    : d.Name_C,
                    PlantCnam = j.buyevaluate.PlantCnam,
                    PlantEnam = j.buyevaluate.PlantEnam,
                    PlantType = j.buyevaluate.PlantType,
                    EngName = (p = _context.AppUsers.Find(j.buyevaluate.EngId)) == null ? ""
                    : p.FullName,
                    Days = DateTime.Now.Subtract(j.buyevaluate.Rtt.GetValueOrDefault()).Days,

                    Flg = _context.Deliveries.Join(_context.DelivFlows, de => de.DocId, f => f.DocId,
                    (de, f) =>
                        new
                        {
                            delivery = de,
                            delivflow = f
                        }).Where(k => k.delivery.PurchaseNo == j.buyevaluate.DocId)
                    .Where(k => s.Contains(k.delivflow.Status)).Count() > 0 ?
                    "E" : j.flow.Status,
                    FlowUid = j.flow.UserId,
                    FlowUname = (p = _context.AppUsers.Find(j.flow.UserId)) == null ? "" : p.FullName,
                    IsAgree = j.buyevaluate.AgreeDate.HasValue ? true : false,
                    AgreeDate = j.buyevaluate.AgreeDate == null ? "" : j.buyevaluate.AgreeDate.Value.ToString("yyyy/MM/dd")
                }));
            //
            //_context.Dispose();
            return rv;
        }

        // GET: BMED/BuyEvaluate/GetBuyEvaluateCounts
        public JsonResult GetBuyEvaluateCounts()
        {
            /* Get user details. */
            var ur = _userRepo.Find(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var buyCount = _context.BuyFlows.Where(f => f.Status == "?")
                                            .Where(f => f.UserId == ur.Id).Count();
            return Json(buyCount);
        }


    }
}