using EDIS.Areas.BMED.Data;
using EDIS.Models.Identity;
using EDIS.Areas.BMED.Models.KeepModels;
using EDIS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using EDIS.Areas.BMED.Models.RepairModels;
using EDIS.Areas.BMED.Models.DeliveryModels;

namespace EDIS.Areas.BMED.Components.Asset
{
    public class BuyEvaluateAssetListItemViewComponent : ViewComponent
    {
        private readonly BMEDDbContext _context;
        private readonly IRepository<AppUserModel, int> _userRepo;
        private readonly CustomUserManager userManager;
        private readonly CustomRoleManager roleManager;
        private int pageSize = 100;

        public BuyEvaluateAssetListItemViewComponent(BMEDDbContext context,
                                                     IRepository<AppUserModel, int> userRepo,
                                                     CustomUserManager customUserManager,
                                                     CustomRoleManager customRoleManager)
        {
            _context = context;
            _userRepo = userRepo;
            userManager = customUserManager;
            roleManager = customRoleManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id = null, string upload = null, int page = 1)
        {

            List<AssetModel> at = new List<AssetModel>();
            if (upload == "採購人員")
            {
                DeliveryModel dd = _context.Deliveries.Find(id);
                if (dd != null)
                    at = _context.BMEDAssets.Where(a => a.Docid == dd.DocId).ToList();
            }
            else
                at = _context.BMEDAssets.Where(a => a.Docid == id).ToList();
            foreach (AssetModel e in at)
            {
                e.upload = upload;
            }
            if (upload == "查看")
                return View("_BuyEvaluateListItem2", at);
            if (at.ToPagedList(page, pageSize).Count <= 0)
                return View(at.ToPagedList(1, pageSize));
            return View(at.ToPagedList(page, pageSize));
        }
    }
}
