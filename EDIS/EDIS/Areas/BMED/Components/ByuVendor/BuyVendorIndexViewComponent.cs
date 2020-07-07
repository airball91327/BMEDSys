using EDIS.Areas.BMED.Data;
using EDIS.Models.Identity;
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
    public class BuyVendorIndexViewComponent : ViewComponent
    {
        private readonly BMEDDbContext _context;

        public BuyVendorIndexViewComponent(BMEDDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id = null)
        {
            List<BuyVendorModel> t = _context.BuyVendors.Where(c => c.DocId == id).ToList();
            return View(t);
        }
    }
}
