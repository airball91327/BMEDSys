using EDIS.Areas.BMED.Data;
using EDIS.Models.Identity;
using EDIS.Areas.BMED.Models.RepairModels;
using EDIS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDIS.Areas.BMED.Models.BuyEvaluateModels;

namespace EDIS.Areas.BMED.Components.AttainFile
{
    public class BMEDAttainFileVenderPListViewComponent : ViewComponent
    {
        private readonly BMEDDbContext _context;
        private readonly IRepository<AppUserModel, int> _userRepo;
        private readonly CustomUserManager userManager;

        public BMEDAttainFileVenderPListViewComponent(BMEDDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id = null)
        {
            List<AttainFileModel> af = new List<AttainFileModel>();
            List<AttainFileModel> af2 = new List<AttainFileModel>();
            if (id != null)
            {
                AppUserModel u;
                af = _context.BMEDAttainFiles.Where(f => f.DocType == "3").Where(f => f.DocId == id).ToList();
                foreach (AttainFileModel a in af)
                {
                    if (a.IsPublic == "Y")
                    {
                        BuyVendorModel b = _context.BuyVendors.Find(a.DocId, a.Rtp);
                        if (b != null)
                            a.UserName = b.VendorNam;
                        else
                        {
                            if (a.Rtp != null){
                                u = _context.AppUsers.Find(a.Rtp);
                                a.UserName = u.FullName;
                            }
                        }
                        af2.Add(a);
                    }
                }
            }
            else
            {
                AppUserModel u;
                af = _context.BMEDAttainFiles.ToList();
                foreach (AttainFileModel a in af)
                {
                    if (a.Rtp != null)
                    {
                        u = _context.AppUsers.Find(a.Rtp);
                        a.UserName = u.FullName;
                    }
                }
            }
            return View(af2);
        }
    }
}
