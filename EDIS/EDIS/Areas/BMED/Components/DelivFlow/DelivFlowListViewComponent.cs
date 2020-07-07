using EDIS.Areas.BMED.Data;
using EDIS.Models.Identity;
using EDIS.Areas.BMED.Models.RepairModels;
using EDIS.Areas.BMED.Repositories;
using EDIS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDIS.Areas.BMED.Models.DeliveryModels;
using Microsoft.EntityFrameworkCore;

namespace EDIS.Areas.BMED.Components.DelivFlow
{
    public class DelivFlowListViewComponent : ViewComponent
    {
        private readonly BMEDDbContext _context;
        private readonly IRepository<AppUserModel, int> _userRepo;
        private readonly CustomUserManager userManager;

        public DelivFlowListViewComponent(BMEDDbContext context,
                                          IRepository<AppUserModel, int> userRepo,
                                          CustomUserManager customUserManager)
        {
            _context = context;
            _userRepo = userRepo;
            userManager = customUserManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var docId = id;
            List<DelivFlowModel> rf = _context.DelivFlows.Where(df => df.DocId == docId).ToList();
            AppUserModel p;
            foreach (DelivFlowModel f in rf)
            {
                p = _context.AppUsers.Find(f.UserId);
                if (p != null)
                {
                    f.UserNam = p.UserName + "(" + p.FullName + ")";
                }
                if (f.Status == "?")
                    ViewData["cls_now"] = f.Cls;
            }
            return View(rf);
        }
    }
}
