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

namespace EDIS.Areas.BMED.Components.BuyVendor
{
    public class BuyVendorStatusListViewComponent : ViewComponent
    {
        private readonly BMEDDbContext _context;

        public BuyVendorStatusListViewComponent(BMEDDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id = null)
        {
            List<BuyVendorModel> tv = _context.BuyVendors.Where(c => c.DocId == id).ToList();
            foreach (BuyVendorModel b in tv)
            {
                if (b.Status == "?")
                    b.Status = "未完成";
                else if (b.Status == "2")
                    b.Status = "已完成";
            }
            return View(tv);
        }
    }
}
