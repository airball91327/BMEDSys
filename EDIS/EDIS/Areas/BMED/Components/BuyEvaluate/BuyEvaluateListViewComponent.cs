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

namespace EDIS.Areas.BMED.Components.BuyFlow
{
    public class BuyEvaluateListViewComponent : ViewComponent
    {
        private readonly BMEDDbContext _context;
        private readonly IRepository<AppUserModel, int> _userRepo;
        private readonly IRepository<DepartmentModel, string> _dptRepo;
        private readonly IEmailSender _emailSender;
        private readonly CustomUserManager userManager;
        private readonly CustomRoleManager roleManager;

        public BuyEvaluateListViewComponent(BMEDDbContext context,
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
            QueryTerms qry = new QueryTerms();
            qry.flowtyp = "待處理";
            var list = new BuyEvaluateController(_context, _userRepo, _dptRepo, _emailSender, userManager, roleManager).GetList(qry, "");
            return View(list);
        }
    }
}
