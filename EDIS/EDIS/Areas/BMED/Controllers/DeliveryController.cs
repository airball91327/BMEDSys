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
using Newtonsoft.Json;

namespace EDIS.Areas.BMED.Controllers
{
    public class objuser
    {
        public int uid;
        public string uname;
        public string gid;
    }

    [Area("BMED")]
    [Authorize]
    public class DeliveryController : Controller
    {
        private readonly BMEDDbContext _context;
        private readonly IRepository<AppUserModel, int> _userRepo;
        private readonly IRepository<DepartmentModel, string> _dptRepo;
        private readonly IEmailSender _emailSender;
        private readonly CustomUserManager userManager;
        private readonly CustomRoleManager roleManager;

        public DeliveryController(BMEDDbContext context,
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

        public IActionResult Index()
        {
            return View(_context.Deliveries.ToList());
        }

        [HttpPost]
        public IActionResult Index(IFormCollection form)
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            listItem.Add(new SelectListItem { Text = "待處理", Value = "待處理" });
            listItem.Add(new SelectListItem { Text = "已處理", Value = "已處理" });
            listItem.Add(new SelectListItem { Text = "已結案", Value = "已結案" });
            ViewData["FLOWTYP"] = new SelectList(listItem, "Value", "Text", "待處理");
            //
            List<DeliveryListVModel> vm;
            vm = GetList(form["qtyFLOWTYP"]);
            if (!string.IsNullOrEmpty(form["qtyDOCID"]))
            {
                vm = vm.Where(m => m.DocId == form["qtyDOCID"]).ToList();
            }
            if (!string.IsNullOrEmpty(form["qtyPURCHASENO"]))
            {
                vm = vm.Where(m => m.PurchaseNo == form["qtyPURCHASENO"]).ToList();
            }
            if (!string.IsNullOrEmpty(form["qtyDPTID"]))
            {
                vm = vm.Where(m => m.Company == form["qtyDPTID"]).ToList();
            }
            if (!string.IsNullOrEmpty(form["qtyACCDPT"]))
            {
                vm = vm.Where(m => m.AccDpt == form["qtyACCDPT"]).ToList();
            }
            if (!string.IsNullOrEmpty(form["qtyBUDGETID"]))
            {
                vm = vm.Where(m => m.BudgetId == form["qtyBUDGETID"]).ToList();
            }
            if (!string.IsNullOrEmpty(form["qtyCONTRACTNO"]))
            {
                vm = vm.Where(m => m.ContractNo == form["qtyCONTRACTNO"]).ToList();
            }
            if (!string.IsNullOrEmpty(form["qtyASSETNO"]))
            {
                AssetModel at = _context.BMEDAssets.Find(form["qtyASSETNO"]);
                if (at != null)
                {
                    vm = vm.Where(m => m.DocId == at.Docid).ToList();
                }
                else
                    vm.Clear();
            }
            foreach (var item in vm)
            {
                var u = _context.AppUsers.Find(item.UserId);
                item.Contact = u == null ? "" : u.Ext;
                u = _context.AppUsers.Find(item.FlowUid);
                item.FlowUname = u == null ? "" : u.FullName;
                item.CompanyNam = _context.Departments.Find(item.Company) == null ? "" : _context.Departments.Find(item.Company).Name_C;
            }
            return PartialView("_DeliveryList", vm);
        }

        public IActionResult Index2()
        {
            //
            List<SelectListItem> listItem = new List<SelectListItem>();
            listItem.Add(new SelectListItem { Text = "已處理", Value = "已處理" });
            listItem.Add(new SelectListItem { Text = "已結案", Value = "已結案" });
            ViewData["FLOWTYP"] = new SelectList(listItem, "Value", "Text", "已處理");
            //
            List<SelectListItem> listItem2 = new List<SelectListItem>();
            SelectListItem li;
            _context.Departments.ToList()
                    .ForEach(d =>
                    {
                        li = new SelectListItem();
                        li.Text = d.Name_C;
                        li.Value = d.DptId;
                        listItem2.Add(li);

                    });
            ViewData["ACCDPT"] = new SelectList(listItem2, "Value", "Text");
            ViewData["APPLYDPT"] = new SelectList(listItem2, "Value", "Text");

            return View();
        }

        [HttpPost]
        public IActionResult Index2(FormCollection form)
        {
            List<DeliveryListVModel> vm;
            vm = GetList2(form["qtyFLOWTYP"]);
            if (form["qtyDOCID"] != "")
            {
                vm = vm.Where(m => m.DocId == form["qtyDOCID"]).ToList();
            }
            if (form["qtyPURCHASENO"] != "")
            {
                vm = vm.Where(m => m.PurchaseNo == form["qtyPURCHASENO"]).ToList();
            }
            if (form["qtyDPTID"] != "")
            {
                vm = vm.Where(m => m.Company == form["qtyDPTID"]).ToList();
            }
            if (form["qtyBUDGETID"] != "")
            {
                vm = vm.Where(m => m.BudgetId == form["qtyBUDGETID"]).ToList();
            }
            if (form["qtyCONTRACTNO"] != "")
            {
                vm = vm.Where(m => m.ContractNo == form["qtyCONTRACTNO"]).ToList();
            }
            if (form["qtyACCDPT"] != "")
            {
                vm = vm.Where(m => m.AccDpt == form["qtyACCDPT"]).ToList();
            }
            if (form["qtyASSETNO"] != "")
            {
                AssetModel at = _context.BMEDAssets.Find(form["qtyASSETNO"]);
                if (at != null)
                {
                    vm = vm.Where(m => m.DocId == at.Docid).ToList();
                }
                else
                    vm.Clear();
            }
            if (form["qtyASSETNAME"] != "")
            {
                string s = form["qtyASSETNAME"];
                List<AssetModel> at = _context.BMEDAssets.Where(a => a.Cname.Contains(s))
                    .ToList();
                if (at != null)
                {
                    vm = vm.Join(at, v => v.DocId, a => a.Docid,
                        (v, a) => v).ToList();
                }
                else
                    vm.Clear();
            }
            if (form["qtyVENDOR"] != "")
            {
                string s = form["qtyVENDOR"];
                List<VendorModel> vs = _context.BMEDVendors.Where(a => a.VendorName.Contains(s))
                    .ToList();
                if (vs != null)
                {
                    vm = vm.Join(vs, v => v.VendorNo, a => a.UniteNo,
                        (v, a) => v).ToList();
                }
                else
                    vm.Clear();
            }
            return PartialView("_DeliveryList", vm);
        }

        public IActionResult DeliveryListIndex()
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            listItem.Add(new SelectListItem { Text = "待處理", Value = "待處理" });
            listItem.Add(new SelectListItem { Text = "已處理", Value = "已處理" });
            listItem.Add(new SelectListItem { Text = "已結案", Value = "已結案" });
            ViewData["Item"] = new SelectList(listItem, "Value", "Text", "待處理");
            return PartialView("_DeliveryListIndex");
        }
        [HttpPost]
        public IActionResult DeliveryListIndex(FormCollection form)
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            listItem.Add(new SelectListItem { Text = "待處理", Value = "待處理" });
            listItem.Add(new SelectListItem { Text = "已處理", Value = "已處理" });
            listItem.Add(new SelectListItem { Text = "已結案", Value = "已結案" });
            ViewData["Item"] = new SelectList(listItem, "Value", "Text", form["qtyFLOWTYP"]);
            //
            List<DeliveryListVModel> vm;
            vm = GetList(form["qtyFLOWTYP"]);
            if (form["qtyDOCID"] != "")
            {
                vm = vm.Where(m => m.DocId == form["qtyDOCID"]).ToList();
            }
            if (form["qtyPURCHASENO"] != "")
            {
                vm = vm.Where(m => m.PurchaseNo == form["qtyPURCHASENO"]).ToList();
            }
            if (form["qtyCUSTID"] != "")
            {
                vm = vm.Where(m => m.Company == form["qtyCUSTID"]).ToList();
            }
            if (form["qtyBUDGETID"] != "")
            {
                vm = vm.Where(m => m.BudgetId == form["qtyBUDGETID"]).ToList();
            }
            if (form["qtyCONTRACTNO"] != "")
            {
                vm = vm.Where(m => m.ContractNo == form["qtyCONTRACTNO"]).ToList();
            }
            if (form["qtyASSETNO"] != "")
            {
                AssetModel at = _context.BMEDAssets.Find(form["qtyASSETNO"]);
                if (at != null)
                {
                    vm = vm.Where(m => m.DocId == at.Docid).ToList();
                }
                else
                    vm.Clear();
            }
            return PartialView("_DeliveryList", vm);
        }
        [HttpPost]
        public JsonResult EditData(DelivDataVModel dv)
        {
            if (ModelState.IsValid)
            {
                DeliveryModel d = _context.Deliveries.Find(dv.DocId);
                if (d != null)
                {
                    _context.Entry(d).State = EntityState.Modified;
                    d.WartyNt = dv.WartyNt;
                    d.AcceptDate = dv.AcceptDate;
                    d.WartySt = dv.WartySt;
                    d.WartyEd = dv.WartyEd;
                    d.FileTestDate = dv.FileTestDate;
                    d.TestUid = dv.TestUid;
                    d.Code = dv.Code;
                    d.Stype2 = dv.Stype2;
                    d.OpenDate = dv.OpenDate;
                    d.OrderDate = dv.OrderDate;
                    _context.SaveChanges();
                }
                return Json(new { success = true, msg = "儲存成功!" });
            }
            return Json(new { success = false, msg = "儲存失敗!" });
        }
        public IActionResult WartyData(string id = null)
        {
            DelivDataVModel dv = new DelivDataVModel();
            if (id != null)
            {
                DeliveryModel d = _context.Deliveries.Find(id);
                if (d != null)
                {
                    dv.DocId = d.DocId;
                    dv.WartyNt = d.WartyNt;
                    dv.AcceptDate = d.AcceptDate;
                    if (d.WartySt == null)
                        dv.WartySt = DateTime.Now;
                    else
                        dv.WartySt = d.WartySt;
                    if (d.WartyEd == null)
                    {
                        if (d.WartyMon != null)
                            dv.WartyEd = dv.WartySt.Value.AddMonths(d.WartyMon);
                        else
                            dv.WartyEd = DateTime.Now;
                    }
                    else
                        dv.WartyEd = d.WartyEd;
                    if (d.FileTestDate == null)
                        dv.FileTestDate = DateTime.Now;
                    else
                        dv.FileTestDate = d.FileTestDate;
                    dv.Stype2 = d.Stype2;
                    dv.Code = d.Code;
                    dv.TestUid = d.TestUid;
                    dv.OpenDate = d.OpenDate;
                    dv.OrderDate = d.OrderDate;
                    List<AssetModel> av = _context.BMEDAssets.Where(a => a.Docid == d.DocId).ToList();
                    List<AssetKeepModel> kv = new List<AssetKeepModel>();
                    AssetKeepModel ak;
                    foreach (AssetModel a in av)
                    {
                        ak = _context.BMEDAssetKeeps.Find(a.AssetNo);
                        if (ak != null)
                        {
                            ak.Cname = a.Cname;
                            kv.Add(ak);
                        }
                    }
                    dv.ak = kv;
                }
                List<SelectListItem> listItem = new List<SelectListItem>();
                listItem.Add(new SelectListItem { Text = "自行", Value = "自行" });
                listItem.Add(new SelectListItem { Text = "委外", Value = "委外" });
                listItem.Add(new SelectListItem { Text = "保固", Value = "保固" });
                listItem.Add(new SelectListItem { Text = "租賃", Value = "租賃" });
                ViewData["FMINOUT"] = new SelectList(listItem, "Value", "Text");
                //
                List<SelectListItem> code = new List<SelectListItem>();
                foreach (var item in _context.DelivCodes)
                {
                    code.Add(new SelectListItem { Text = item.DSC, Value = item.Code.ToString() });
                }
                ViewData["Code"] = new SelectList(code, "Value", "Text", dv.Code);
                //
                List<SelectListItem> stype2 = new List<SelectListItem>();
                stype2.Add(new SelectListItem { Text = "一般", Value = "N" });
                stype2.Add(new SelectListItem { Text = "須報備", Value = "S" });
                ViewData["Stype2"] = new SelectList(stype2, "Value", "Text", dv.Stype2);
            }
            return PartialView(dv);
        }

        public IActionResult DeliveryList()
        {
            return PartialView("_DeliveryList", GetList("待處理"));
        }

        //
        // GET: /Delivery/Details/5
        public IActionResult Details(string id = null)
        {
            DeliveryModel delivery = _context.Deliveries.Find(id);
            if (delivery == null)
            {
                return NotFound();
            }
            DepartmentModel c = _context.Departments.Find(delivery.AccDpt);
            if (c != null)
                delivery.AccDptNam = c.Name_C;
            VendorModel v = _context.BMEDVendors.Where(vv => vv.UniteNo == delivery.VendorId).FirstOrDefault();
            if (v != null)
                delivery.VendorNam = v.VendorName;
            AppUserModel u = _context.AppUsers.Find(Convert.ToInt32(delivery.DelivPson));
            if (u != null)
                delivery.DelivPsonNam = u.FullName;
            return View(delivery);
        }

        //
        // GET: /Delivery/Create
        public IActionResult Create(string id = null)
        {
            DeliveryModel r = new DeliveryModel();
            // Get Login User's details.
            var u = _userRepo.Find(ur => ur.UserName == User.Identity.Name).FirstOrDefault();
            DepartmentModel c = _context.Departments.Find(u.DptId);
            VendorModel v = _context.BMEDVendors.Find(u.VendorId);
            if (id != null)
            {
                //BuyEvaluate b = _context.BuyEvaluates.Find(id);
                //if (b != null)
                //{
                //    r.EngId = b.EngId;
                //    r.BudgetId = b.BudgetId;
                //    int a = 0;
                //    if (int.TryParse(b.AccDpt, out a))
                //    {
                //        c = _context.Departments.Find(b.AccDpt);
                //        if (c != null)
                //        {
                //            r.AccDpt = c.DptId;
                //            r.AccDptNam = c.Name_C;
                //        }
                //    }
                //    else
                //    {
                //        c = _context.Departments.Where(d => d.Name_C == b.AccDpt).FirstOrDefault();
                //        if (c != null)
                //        {
                //            r.AccDpt = c.DptId;
                //            r.AccDptNam = c.Name_C;
                //        }
                //    }
                //}
            }
            r.DocId = GetID();
            r.UserId = u.Id;
            r.UserName = u.FullName;
            c = _context.Departments.Find(u.DptId);
            if (c != null)
            {
                r.Company = c.DptId == null ? "" : c.Name_C;
                if (r.AccDpt == null)
                {
                    r.AccDpt = c.DptId == null ? "" : c.DptId;
                    r.AccDptNam = c.DptId == null ? "" : c.Name_C;
                }
            }
            r.Contact = u.Mobile;
            r.ApplyDate = DateTime.Now;
            r.PurchaseNo = id;
            r.WartyMon = 0;
            r.DelivDateR = DateTime.Now;
            _context.Deliveries.Add(r);
            _context.SaveChanges();
            List<SelectListItem> listItem = new List<SelectListItem>();
            List<SelectListItem> listItem2 = new List<SelectListItem>();
            List<SelectListItem> listItem3 = new List<SelectListItem>();
            List<SelectListItem> listItem4 = new List<SelectListItem>();
            List<SelectListItem> listItem5 = new List<SelectListItem>();
            var eng = roleManager.GetUsersInRole("MedEngineer").ToList();
            var buyer = roleManager.GetUsersInRole("Buyer");
            AppUserModel p;
            foreach (string s in eng)
            {
                p = _context.AppUsers.Where(ur => ur.UserName == s).FirstOrDefault();
                if (p.Status == "Y")
                    listItem.Add(new SelectListItem { Text = "(" + p.UserName + ")" + p.FullName, Value = p.Id.ToString() });
            }
            ViewData["ENG"] = new SelectList(listItem, "Value", "Text");
            //
            foreach (string s2 in buyer)
            {
                p = _context.AppUsers.Where(ur => ur.UserName == s2).FirstOrDefault();
                listItem2.Add(new SelectListItem { Text = p.FullName, Value = p.Id.ToString() });
            }
            ViewData["PUR"] = new SelectList(listItem2, "Value", "Text");
            //
            //List<BuyVendor> bv = _context.BuyVendors.Where(t => t.Docid == id).ToList();
            //foreach (BuyVendor b in bv)
            //{
            //    listItem3.Add(new SelectListItem { Text = b.VendorNam, Value = b.UniteNo });
            //}
            List<VendorModel> bv = _context.BMEDVendors.ToList();
            foreach (VendorModel b in bv)
            {
                listItem3.Add(new SelectListItem { Text = b.VendorName, Value = b.UniteNo });
            }
            ViewData["Vendors"] = new SelectList(listItem3, "Value", "Text");
            //
            //List<objuser> uv = _context.AppUsers.Join(_context.Departments, up => up.DptId, co => co.DptId,
            //    (up, co) => new objuser
            //    {
            //        uid = up.UserId,
            //        uname = up.FullName,
            //        gid = co.GroupId
            //    }).Where(co => co.gid == c.GroupId).ToList();

            List<AppUserModel> uv = _context.AppUsers.ToList();
            uv = uv.OrderBy(urs => urs.UserName).ToList();
            foreach (AppUserModel z in uv)
            {
                listItem4.Add(new SelectListItem { Text = "(" + z.UserName + ")" + z.FullName, Value = z.Id.ToString() });
            }
            ViewData["Users"] = new SelectList(listItem4, "Value", "Text");
            ViewData["Item2"] = new SelectList(listItem5, "Value", "Text");
            return View(r);
        }

        //
        // POST: /Delivery/Create
        [HttpPost]
        public IActionResult Create(DeliveryModel delivery)
        {
            if (ModelState.IsValid)
            {
                // Get Login User's details.
                var ur = _userRepo.Find(usr => usr.UserName == User.Identity.Name).FirstOrDefault();

                _context.Entry(delivery).State = EntityState.Modified;
                DelivFlowModel rf = new DelivFlowModel();
                rf.DocId = delivery.DocId;
                rf.StepId = 1;
                rf.UserId = ur.Id;
                rf.Status = "1";
                rf.Role = roleManager.GetRolesForUser(ur.Id).FirstOrDefault();
                rf.Rtp = ur.Id;
                rf.Rdt = null;
                rf.Rtt = DateTime.Now;
                rf.Cls = "申請者";
                _context.DelivFlows.Add(rf);
                //
                rf = new DelivFlowModel();
                rf.DocId = delivery.DocId;
                rf.StepId = 2;
                rf.Status = "?";
                AppUserModel u;

                u = _context.AppUsers.Find(Convert.ToInt32(delivery.DelivPson));
                rf.UserId = u.Id;
                rf.Role = roleManager.GetRolesForUser(u.Id).FirstOrDefault();
                rf.Rtp = null;
                rf.Rdt = null;
                rf.Rtt = DateTime.Now;
                rf.Cls = "得標廠商";
                _context.DelivFlows.Add(rf);
                //
                List<AssetModel> ar = _context.BMEDAssets.Where(a => !string.IsNullOrEmpty(a.Docid))
                                                         .Where(a => a.Docid == delivery.PurchaseNo).ToList();
                VendorModel v;
                u = _context.AppUsers.Find(Convert.ToInt32(delivery.UserDpt));
                foreach (AssetModel a in ar)
                {
                    v = _context.BMEDVendors.Where(vv => vv.UniteNo == delivery.VendorId).FirstOrDefault();
                    if (v != null)
                        a.VendorId = v.VendorId;
                    a.DelivUid = u.Id;
                    a.DelivDpt = u.DptId;
                    _context.Entry(a).State = EntityState.Modified;
                }
                //
                _context.SaveChanges();
                //----------------------------------------------------------------------------------
                // Mail
                //----------------------------------------------------------------------------------
                Tmail mail = new Tmail();
                string body = "";
                mail.from = new System.Net.Mail.MailAddress(ur.Email);
                u = _context.AppUsers.Find(Convert.ToInt32(delivery.DelivPson));
                mail.to = new System.Net.Mail.MailAddress(u.Email);
                AppUserModel up = _context.AppUsers.Find(delivery.EngId);
                if (up != null)
                {
                    if (!string.IsNullOrEmpty(up.Email))
                        mail.cc = new System.Net.Mail.MailAddress(up.Email);
                }
                //
                mail.message.Subject = "醫療儀器管理資訊系統[驗收案]：採購案號： " + delivery.PurchaseNo;
                body += "<p>申請人：" + delivery.UserName + "</p>";
                body += "<p>合約號碼：" + delivery.ContractNo + "</p>";
                body += "<p>採購案號：" + delivery.PurchaseNo + "</p>";
                body += "<p><a href='http://dms.cch.org.tw/MvcMedEngMgr'>處理案件</a></p>";
                body += "<br/>";
                body += "<p>若有任何問題，請與驗收工程師(" + _context.AppUsers.Find(delivery.EngId).FullName + ")聯絡</p>";
                body += "<h3>this is a inform letter from system manager.Do not reply for it.</h3>";
                mail.message.Body = body;
                mail.message.IsBodyHtml = true;
                //mail.SendMail();
                //----------------------------------------------------------------------------------

                return RedirectToAction("Index", "Home", new { Area = "" });
            }

            return View(delivery);
        }

        //
        // GET: /Delivery/Edit/5
        public IActionResult Edit(string id = null)
        {
            DeliveryModel delivery = _context.Deliveries.Find(id);
            if (delivery == null)
            {
                return NotFound();
            }
            DepartmentModel c = _context.Departments.Find(delivery.AccDpt);
            if (c != null)
                delivery.AccDptNam = c.Name_C;
            VendorModel v = _context.BMEDVendors.Where(vv => vv.UniteNo == delivery.VendorId).FirstOrDefault();
            if (v != null)
                delivery.VendorNam = v.VendorName;
            AppUserModel u = _context.AppUsers.Find(Convert.ToInt32(delivery.DelivPson));
            if (u != null)
                delivery.DelivPsonNam = u.FullName;
            //BuyEvaluate b = db.BuyEvaluates.Find(delivery.PurchaseNo);
            //if (b != null)
            //    delivery.BudgetId = b.BudgetId;
            return View(delivery);
        }

        //
        // POST: /Delivery/Edit/5
        [HttpPost]
        public IActionResult Edit(DeliveryModel delivery)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(delivery).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(delivery);
        }

        //
        // GET: /Delivery/Delete/5
        public IActionResult Delete(string id = null)
        {
            DeliveryModel delivery = _context.Deliveries.Find(id);
            if (delivery == null)
            {
                return NotFound();
            }
            return View(delivery);
        }

        //
        // POST: /Delivery/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(string id)
        {
            DeliveryModel delivery = _context.Deliveries.Find(id);
            _context.Deliveries.Remove(delivery);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public List<DeliveryListVModel> GetList(string cls = null)
        {
            List<DeliveryListVModel> dv = new List<DeliveryListVModel>();
            List<DelivFlowModel> rf = new List<DelivFlowModel>();
            List<DelivFlowModel> rf2;
            // Get Login User's details.
            var ur = _userRepo.Find(usr => usr.UserName == User.Identity.Name).FirstOrDefault();
            switch (cls)
            {
                case "已處理":
                    rf2 = _context.DelivFlows.FromSql("SELECT * FROM BMEDDELIVFLOWS WHERE STATUS ='?'")
                                             .Where(m => m.UserId != ur.Id).ToList();
                    if (userManager.IsInRole(User, "MedToDo"))
                    {
                        rf.AddRange(rf2);
                    }
                    else
                    {
                        foreach (DelivFlowModel f in rf2)
                        {
                            if (_context.DelivFlows.Where(m => m.DocId == f.DocId).Where(m => m.UserId == ur.Id).Count() > 0)
                            {
                                rf.Add(f);
                            }
                        }
                    }

                    break;
                case "已結案":
                    rf2 = _context.DelivFlows.FromSql("SELECT * FROM BMEDDELIVFLOWS WHERE STATUS ='2'").ToList();
                    if (userManager.IsInRole(User, "MedToDo"))
                    {
                        rf.AddRange(rf2);
                    }
                    else
                    {
                        foreach (DelivFlowModel f in rf2)
                        {
                            if (_context.DelivFlows.Where(m => m.DocId == f.DocId).Where(m => m.UserId == ur.Id).Count() > 0)
                            {
                                rf.Add(f);
                            }
                        }
                    }
                    break;
                case "查詢":
                    rf2 = _context.DelivFlows.FromSql("SELECT * FROM BMEDDELIVFLOWS WHERE STATUS ='?'").ToList();
                    DeliveryModel r;
                    foreach (DelivFlowModel f in rf2)
                    {
                        r = _context.Deliveries.Find(f.DocId);
                        rf.Add(f);
                    }
                    break;
                default:
                    rf = _context.DelivFlows.FromSql("SELECT * FROM BMEDDELIVFLOWS WHERE STATUS ='?'")
                        .Where(m => m.UserId == ur.Id).ToList();
                    break;
            }
            rf.OrderByDescending(m => m.Rtt);
            DeliveryListVModel i;
            foreach (DelivFlowModel f in rf)
            {
                DeliveryModel r = _context.Deliveries.Find(f.DocId);
                AppUserModel p = _context.AppUsers.Find(r.UserId);
                DepartmentModel c = _context.Departments.Find(p.DptId);
                //BuyEvaluate b = db.BuyEvaluates.Find(r.PurchaseNo);
                i = new DeliveryListVModel();
                i.DocType = "驗收";
                i.DocId = r.DocId;
                i.UserId = r.UserId;
                i.UserName = r.UserName;
                if (p != null && p.DptId != null)
                {
                    i.Company = p.DptId;
                    i.CompanyNam = c == null ? "" : c.Name_C;
                }
                i.ContractNo = r.ContractNo;
                i.PurchaseNo = r.PurchaseNo;
                i.CrlItemNo = r.CrlItemNo;
                i.AccDpt = r.AccDpt;
                i.AccDptNam = _context.Departments.Find(r.AccDpt) == null ? "" : _context.Departments.Find(r.AccDpt).Name_C;
                i.BudgetId = "";
                if (f.Status == "?")
                    i.Days = DateTime.Now.Subtract(r.ApplyDate.GetValueOrDefault()).Days;
                else
                    i.Days = null;
                i.Flg = f.Status;
                i.FlowUid = f.UserId;
                dv.Add(i);
            }
            //
            return dv;
        }

        public List<DeliveryListVModel> GetList2(string cls = null)
        {
            List<DeliveryListVModel> dv = new List<DeliveryListVModel>();
            List<DelivFlowModel> rf = new List<DelivFlowModel>();
            List<DelivFlowModel> rf2;
            // Get Login User's details.
            var ur = _userRepo.Find(usr => usr.UserName == User.Identity.Name).FirstOrDefault();
            switch (cls)
            {
                case "已處理":
                    rf2 = _context.DelivFlows.FromSql("SELECT * FROM BMEDDELIVFLOWS WHERE STATUS ='?'")
                        .Where(m => m.UserId != ur.Id).ToList();
                    if (!userManager.IsInRole(User, "Usual"))
                    {
                        rf.AddRange(rf2);
                    }
                    else
                    {
                        foreach (DelivFlowModel f in rf2)
                        {
                            if (_context.DelivFlows.Where(m => m.DocId == f.DocId).Where(m => m.UserId == ur.Id).Count() > 0)
                            {
                                rf.Add(f);
                            }
                        }
                    }

                    break;
                case "已結案":
                    rf2 = _context.DelivFlows.FromSql("SELECT * FROM BMEDDELIVFLOWS WHERE STATUS ='2'").ToList();
                    if (!userManager.IsInRole(User, "Usual"))
                    {
                        rf.AddRange(rf2);
                    }
                    else
                    {
                        foreach (DelivFlowModel f in rf2)
                        {
                            if (_context.DelivFlows.Where(m => m.DocId == f.DocId).Where(m => m.UserId == ur.Id).Count() > 0)
                            {
                                rf.Add(f);
                            }
                        }
                    }
                    break;
                case "所有":
                    rf2 = _context.DelivFlows.FromSql("SELECT * FROM BMEDDELIVFLOWS WHERE STATUS ='2' OR STATUS ='?'").ToList();
                    if (!userManager.IsInRole(User, "Usual"))
                    {
                        rf.AddRange(rf2);
                    }
                    else
                    {
                        foreach (DelivFlowModel f in rf2)
                        {
                            if (_context.DelivFlows.Where(m => m.DocId == f.DocId).Where(m => m.UserId == ur.Id).Count() > 0)
                            {
                                rf.Add(f);
                            }
                        }
                    }
                    break;
                default:
                    rf = _context.DelivFlows.FromSql("SELECT * FROM BMEDDELIVFLOWS WHERE STATUS ='?'")
                                            .Where(m => m.UserId == ur.Id).ToList();
                    break;
            }
            rf.OrderByDescending(m => m.Rtt);
            DeliveryListVModel i;
            foreach (DelivFlowModel f in rf)
            {
                DeliveryModel r = _context.Deliveries.Find(f.DocId);
                AppUserModel p = _context.AppUsers.Find(r.UserId);
                i = new DeliveryListVModel();
                i.DocType = "驗收";
                i.DocId = r.DocId;
                i.UserId = r.UserId;
                i.UserName = r.UserName;
                i.ContractNo = r.ContractNo;
                i.PurchaseNo = r.PurchaseNo;
                i.CrlItemNo = r.CrlItemNo;
                i.AccDpt = r.AccDpt;
                i.AccDptNam = _context.Departments.Find(r.AccDpt) == null ? "" : _context.Departments.Find(r.AccDpt).Name_C;
                i.BudgetId = "";
                if (f.Status == "?")
                    i.Days = DateTime.Now.Subtract(r.ApplyDate.GetValueOrDefault()).Days;
                else
                    i.Days = null;
                i.Flg = f.Status;
                i.FlowUid = f.UserId;
                i.VendorNo = r.VendorId;
                dv.Add(i);
            }
            //
            return dv;
        }

        public string GetID()
        {
            string str = "";
            str += "SELECT MAX(DOCID) DocId FROM BMEDDeliveries ";
            var r = _context.Deliveries.FromSql(str).Select(d => d.DocId).ToList();
            string did = "";
            int yymm = (System.DateTime.Now.Year - 1911) * 100 + System.DateTime.Now.Month;
            foreach (string s in r)
            {
                did = s;
            }
            if (did != "")
            {
                if (Convert.ToInt64(did) / 100000 == yymm)
                    did = Convert.ToString(Convert.ToInt64(did) + 1);
                else
                    did = Convert.ToString(yymm * 100000 + 1);
            }
            else
            {
                did = Convert.ToString(yymm * 100000 + 1);
            }
            return did;
        }

        // GET: BMED/Delivery/GetDeliveryCounts
        public JsonResult GetDeliveryCounts()
        {
            /* Get user details. */
            var ur = _userRepo.Find(u => u.UserName == User.Identity.Name).FirstOrDefault();
            var deliveryCount = _context.DelivFlows.Where(f => f.Status == "?")
                                                   .Where(f => f.UserId == ur.Id).Count();
            return Json(deliveryCount);
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}