using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using EDIS.Areas.BMED.Data;
using EDIS.Areas.BMED.Models.KeepModels;
using EDIS.Areas.BMED.Models.RepairModels;
using EDIS.Models.Identity;
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
        private readonly CustomUserManager userManager;
        private readonly CustomRoleManager roleManager;
        private int pageSize = 100;

        public AssetController(BMEDDbContext context,
                               CustomRoleManager customRoleManager,
                               CustomUserManager customUserManager)
        {
            _context = context;
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
                .GroupJoin(_context.AppUsers, e => e.Asset.DelivUid, u => u.UserName,
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
            //
            if (at2.ToPagedList(page, pageSize).Count <= 0)
                return PartialView("List", at2.ToPagedList(1, pageSize));
            return PartialView("List", at2.ToPagedList(page, pageSize));
        }

    }
}