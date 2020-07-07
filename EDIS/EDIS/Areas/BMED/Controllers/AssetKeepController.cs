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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EDIS.Areas.BMED.Controllers
{
    [Area("BMED")]
    [Authorize]
    public class AssetKeepController : Controller
    {
        private readonly BMEDDbContext _context;
        private readonly IRepository<AppUserModel, int> _userRepo;
        private readonly CustomUserManager userManager;
        private readonly CustomRoleManager roleManager;

        public AssetKeepController(BMEDDbContext context,
                                   IRepository<AppUserModel, int> userRepo,
                                   CustomRoleManager customRoleManager,
                                   CustomUserManager customUserManager)
        {
            _context = context;
            _userRepo = userRepo;
            roleManager = customRoleManager;
            userManager = customUserManager;
        }

        // GET: BMED/AssetKeep
        public IActionResult Index()
        {
            return View();
        }

        // GET: BMED/AssetKeep/Details/5
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            AssetKeepModel assetKeep = _context.BMEDAssetKeeps.Find(id);
            if (assetKeep == null)
            {
                return StatusCode(404);
            }
            assetKeep.KeepEngName = assetKeep.KeepEngId == 0 ? "" : _context.AppUsers.Find(assetKeep.KeepEngId).FullName;
            return PartialView(assetKeep);
        }

        // GET: BMED/AssetKeep/Edit/5
        public IActionResult Edit(string id)
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            AppUserModel u;
            //db.AppUsers.ToList().ForEach(d =>
            //{
            //    listItem.Add(new SelectListItem { Text = d.FullName, Value = d.Id.ToString() });
            //});

            // Get MedEngineers to set dropdownlist.
            var s = roleManager.GetUsersInRole("MedEngineer").ToList();
            foreach (string l in s)
            {
                u = _context.AppUsers.Where(ur => ur.UserName == l).FirstOrDefault();
                if (u != null)
                {
                    listItem.Add(new SelectListItem { Text = u.FullName, Value = u.Id.ToString() });
                }
            }
            ViewData["KeepEngId"] = new SelectList(listItem, "Value", "Text", "");

            List<SelectListItem> listItem2 = new List<SelectListItem>();
            listItem2.Add(new SelectListItem { Text = "自行", Value = "自行" });
            listItem2.Add(new SelectListItem { Text = "委外", Value = "委外" });
            listItem2.Add(new SelectListItem { Text = "保固", Value = "保固" });
            listItem2.Add(new SelectListItem { Text = "租賃", Value = "租賃" });
            ViewData["InOut"] = new SelectList(listItem2, "Value", "Text", "");
            //
            List<SelectListItem> listItem3 = new List<SelectListItem>();
            _context.BMEDKeepFormats.ToList()
                .ForEach(x =>
                {
                    listItem3.Add(new SelectListItem { Text = x.FormatId, Value = x.FormatId });
                });
            ViewData["FormatId"] = new SelectList(listItem3, "Value", "Text", "");
            //
            if (id == null)
            {
                return PartialView();
            }
            AssetKeepModel assetKeep = _context.BMEDAssetKeeps.Find(id);
            if (assetKeep == null)
            {
                assetKeep = new AssetKeepModel();
                assetKeep.AssetNo = id;
            }
            return PartialView(assetKeep);
        }

        // POST: BMED/AssetKeep/Edit/5
        [HttpPost]
        public IActionResult Edit(AssetKeepModel assetKeep)
        {
            if (ModelState.IsValid)
            {
                assetKeep.KeepEngName = _context.AppUsers.Find(assetKeep.KeepEngId).FullName;
                _context.Entry(assetKeep).State = EntityState.Modified;
                _context.SaveChanges();
                return new JsonResult(assetKeep)
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

        public IActionResult EditData(string ano = null, string id = null)
        {
            var ur = _userRepo.Find(usr => usr.UserName == this.User.Identity.Name).FirstOrDefault();

            AssetModel at = _context.BMEDAssets.Find(ano);
            DeliveryModel d = _context.Deliveries.Find(id);
            int vid = d.VendorId != null ? Convert.ToInt32(d.VendorId) : 0;
            VendorModel v = _context.BMEDVendors.Where(vv => vv.VendorId == vid).ToList().FirstOrDefault();
            List<string> s;
            SelectListItem li;
            s = roleManager.GetUsersInRole("Engineer").ToList();
            List<SelectListItem> list = new List<SelectListItem>();
            AppUserModel u;
            foreach (string l in s)
            {
                u = _context.AppUsers.Where(usr => usr.UserName == l).FirstOrDefault();
                if (u != null)
                {
                    if (u.VendorId != null)
                    {
                        if (u.VendorId == v.VendorId)
                        {
                            li = new SelectListItem();
                            li.Text = u.FullName;
                            li.Value = u.Id.ToString();
                            list.Add(li);
                        }
                    }
                }
            }
            DepartmentModel dpt = _context.Departments.Find(at.DelivDpt);
            DepartmentModel g;
            if (dpt != null)
            {
                s = roleManager.GetUsersInRole("MedEngineer").ToList();
                foreach (string l in s)
                {
                    u = _context.AppUsers.Where(usr => usr.UserName == l).FirstOrDefault();
                    if (u != null)
                    {
                        if (u.DptId != null)
                        {
                            g = _context.Departments.Find(u.DptId);
                            if (g.DptId == dpt.DptId)
                            {
                                li = new SelectListItem();
                                li.Text = u.FullName;
                                li.Value = u.Id.ToString();
                                list.Add(li);
                            }
                        }
                    }
                }
            }
            ViewData["Items"] = new SelectList(list, "Value", "Text", "");
            List<SelectListItem> listItem = new List<SelectListItem>();
            listItem.Add(new SelectListItem { Text = "自行", Value = "自行" });
            listItem.Add(new SelectListItem { Text = "委外", Value = "委外" });
            listItem.Add(new SelectListItem { Text = "保固", Value = "保固" });
            listItem.Add(new SelectListItem { Text = "租賃", Value = "租賃" });
            ViewData["INOUTITEMS"] = new SelectList(listItem, "Value", "Text", "");
            //
            List<SelectListItem> list2 = new List<SelectListItem>();
            List<KeepFormatModel> kf = _context.BMEDKeepFormats.ToList();
            foreach (KeepFormatModel k in kf)
            {
                li = new SelectListItem { Text = k.FormatId, Value = k.FormatId };
                list2.Add(li);
            }
            ViewData["FORMATITEMS"] = new SelectList(list2, "Value", "Text", "");
            //
            AssetKeepModel assetkeep = _context.BMEDAssetKeeps.Find(ano);
            if (assetkeep == null)
            {
                return NotFound();
            }
            assetkeep.Cname = _context.BMEDAssets.Find(assetkeep.AssetNo).Cname;
            if (assetkeep.KeepYm == null)
            {
                assetkeep.KeepYm = (d.DelivDateR.Year - 1911) * 100 + d.DelivDateR.Month;
            }
            return PartialView(assetkeep);
        }

        [HttpPost]
        public JsonResult EditData(AssetKeepModel assetkeep)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(assetkeep).State = EntityState.Modified;
                try
                {
                    _context.SaveChanges();
                    return Json(new { success = true, msg = "儲存成功!" });
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                    return Json(new { success = false, msg = s });
                }
            }
            return Json(new { success = false, msg = "儲存失敗!" });
        }

        // POST: BMED/AssetKeep/UpdEngineer/5
        [HttpPost]
        public ActionResult UpdEngineer(string id, string assets)
        {
            string[] s = assets.Split(new char[] { ';' });
            AssetKeepModel assetKeep;
            foreach (string ss in s)
            {
                assetKeep = _context.BMEDAssetKeeps.Find(ss);
                if (assetKeep != null)
                {
                    AppUserModel u = _context.AppUsers.Find(Convert.ToInt32(id));
                    if (u != null)
                    {
                        assetKeep.KeepEngId = u.Id;
                        assetKeep.KeepEngName = u.FullName;
                        _context.Entry(assetKeep).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                }
            }
            return new JsonResult(id)
            {
                Value = new { success = true, error = "" }
            };
        }

    }
}