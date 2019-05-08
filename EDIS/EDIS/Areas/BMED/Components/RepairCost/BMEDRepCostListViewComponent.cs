using EDIS.Areas.BMED.Data;
using EDIS.Areas.BMED.Models.RepairModels;
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

        public BMEDRepCostListViewComponent(BMEDDbContext context)
        {
            _context = context;
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

            if (viewType.Contains("Edit"))
            {
                return View(rc);
            }
            return View("List2", rc);
        }
    }
}
