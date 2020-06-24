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

namespace EDIS.Areas.BMED.Controllers
{
    [Area("BMED")]
    [Authorize]
    public class BudgetController : Controller
    {
        private BMEDDbContext _context;
        private readonly IRepository<AppUserModel, int> _userRepo;
        private readonly IRepository<DepartmentModel, string> _dptRepo;
        private readonly IEmailSender _emailSender;
        private readonly CustomUserManager userManager;
        private readonly CustomRoleManager roleManager;

        public BudgetController(BMEDDbContext context,
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
        // GET: /Budget/
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Index2()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index2(FormCollection form)
        {
            List<BudgetModel> vm = _context.Budgets.ToList();
            if (form["qtyDOCID"] != "")
            {
                vm = vm.Where(v => v.DocId == form["qtyDOCID"]).ToList();
            }
            if (form["qtyACCDPT"] != "")
            {
                vm = vm.Where(v => v.AccDpt.Contains(form["qtyACCDPT"])).ToList();
            }
            return PartialView("List2", vm);
        }

        [HttpPost]
        public IActionResult Index(IFormCollection form)
        {
            List<BudgetModel> vm;
            var s = this.Request.Form["qtyTRANS"][0];
            if (s == "true")
            {
                string[] v = { "?", "2" };
                var ds = _context.BuyFlows.Where(f => v.Contains(f.Status))
                    .Join(_context.BuyEvaluates, f => f.DocId, b => b.DocId,
                    (f, b) => b).Select(b => b.BudgetId).Distinct();
                vm = _context.Budgets.Where(b => !ds.Contains(b.DocId)).ToList();
            }
            else
                vm = _context.Budgets.ToList();
            if (form["qtyDOCID"] != "")
            {
                vm = vm.Where(v => v.DocId == form["qtyDOCID"]).ToList();
            }
            if (form["qtyACCDPT"] != "")
            {
                vm = vm.Where(v => v.AccDpt.Contains(form["qtyACCDPT"])).ToList();
            }
            return PartialView("List", vm);
        }

        public IActionResult List()
        {
            List<BudgetModel> vm = _context.Budgets.ToList();
            string[] v = { "?", "2" };
            var ds = _context.BuyFlows.Where(f => v.Contains(f.Status))
                .Join(_context.BuyEvaluates, f => f.DocId, b => b.DocId,
                (f, b) => b).Select(b => b.BudgetId).Distinct();
            vm = _context.Budgets.Where(b => !ds.Contains(b.DocId)).ToList();
            return PartialView(vm);
        }

        public IActionResult List2()
        {
            return PartialView(_context.Budgets.ToList());
        }

        //
        // GET: /Budget/Details/5
        public IActionResult Details(string id = null)
        {
            BudgetModel budget = _context.Budgets.Find(id);
            if (budget == null)
            {
                return NotFound();
            }
            return View(budget);
        }

        //
        // GET: /Budget/Create
        public IActionResult Create()
        {
            return View();
        }

        //
        // POST: /Budget/Create
        [HttpPost]
        public IActionResult Create(BudgetModel budget)
        {
            if (ModelState.IsValid)
            {
                _context.Budgets.Add(budget);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(budget);
        }

        //
        // GET: /Budget/Edit/5
        public IActionResult Edit(string id = null)
        {
            BudgetModel budget = _context.Budgets.Find(id);
            if (budget == null)
            {
                return NotFound();
            }
            return View(budget);
        }

        //
        // POST: /Budget/Edit/5
        [HttpPost]
        public IActionResult Edit(BudgetModel budget)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(budget).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(budget);
        }

        //
        // GET: /Budget/Delete/5
        public IActionResult Delete(string id = null)
        {
            BudgetModel budget = _context.Budgets.Find(id);
            if (budget == null)
            {
                return NotFound();
            }
            return View(budget);
        }

        //
        // POST: /Budget/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(string id)
        {
            BudgetModel budget = _context.Budgets.Find(id);
            _context.Budgets.Remove(budget);
            _context.SaveChanges();
            return Redirect(Url.Content("~/Budget/Index"));
        }

        public IActionResult TransToBuyEvaluate()
        {            
            // Get Login User's details.
            var user = _userRepo.Find(ur => ur.UserName == User.Identity.Name).FirstOrDefault();

            String[] st = { "?", "2" };
            var s = _context.BuyFlows.Where(b => st.Contains(b.Status))
                .Join(_context.BuyEvaluates, b => b.DocId, e => e.DocId,
                (b, e) => e)
                .Select(e => e.BudgetId).Distinct();
            List<BudgetModel> bs = _context.Budgets.Where(b => !s.Contains(b.DocId)).ToList();
            BuyEvaluateModel r;
            BuyFlowModel rf;
            Tmail mail;
            string body = "";
            AppUserModel a = _context.AppUsers.Find(user.Id);
            AppUserModel u;
            DepartmentModel c = _context.Departments.Find(a.DptId);
            int dc = 0;
            foreach (BudgetModel b in bs)
            {
                r = new BuyEvaluateModel();
                if (dc <= 0)
                    dc = Convert.ToInt32(r.GetID(ref _context));
                else
                    dc++;
                r.DocId = Convert.ToString(dc);
                r.PlantCnam = b.PlantName;
                r.PlantEnam = b.PlantName;
                r.Standard = b.Standard;
                r.Price = b.Price;
                r.Amt = b.Amt;
                r.Unit = b.Unit;
                r.TotalPrice = b.TotalPrice;
                r.UserId = a.Id;
                r.UserName = a.FullName;
                r.Company = c.DptId == null ? "" : c.Name_C;
                r.AccDpt = b.AccDpt == null ? "" : b.AccDpt;
                r.AccDptNam = _context.Departments.Find(b.AccDpt) == null ? "" : _context.Departments.Find(b.AccDpt).Name_C;
                r.Contact = a.Mobile;
                r.PlantType = "醫療儀器";
                r.Place = r.AccDptNam;
                if (_context.AppUsers.Where(up => up.FullName == b.EngName && up.Status == "Y").FirstOrDefault() == null)
                {
                    throw new Exception("無工程師[" + b.EngName + "]資料!!請建立使用者資料。");
                }
                r.EngId = _context.AppUsers.Where(up => up.FullName == b.EngName && up.Status == "Y").FirstOrDefault() == null ? user.Id :
                    _context.AppUsers.Where(up => up.FullName == b.EngName && up.Status == "Y").FirstOrDefault().Id;
                r.Rtp = a.Id;
                r.Rtt = DateTime.Now;
                r.BudgetId = b.DocId;
                _context.BuyEvaluates.Add(r);
                //
                rf = new BuyFlowModel();
                rf.DocId = r.DocId;
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
                rf.DocId = r.DocId;
                rf.StepId = 2;
                rf.UserId = r.EngId;
                rf.Status = "?";
                u = _context.AppUsers.Find(r.EngId);
                rf.Role = roleManager.GetRolesForUser(u.Id).FirstOrDefault();
                rf.Rtp = null;
                rf.Rdt = null;
                rf.Rtt = DateTime.Now;
                rf.Cls = "設備工程師";
                _context.BuyFlows.Add(rf);
                //
                try
                {
                    _context.SaveChanges();
                    //
                    mail = new Tmail();
                    body = "";
                    u = _context.AppUsers.Find(user.Id);
                    mail.from = new System.Net.Mail.MailAddress(u.Email); //u.Email
                    u = _context.AppUsers.Find(rf.UserId);
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
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

            }
            return new JsonResult(bs)
            {
                Value = new { success = true, error = "" },
            };

        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}