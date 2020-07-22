using EDIS.Areas.BMED.Data;
using EDIS.Areas.BMED.Models.RepairModels;
using EDIS.Models.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDIS.Areas.BMED.Components.RepairCost
{
    public class BMEDRepCostListViewComponent : ViewComponent
    {
        private readonly BMEDDbContext _context;
        private readonly CustomUserManager userManager;

        public BMEDRepCostListViewComponent(BMEDDbContext context,
                                            CustomUserManager customUserManager)
        {
            _context = context;
            userManager = customUserManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id, string viewType)
        {
            List<RepairCostModel> rc = _context.BMEDRepairCosts.Include(r => r.TicketDtl)
                                                               .Where(c => c.DocId == id).ToList();
            rc.ForEach(r => {
                if (r.StockType == "0")
                    r.StockType = "庫存";
                else if (r.StockType == "2")
                    r.StockType = "發票";
                else
                    r.StockType = "簽單";
            });
            //
            var repair = _context.BMEDRepairs.Find(id);
            if (!string.IsNullOrEmpty(repair.AssetNo))
            {
                AssetModel at = _context.BMEDAssets.Find(repair.AssetNo);
                if (at != null)
                {
                    if (at.Cost != null)
                    {
                        if (at.Cost > 0)
                        {
                            decimal repcost = _context.BMEDRepairDtls.Where(m => m.AssetNo != null)
                                .Where(m => m.AssetNo == at.AssetNo).Select(m => m.Cost)
                                .Sum();
                            ViewData["RepRatio"] = decimal.Round(repcost / at.Cost.Value * 100m, 2);
                        }  
                        else
                            ViewData["RepRatio"] = 0;
                    }
                }
            }
            /* Check the device's contract. */
            var repairDtl = _context.BMEDRepairDtls.Find(id);
            if (repairDtl.NotInExceptDevice == "Y") //該案件為統包
            {
                ViewData["HideCost"] = "Y";
            }
            else
            {
                ViewData["HideCost"] = "N";
            }

            if (viewType.Contains("Edit"))
            {
                return View(rc);
            }
            return View("List2", rc);
        }
    }
}
