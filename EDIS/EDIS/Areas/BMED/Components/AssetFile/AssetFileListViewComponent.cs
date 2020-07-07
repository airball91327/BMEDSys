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

namespace EDIS.Areas.BMED.Components.AssetFile
{
    public class AssetFileListViewComponent : ViewComponent
    {
        private readonly BMEDDbContext _context;
        private readonly IRepository<AppUserModel, int> _userRepo;
        private readonly CustomUserManager userManager;
        private readonly CustomRoleManager roleManager;
        private int pageSize = 100;

        public AssetFileListViewComponent(BMEDDbContext context,
                                          IRepository<AppUserModel, int> userRepo,
                                          CustomUserManager customUserManager,
                                          CustomRoleManager customRoleManager)
        {
            _context = context;
            _userRepo = userRepo;
            userManager = customUserManager;
            roleManager = customRoleManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id = null, string title = null)
        {
            List<AssetFileModel> af = new List<AssetFileModel>();
            if (id != null)
            {
                AppUserModel u;
                af = _context.AssetFiles.Where(f => f.AssetNo == id).ToList();
                if (title != null)
                    af = af.Where(f => f.Title == title).ToList();
                foreach (AssetFileModel a in af)
                {
                    u = _context.AppUsers.Find(Convert.ToInt32(a.Rtp));
                    a.UserName = u.FullName;
                }
            }
            else
            {
                AppUserModel u;
                af = _context.AssetFiles.ToList();
                foreach (AssetFileModel a in af)
                {
                    u = _context.AppUsers.Find(Convert.ToInt32(a.Rtp));
                    a.UserName = u.FullName;
                }
            }
            return View(af);
        }
    }
}
