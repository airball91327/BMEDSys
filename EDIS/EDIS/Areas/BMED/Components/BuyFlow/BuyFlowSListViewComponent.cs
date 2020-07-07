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
    public class BuyFlowSListViewComponent : ViewComponent
    {
        private readonly BMEDDbContext _context;
        private readonly IRepository<AppUserModel, int> _userRepo;
        private readonly CustomUserManager userManager;

        public BuyFlowSListViewComponent(BMEDDbContext context,
                                         IRepository<AppUserModel, int> userRepo,
                                         CustomUserManager customUserManager)
        {
            _context = context;
            _userRepo = userRepo;
            userManager = customUserManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id = null)
        {
            List<BuySFlowModel> sf = _context.BuySFlows.Where(bs => bs.DocId == id).ToList();
            List<BuyFlowModel> rf = new List<BuyFlowModel>();
            List<BuyFlowModel> rf2;
            AppUserModel p;
            foreach (BuySFlowModel f in sf)
            {
                rf2 = _context.BuyFlows.Where(bf => bf.DocId == f.DocSid).ToList();
                foreach (BuyFlowModel f2 in rf2)
                {
                    p = _context.AppUsers.Find(f2.UserId);
                    if (p != null)
                    {
                        f2.UserNam = p.UserName + "(" + p.FullName + ")";
                        rf.Add(f2);
                    }
                }
            }
            return View(rf);
        }
    }
}
