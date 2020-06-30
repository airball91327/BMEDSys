using EDIS.Areas.BMED.Data;
using EDIS.Models.Identity;
using EDIS.Areas.BMED.Models.RepairModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDIS.Areas.BMED.Models.BuyEvaluateModels;

namespace EDIS.Areas.BMED.Components.AttainFile
{
    public class BMEDAttainFileVendorListViewComponent : ViewComponent
    {
        private readonly BMEDDbContext _context;

        public BMEDAttainFileVendorListViewComponent(BMEDDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id = null, string typ = null, string uniteno = null)
        {
            List<AttainFileModel> af = new List<AttainFileModel>();
            List<AttainFileModel> af2 = new List<AttainFileModel>();
            if (id != null)
            {
                AppUserModel u;
                int uno = Convert.ToInt32(uniteno);
                af = _context.BMEDAttainFiles.Where(f => f.DocType == typ).Where(f => f.DocId == id).ToList();
                foreach (AttainFileModel a in af)
                {
                    if (a.DocType == "3" && (a.Rtp == uno || a.IsPublic == "Y"))
                    {
                        BuyVendorModel b = _context.BuyVendors.Find(a.DocId, a.Rtp);
                        if (b != null)
                            a.UserName = b.VendorNam;
                        else
                        {
                            if (a.Rtp != null)
                            {
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
