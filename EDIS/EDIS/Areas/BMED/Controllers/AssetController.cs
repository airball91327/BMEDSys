using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
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
using X.PagedList;

namespace EDIS.Areas.BMED.Controllers
{
    [Area("BMED")]
    [Authorize]
    public class AssetController : Controller
    {
        private readonly BMEDDbContext _context;
        private readonly IRepository<AppUserModel, int> _userRepo;
        private readonly CustomRoleManager roleManager;
        private readonly CustomUserManager userManager;
        private int pageSize = 100;

        public AssetController(BMEDDbContext context,
                               IRepository<AppUserModel, int> userRepo,
                               CustomRoleManager customRoleManager,
                               CustomUserManager customUserManager)
        {
            _context = context;
            _userRepo = userRepo;
            roleManager = customRoleManager;
            userManager = customUserManager;
        }

        // GET: BMED/Asset
        public IActionResult Index()
        {
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
            ViewData["DELIVDPT"] = new SelectList(listItem2, "Value", "Text");
            List<SelectListItem> listItem4 = new List<SelectListItem>();
            _context.BMEDDeviceClassCodes.ToList()
                .ForEach(d =>
                {
                    listItem4.Add(new SelectListItem { Text = d.M_name, Value = d.M_code });
                });
            ViewData["BmedNo"] = new SelectList(listItem4, "Value", "Text", "");

            return View();
        }

        // POST: BMED/Asset
        [HttpPost]
        public IActionResult Index(IFormCollection fm, int page = 1)
        {
            QryAsset qryAsset = new QryAsset();
            qryAsset.AssetName = fm["AssetName"];
            qryAsset.AssetNo = fm["AssetNo"];
            qryAsset.AccDpt = fm["AccDpt"];
            qryAsset.DelivDpt = fm["DelivDpt"];
            qryAsset.Type = fm["Type"];
            qryAsset.BmedNo = fm["BmedNo"];
            List<AssetModel> at = new List<AssetModel>();
            List<AssetModel> at2 = new List<AssetModel>();
            _context.BMEDAssets.GroupJoin(_context.Departments, a => a.DelivDpt, d => d.DptId,
                (a, d) => new { Asset = a, Department = d })
                .SelectMany(p => p.Department.DefaultIfEmpty(),
                (x, y) => new { Asset = x.Asset, Department = y })
                .ToList()
                .GroupJoin(_context.AppUsers, e => e.Asset.DelivUid, u => u.Id,
                (e, u) => new { Asset = e, AppUser = u })
                .SelectMany(p => p.AppUser.DefaultIfEmpty(),
                (e, y) => new { Asset = e.Asset.Asset, Department = e.Asset.Department, AppUser = y })
                .ToList()
                .ForEach(p =>
                {
                    p.Asset.DelivDptName = p.Department == null ? "" : p.Department.Name_C;
                    p.Asset.DelivEmp = p.AppUser == null ? "" : p.AppUser.FullName;
                    at.Add(p.Asset);
                });
            at.GroupJoin(_context.Departments, a => a.AccDpt, d => d.DptId,
                (a, d) => new { Asset = a, Department = d })
                .SelectMany(p => p.Department.DefaultIfEmpty(),
                (x, y) => new { Asset = x.Asset, Department = y })
                .ToList()
                .ForEach(p =>
                {
                    p.Asset.AccDptName = p.Department == null ? "" : p.Department.Name_C;
                    at2.Add(p.Asset);
                });
            if (!string.IsNullOrEmpty(qryAsset.AssetNo))
            {
                at2 = at2.Where(a => a.AssetNo == qryAsset.AssetNo).ToList();
            }
            if (!string.IsNullOrEmpty(qryAsset.AssetName))
            {
                at2 = at2.Where(a => a.Cname.Contains(qryAsset.AssetName)).ToList();
            }
            if (!string.IsNullOrEmpty(qryAsset.AccDpt))
            {
                at2 = at2.Where(a => a.AccDpt == qryAsset.AccDpt).ToList();
            }
            if (!string.IsNullOrEmpty(qryAsset.DelivDpt))
            {
                at2 = at2.Where(a => a.DelivDpt == qryAsset.DelivDpt).ToList();
            }
            if (!string.IsNullOrEmpty(qryAsset.BmedNo))
            {
                at2 = at2.Where(a => a.BmedNo != null)
                    .Where(a => a.BmedNo.Contains(qryAsset.BmedNo)).ToList();
            }
            if (!string.IsNullOrEmpty(qryAsset.Type))
            {
                at2 = at2.Where(a => a.Type == qryAsset.Type).ToList();
            }
            // Get MedEngineers to set dropdownlist.
            List<SelectListItem> listItem = new List<SelectListItem>();
            var s = roleManager.GetUsersInRole("MedEngineer").ToList();
            foreach (string l in s)
            {
                AppUserModel u = _context.AppUsers.Where(ur => ur.UserName == l).FirstOrDefault();
                if (u != null)
                {
                    listItem.Add(new SelectListItem { Text = u.FullName, Value = u.Id.ToString() });
                }
            }
            ViewData["KeepEngId"] = new SelectList(listItem, "Value", "Text", "");
            ViewData["AssetEngId"] = new SelectList(listItem, "Value", "Text", "");
            //
            if (at2.ToPagedList(page, pageSize).Count <= 0)
                return PartialView("List", at2.ToPagedList(1, pageSize));
            return PartialView("List", at2.ToPagedList(page, pageSize));
        }

        // GET: BMED/Asset/List
        public IActionResult List()
        {
            List<AssetModel> at = _context.BMEDAssets.ToList();
            DepartmentModel d;
            at.ForEach(a =>
            {
                a.DelivDptName = (d = _context.Departments.Find(a.DelivDpt)) == null ? "" : d.Name_C;
            });

            return PartialView(at);
        }

        // GET: BMED/Asset/Details/5
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            AssetModel asset = _context.BMEDAssets.Find(id);
            if (asset == null)
            {
                return StatusCode(404);
            }
            if (asset.DelivUid != null)
            {
                var userName = _context.AppUsers.Find(asset.DelivUid).UserName;
                asset.DelivEmp = "(" + userName + ") " + asset.DelivEmp;
            }
            asset.DelivDptName = _context.Departments.Find(asset.DelivDpt).Name_C;
            asset.AccDptName = _context.Departments.Find(asset.AccDpt).Name_C;
            asset.VendorName = asset.VendorId == null ? "" : _context.BMEDVendors.Find(asset.VendorId).VendorName;

            return View(asset);
        }

        // GET: BMED/Asset/AssetView/5
        public IActionResult AssetView(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            AssetModel asset = _context.BMEDAssets.Find(id);
            if (asset == null)
            {
                return StatusCode(404);
            }
            asset.DelivDptName = _context.Departments.Find(asset.DelivDpt).Name_C;
            asset.AccDptName = _context.Departments.Find(asset.AccDpt).Name_C;

            return PartialView(asset);
        }

        // GET: BMED/Asset/Create
        public ActionResult Create()
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            _context.Departments.ToList().ForEach(d =>
            {
                listItem.Add(new SelectListItem { Text = d.Name_C, Value = d.DptId });
            });
            ViewData["DelivDpt"] = new SelectList(listItem, "Value", "Text", "");

            List<SelectListItem> listItem2 = new List<SelectListItem>();
            listItem2.Add(new SelectListItem { Text = "", Value = "" });
            ViewData["DelivUid"] = new SelectList(listItem2, "Value", "Text", "");

            ViewData["AccDpt"] = new SelectList(listItem, "Value", "Text", "");

            List<SelectListItem> listItem3 = new List<SelectListItem>();
            listItem3.Add(new SelectListItem { Text = "正常", Value = "正常" });
            listItem3.Add(new SelectListItem { Text = "報廢", Value = "報廢" });
            ViewData["DisposeKind"] = new SelectList(listItem3, "Value", "Text", "");
            //
            List<SelectListItem> listItem4 = new List<SelectListItem>();
            _context.BMEDDeviceClassCodes.ToList()
                .ForEach(d =>
                {
                    listItem4.Add(new SelectListItem { Text = d.M_name, Value = d.M_code });
                });
            ViewData["BmedNo"] = new SelectList(listItem4, "Value", "Text", "");
            //
            // Get MedEngineers to set dropdownlist.
            var s = roleManager.GetUsersInRole("MedEngineer").ToList();
            List<SelectListItem> listItem5 = new List<SelectListItem>();
            foreach (string l in s)
            {
                AppUserModel u = _context.AppUsers.Where(ur => ur.UserName == l).FirstOrDefault();
                if (u != null)
                {
                    listItem5.Add(new SelectListItem { Text = u.FullName, Value = u.Id.ToString() });
                }
            }
            ViewData["AssetEngId"] = new SelectList(listItem5, "Value", "Text", "");

            return View();
        }

        // POST: BMED/Asset/Create
        [HttpPost]
        public ActionResult Create(AssetModel asset)
        {
            if (ModelState.IsValid)
            {
                asset.AssetNo = asset.AssetNo.Trim();
                if (_context.BMEDAssets.Find(asset.AssetNo) != null)
                {
                    throw new Exception("財產編號已經存在!!");
                }
                try
                {
                    asset.DelivEmp = asset.DelivUid == null ? "" : _context.AppUsers.Find(asset.DelivUid).FullName;
                    asset.AssetEngName = asset.AssetEngId == 0 ? "" : _context.AppUsers.Find(asset.AssetEngId).FullName;
                    _context.BMEDAssets.Add(asset);
                    AssetKeepModel ak = new AssetKeepModel();
                    ak.AssetNo = asset.AssetNo;
                    _context.BMEDAssetKeeps.Add(ak);
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                return new JsonResult(asset)
                {
                    Value = new { success = true, error = "", id = asset.AssetNo }
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
            //List<SelectListItem> listItem = new List<SelectListItem>();
            //db.Departments.ToList().ForEach(d => {
            //    listItem.Add(new SelectListItem { Text = d.Name_C, Value = d.DptId });
            //});
            //ViewData["DelivDpt"] = new SelectList(listItem, "Value", "Text", "");

            //List<SelectListItem> listItem2 = new List<SelectListItem>();
            //listItem2.Add(new SelectListItem { Text = "", Value = "" });
            //ViewData["DelivUid"] = new SelectList(listItem2, "Value", "Text", "");

            //ViewData["AccDpt"] = new SelectList(listItem, "Value", "Text", "");

            //List<SelectListItem> listItem3 = new List<SelectListItem>();
            //listItem3.Add(new SelectListItem { Text = "正常", Value = "正常" });
            //listItem3.Add(new SelectListItem { Text = "報廢", Value = "報廢" });
            //ViewData["DisposeKind"] = new SelectList(listItem3, "Value", "Text", "");

            //return View(asset);
        }

        // GET: BMED/Asset/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            AssetModel asset = _context.BMEDAssets.Find(id);
            if (asset == null)
            {
                return StatusCode(404);
            }
            //
            List<SelectListItem> listItem = new List<SelectListItem>();
            _context.Departments.ToList().ForEach(d =>
            {
                listItem.Add(new SelectListItem { Text = d.Name_C, Value = d.DptId });
            });
            ViewData["DelivDpt"] = new SelectList(listItem, "Value", "Text", "");

            List<SelectListItem> listItem2 = new List<SelectListItem>();
            _context.AppUsers.Where(u => u.DptId == asset.DelivDpt).ToList().ForEach(u =>
            {
                listItem2.Add(new SelectListItem { Text = u.FullName, Value = u.Id.ToString() });
            });
            if (listItem2.Where(item => item.Value == asset.DelivUid.ToString()).Count() == 0)
                listItem2.Add(new SelectListItem { Text = asset.DelivEmp, Value = asset.DelivUid.ToString() });
            ViewData["DelivUid"] = new SelectList(listItem2, "Value", "Text", "");

            ViewData["AccDpt"] = new SelectList(listItem, "Value", "Text", "");

            List<SelectListItem> listItem3 = new List<SelectListItem>();
            listItem3.Add(new SelectListItem { Text = "正常", Value = "正常" });
            listItem3.Add(new SelectListItem { Text = "報廢", Value = "報廢" });
            ViewData["DisposeKind"] = new SelectList(listItem3, "Value", "Text", "");
            //
            List<SelectListItem> listItem4 = new List<SelectListItem>();
            _context.BMEDDeviceClassCodes.ToList()
                .ForEach(d =>
                {
                    listItem4.Add(new SelectListItem { Text = d.M_name, Value = d.M_code });
                });
            ViewData["BmedNo"] = new SelectList(listItem4, "Value", "Text", "");
            //
            // Get MedEngineers to set dropdownlist.
            var s = roleManager.GetUsersInRole("MedEngineer").ToList();
            List<SelectListItem> listItem5 = new List<SelectListItem>();
            foreach (string l in s)
            {
                AppUserModel u = _context.AppUsers.Where(ur => ur.UserName == l).FirstOrDefault();
                if (u != null)
                {
                    listItem5.Add(new SelectListItem { Text = u.FullName, Value = u.Id.ToString() });
                }
            }
            ViewData["AssetEngId"] = new SelectList(listItem5, "Value", "Text", "");
            //
            if (asset.VendorId != null)
            {
                asset.VendorName = _context.BMEDVendors.Where(v => v.VendorId == asset.VendorId).FirstOrDefault().VendorName;
            }

            return View(asset);
        }

        // POST: BMED/Asset/Edit/5
        [HttpPost]
        public ActionResult Edit(AssetModel asset)
        {
            if (ModelState.IsValid)
            {
                asset.DelivEmp = asset.DelivUid == null ? "" : _context.AppUsers.Find(asset.DelivUid).FullName;
                asset.AssetEngName = asset.AssetEngId == 0 ? "" : _context.AppUsers.Find(asset.AssetEngId).FullName;
                _context.Entry(asset).State = EntityState.Modified;
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                return new JsonResult(asset)
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

        // GET: BMED/Asset/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            AssetModel asset = _context.BMEDAssets.Find(id);
            AssetKeepModel ak = _context.BMEDAssetKeeps.Find(id);
            _context.BMEDAssets.Remove(asset);
            _context.BMEDAssetKeeps.Remove(ak);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // POST: BMED/Asset/UpdEngineer/5
        [HttpPost]
        public ActionResult UpdEngineer(string id, string assets)
        {
            string[] s = assets.Split(new char[] { ';' });
            AssetModel asset;
            foreach (string ss in s)
            {
                asset = _context.BMEDAssets.Find(ss);
                if (asset != null)
                {
                    AppUserModel u = _context.AppUsers.Find(Convert.ToInt32(id));
                    if (u != null)
                    {
                        asset.AssetEngId = u.Id;
                        asset.AssetEngName = u.FullName;
                        _context.Entry(asset).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                }
            }
            return new JsonResult(id)
            {
                Value = new { success = true, error = "" }
            };
        }

        public ActionResult New(string docid, DateTime rdate)
        {
            // Get Login User's details.
            var ur = _userRepo.Find(usr => usr.UserName == User.Identity.Name).FirstOrDefault();

            AssetModel asset = new AssetModel();
            asset.AccDate = DateTime.Now;
            asset.BuyDate = DateTime.Now;
            asset.DisposeKind = "正常";
            if (docid != null)
            {
                DeliveryModel dy = _context.Deliveries.Find(docid);
                //BuyEvaluate d = db.BuyEvaluates.Find(dy.PurchaseNo);
                //if (d != null)
                //{
                //    asset.Cname = d.PlantCnam;
                //    asset.Ename = d.PlantEnam;
                //}
                if (dy != null)
                {
                    asset.AccDpt = dy.AccDpt;
                    VendorModel vr = _context.BMEDVendors.Where(v => v.UniteNo == dy.VendorId).FirstOrDefault();
                    asset.VendorId = vr == null ? 0 : vr.VendorId;
                }
                asset.Docid = docid;
            }
            //asset.DelivUid = 34;
            //asset.DelivEmp = "張三";
            //asset.DelivDpt = "8420";
            asset.KeepYm = (rdate.Year - 1911) * 100 + rdate.Month;
            AppUserModel u = _context.AppUsers.Find(ur.Id);
            if (u != null)
            {
                asset.DelivUid = u.Id;
                asset.DelivEmp = u.FullName;
                asset.DelivDpt = u.DptId;

                if (u.DptId != null)
                {
                    asset.AccDpt = u.DptId;
                    asset.DelivDpt = u.DptId;
                    DepartmentModel co = _context.Departments.Find(u.DptId);
                    if (co != null)
                        asset.LeaveSite = co.Name_C;
                }
            }
            //
            List<SelectListItem> listItem = new List<SelectListItem>();
            List<SelectListItem> delivdpt = new List<SelectListItem>();
            List<SelectListItem> accdpt = new List<SelectListItem>();
            listItem.Add(new SelectListItem { Text = asset.DelivEmp, Value = asset.DelivUid.Value.ToString() });
            List<AppUserModel> ul;
            string gid = "CCH";
            _context.Departments.ToList()
                .ForEach(o => {
                    delivdpt.Add(new SelectListItem
                    {
                        Text = o.Name_C,
                        Value = o.DptId
                    });
                    accdpt.Add(new SelectListItem
                    {
                        Text = o.Name_C,
                        Value = o.DptId
                    });
                });
            ViewData["DelivUids"] = new SelectList(listItem, "Value", "Text");
            ViewData["DelivDpts"] = new SelectList(delivdpt, "Value", "Text", asset.DelivDpt);
            ViewData["AccDpts"] = new SelectList(accdpt, "Value", "Text", asset.AccDpt);

            return PartialView(asset);
        }

        [HttpPost]
        public ActionResult New(AssetModel asset)
        {
            if (ModelState.IsValid)
            {
                if (asset.AssetNo.Contains(";") || asset.AssetNo.Contains("、") || asset.AssetNo.Contains(","))
                {
                    return Content("財產編號只能有一個，請重新輸入!!");
                }
                if (_context.BMEDAssets.Where(a => a.AssetNo == asset.AssetNo).Count() > 0)
                {
                    return Content("新增失敗，此財產編號已經存在!!");
                    //return View(asset);
                }
                if (string.IsNullOrEmpty(asset.LeaveSite))
                {
                    return Content("[放置地點]不可空白!!");
                }
                if (string.IsNullOrEmpty(asset.RiskLvl))
                {
                    return Content("[風險等級]不可空白!!");
                }
                if (asset.KeepYm == null || asset.Cycle == null)
                {
                    return Content("[保養起始年月]或[保養週期]不可空白!!");
                }
                if (string.IsNullOrEmpty(asset.MakeNo))
                {
                    return Content("[製造號碼]不可空白!!");
                }
                if (asset.RelDate == null)
                {
                    return Content("[製造日期]不可空白!!");
                }
                if (string.IsNullOrEmpty(asset.DelivDpt))
                {
                    return Content("[保管部門]不可空白!!");
                }
                if (string.IsNullOrEmpty(asset.AccDpt))
                {
                    return Content("[成本中心]不可空白!!");
                }
                // Get Login User's details.
                var ur = _userRepo.Find(usr => usr.UserName == User.Identity.Name).FirstOrDefault();

                asset.Rtp = ur.Id;
                asset.Rtt = DateTime.Now;
                if (asset.DelivUid != null)
                {
                    var delivUser = _context.AppUsers.Where(u => u.Id == asset.DelivUid).FirstOrDefault();
                    asset.DelivEmp = delivUser == null ? "" : delivUser.FullName;
                }
                _context.BMEDAssets.Add(asset);
                AssetKeepModel ak = _context.BMEDAssetKeeps.Find(asset.AssetNo);
                if (ak == null)
                {
                    ak = new AssetKeepModel();
                    ak.AssetNo = asset.AssetNo;
                    ak.KeepYm = asset.KeepYm;
                    ak.Cycle = asset.Cycle;
                    _context.BMEDAssetKeeps.Add(ak);
                }
                else
                {
                    ak.KeepYm = asset.KeepYm;
                    ak.Cycle = asset.Cycle;
                    _context.Entry(ak).State = EntityState.Modified;
                }
                try
                {
                    _context.SaveChanges();
                    return Content("success");
                }
                catch (Exception e)
                {
                    return Content(e.Message);
                }
            }
            else
            {
                string msg = "";
                foreach (var error in ViewData.ModelState.Values.SelectMany(modelState => modelState.Errors))
                {
                    msg += error.ErrorMessage + Environment.NewLine;
                }
                return Content(msg);
            }
        }

        public IActionResult NewEdit(string id)
        {
            AssetModel asset = _context.BMEDAssets.Find(id);
            AssetKeepModel ak = _context.BMEDAssetKeeps.Find(id);
            if (ak != null)
            {
                asset.KeepYm = ak.KeepYm;
                asset.Cycle = ak.Cycle;
            }
            List<SelectListItem> listItem = new List<SelectListItem>();
            listItem.Add(new SelectListItem { Text = "正常", Value = "正常" });
            listItem.Add(new SelectListItem { Text = "報廢", Value = "報廢" });
            ViewData["DKIND"] = new SelectList(listItem, "Value", "Text");
            //
            List<SelectListItem> listItem2 = new List<SelectListItem>();
            List<SelectListItem> delivdpt = new List<SelectListItem>();
            List<SelectListItem> accdpt = new List<SelectListItem>();
            if (asset.DelivUid == null)
            {
                asset.DelivUid = null;
                asset.DelivEmp = "";
                asset.DelivDpt = "";
            }
            listItem2.Add(new SelectListItem { Text = asset.DelivEmp, Value = asset.DelivUid.Value.ToString() });
            List<AppUserModel> ul;
            string gid = "CCH";
            _context.Departments.ToList()
                .ForEach(d => {
                    delivdpt.Add(new SelectListItem
                    {
                        Text = d.Name_C,
                        Value = d.DptId
                    });
                    accdpt.Add(new SelectListItem
                    {
                        Text = d.Name_C,
                        Value = d.DptId
                    });
                });
            ViewData["DelivUids"] = new SelectList(listItem2, "Value", "Text", asset.DelivUid);
            ViewData["DelivDpts"] = new SelectList(delivdpt, "Value", "Text", asset.DelivDpt);
            ViewData["AccDpts"] = new SelectList(accdpt, "Value", "Text", asset.AccDpt);

            return View(asset);
        }

        [HttpPost]
        public IActionResult NewEdit(AssetModel asset)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(asset.LeaveSite))
                {
                    return Content("[放置地點]不可空白!!");
                }
                if (string.IsNullOrEmpty(asset.RiskLvl))
                {
                    return Content("[風險等級]不可空白!!");
                }
                if (asset.KeepYm == null || asset.Cycle == null)
                {
                    return Content("[保養起始年月]或[保養週期]不可空白!!");
                }
                if (string.IsNullOrEmpty(asset.MakeNo))
                {
                    return Content("[製造號碼]不可空白!!");
                }
                //if (asset.RelDate == null)
                //{
                //    return Content("[製造日期]不可空白!!");
                //}

                // Get Login User's details.
                var ur = _userRepo.Find(usr => usr.UserName == User.Identity.Name).FirstOrDefault();

                asset.Rtp = ur.Id;
                asset.Rtt = DateTime.Now;
                if (asset.DelivUid != null)
                {
                    var u = _context.AppUsers.Find(asset.DelivUid);
                    if (u != null)
                    {
                        asset.DelivEmp = u.FullName;
                    }
                }
                _context.Entry(asset).State = EntityState.Modified;
                //
                AssetKeepModel ak = _context.BMEDAssetKeeps.Find(asset.AssetNo);
                if (ak == null)
                {
                    ak = new AssetKeepModel();
                    ak.AssetNo = asset.AssetNo;
                    ak.KeepYm = asset.KeepYm;
                    ak.Cycle = asset.Cycle;
                    _context.BMEDAssetKeeps.Add(ak);
                }
                else
                {
                    ak.KeepYm = asset.KeepYm;
                    ak.Cycle = asset.Cycle;
                    _context.Entry(ak).State = EntityState.Modified;
                }
                try
                {
                    _context.SaveChanges();
                    return Content("success");
                }
                catch (Exception e)
                {
                    return Content(e.Message);
                }
            }
            else
            {
                string msg = "";
                foreach (var error in ViewData.ModelState.Values.SelectMany(modelState => modelState.Errors))
                {
                    msg += error.ErrorMessage + Environment.NewLine;
                }
                return Content(msg);
            }
        }

        public IActionResult BuyEvaluateListItem(string id = null, string upload = null, int page = 1)
        {
            return ViewComponent("BuyEvaluateAssetListItem", new { id = id, upload = upload, page = page });
        }

        public IActionResult Delete2(string id = null)
        {
            AssetModel asset = _context.BMEDAssets.Find(id);
            if (asset == null)
            {
                return NotFound();
            }
            return View(asset);
        }

        [HttpPost]
        public JavaScriptResult DeleteById(string id)
        {
            AssetModel asset = _context.BMEDAssets.Find(id);
            _context.BMEDAssets.Remove(asset);
            AssetKeepModel keep = _context.BMEDAssetKeeps.Find(id);
            _context.BMEDAssetKeeps.Remove(keep);
            _context.SaveChanges();

            return new JavaScriptResult("alert('刪除成功!');window.opener.location.reload();close();");
        }

        public class JavaScriptResult : ContentResult
        {
            public JavaScriptResult(string script)
            {
                this.Content = script;
                this.ContentType = "application/javascript";
            }
        }

    }
}