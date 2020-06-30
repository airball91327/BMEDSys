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
using EDIS.Areas.BMED.Models.BuyEvaluateModels;
using Microsoft.EntityFrameworkCore;

namespace EDIS.Areas.BMED.Components.BuyFlow
{
    public class BuyFlowListViewComponent : ViewComponent
    {
        private readonly BMEDDbContext _context;
        private readonly IRepository<AppUserModel, int> _userRepo;
        private readonly CustomUserManager userManager;

        public BuyFlowListViewComponent(BMEDDbContext context,
                                        IRepository<AppUserModel, int> userRepo,
                                        CustomUserManager customUserManager)
        {
            _context = context;
            _userRepo = userRepo;
            userManager = customUserManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id = null, string sid = null)
        {
            List<BuyFlowModel> rf = _context.BuyFlows.Where(bf => bf.DocId == id).ToList();
            AppUserModel p;
            foreach (BuyFlowModel f in rf)
            {
                p = _context.AppUsers.Find(f.UserId);
                f.UserNam = p.UserName + "(" + p.FullName + ")";
                if (f.Status == "?")
                    ViewData["cls_now"] = f.Cls;
            }
            if (sid != null)
            {
                List<BuyFlowModel> rf2 = _context.BuyFlows.Where(bf => bf.DocId == sid).ToList();
                foreach (BuyFlowModel f in rf2)
                {
                    p = _context.AppUsers.Find(f.UserId);
                    f.UserNam = p.UserName + "(" + p.FullName + ")";
                    if (f.Status == "?")
                        ViewData["cls_now"] = f.Cls;
                    rf.Add(f);
                }
            }
            return View(rf);
        }
    }
}
