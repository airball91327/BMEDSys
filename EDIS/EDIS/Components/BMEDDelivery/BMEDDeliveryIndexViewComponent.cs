using EDIS.Areas.BMED.Data;
using EDIS.Areas.BMED.Models.RepairModels;
using EDIS.Models.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDIS.Components.BMEDDelivery
{
    public class BMEDDeliveryIndexViewComponent : ViewComponent
    {
        private readonly BMEDDbContext _context;
        private readonly CustomRoleManager roleManager;

        public BMEDDeliveryIndexViewComponent(BMEDDbContext context,
                                              CustomRoleManager customRoleManager)
        {
            _context = context;
            roleManager = customRoleManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //
            List<SelectListItem> listItem = new List<SelectListItem>();
            listItem.Add(new SelectListItem { Text = "待處理", Value = "待處理" });
            listItem.Add(new SelectListItem { Text = "已處理", Value = "已處理" });
            listItem.Add(new SelectListItem { Text = "已結案", Value = "已結案" });
            ViewData["FLOWTYP"] = new SelectList(listItem, "Value", "Text", "待處理");
            //
            List<SelectListItem> listItem2 = new List<SelectListItem>();
            SelectListItem li;
            _context.Departments.ToList()
                    .ForEach(d =>
                    {
                        li = new SelectListItem();
                        li.Text = d.Name_C;
                        li.Value = d.DptId;
                        listItem2.Add(li);

                    });
            ViewData["ACCDPT"] = new SelectList(listItem2, "Value", "Text");
            ViewData["APPLYDPT"] = new SelectList(listItem2, "Value", "Text");

            return View();
        }
    }
}
