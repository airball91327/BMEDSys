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
    public class AssetFileCheckFilesViewComponent : ViewComponent
    {
        private readonly BMEDDbContext _context;
        private readonly IRepository<AppUserModel, int> _userRepo;
        private readonly CustomUserManager userManager;
        private readonly CustomRoleManager roleManager;
        private int pageSize = 100;

        public AssetFileCheckFilesViewComponent(BMEDDbContext context,
                                                IRepository<AppUserModel, int> userRepo,
                                                CustomUserManager customUserManager,
                                                CustomRoleManager customRoleManager)
        {
            _context = context;
            _userRepo = userRepo;
            userManager = customUserManager;
            roleManager = customRoleManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id = null, string cls = null)
        {
            if (id != null)
            {
                List<AssetFileModel> af = _context.AssetFiles.Where(f => f.AssetNo == id).ToList();
                List<NeedFileModel> nf = new List<NeedFileModel>();
                if (cls == "得標廠商")
                    nf = _context.NeedFiles.Where(e => e.Type == "1").ToList();
                else if (cls == "設備工程師")
                    nf = _context.NeedFiles.Where(e => e.Type == "2").ToList();
                foreach (NeedFileModel n in nf)
                {
                    if (af.Where(f => f.Title == n.Title).Count() <= 0)
                        return Content("檔案尚未上載完成!!");
                }
            }
            return Content("");
        }
    }
}
