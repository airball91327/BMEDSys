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
using EDIS.Areas.BMED.Models;
using EDIS.Areas.BMED.Controllers;
using EDIS.Services;

namespace EDIS.Components.BMEDBuyEvaluate
{
    public class BuyEvaluateIndexViewComponent : ViewComponent
    {
        private readonly BMEDDbContext _context;
        private readonly IRepository<AppUserModel, int> _userRepo;
        private readonly IRepository<DepartmentModel, string> _dptRepo;
        private readonly IEmailSender _emailSender;
        private readonly CustomUserManager userManager;
        private readonly CustomRoleManager roleManager;

        public BuyEvaluateIndexViewComponent(BMEDDbContext context,
                                             IRepository<AppUserModel, int> userRepo,
                                             IRepository<DepartmentModel, string> dptRepo,
                                             IEmailSender emailSender,
                                             CustomUserManager customUserManager,
                                             CustomRoleManager customRoleManager)
        {
            _context = context;
            _userRepo = userRepo;
            _dptRepo = dptRepo;
            _emailSender = emailSender;
            userManager = customUserManager;
            roleManager = customRoleManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id = null, string sid = null)
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            listItem.Add(new SelectListItem { Text = "待處理", Value = "待處理" });
            listItem.Add(new SelectListItem { Text = "已處理", Value = "已處理" });
            listItem.Add(new SelectListItem { Text = "已結案", Value = "已結案" });
            ViewData["Item"] = new SelectList(listItem, "Value", "Text", "待處理");
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
            ViewData["ApplyDpt"] = new SelectList(listItem2, "Value", "Text");

            return View();
        }
    }
}
