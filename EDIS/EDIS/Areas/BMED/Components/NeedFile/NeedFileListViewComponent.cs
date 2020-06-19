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
    public class NeedFileListViewComponent : ViewComponent
    {
        private readonly BMEDDbContext _context;
        private readonly IRepository<AppUserModel, int> _userRepo;
        private readonly CustomUserManager userManager;
        private readonly CustomRoleManager roleManager;
        private int pageSize = 100;

        public NeedFileListViewComponent(BMEDDbContext context,
                                         IRepository<AppUserModel, int> userRepo,
                                         CustomUserManager customUserManager,
                                         CustomRoleManager customRoleManager)
        {
            _context = context;
            _userRepo = userRepo;
            userManager = customUserManager;
            roleManager = customRoleManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id = null, string up = null)
        {
            ViewData["ano"] = id;
            List<NeedFileModel> nf = new List<NeedFileModel>();
            if (up == null)
                nf = _context.NeedFiles.ToList();
            else if (up == "得標廠商")
            {
                nf = _context.NeedFiles.Where(f => f.Type == "1").ToList();
            }
            else if (up == "設備工程師")
            {
                nf = _context.NeedFiles.Where(f => f.Type != "3" && f.Type != "9").ToList();
            }
            else if (up == "維修工程師")
            {
                nf = _context.NeedFiles.Where(f => f.Type == "3").ToList();
            }
            else if (up == "採購人員")
            {
                NeedFileModel n = new NeedFileModel();
                n.SeqNo = 12;
                n.Title = "其他";
                n.FileDes = "";
                nf.Add(n);
            }
            return View(nf);
        }
    }
}
