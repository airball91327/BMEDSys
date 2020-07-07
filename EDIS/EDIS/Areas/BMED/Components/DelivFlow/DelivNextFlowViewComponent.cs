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
using EDIS.Areas.BMED.Models.DeliveryModels;
using Microsoft.EntityFrameworkCore;

namespace EDIS.Areas.BMED.Components.DelivFlow
{
    public class DelivNextFlowViewComponent : ViewComponent
    {
        private readonly BMEDDbContext _context;
        private readonly IRepository<AppUserModel, int> _userRepo;
        private readonly CustomUserManager userManager;

        public DelivNextFlowViewComponent(BMEDDbContext context,
                                          IRepository<AppUserModel, int> userRepo,
                                          CustomUserManager customUserManager)
        {
            _context = context;
            _userRepo = userRepo;
            userManager = customUserManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var docId = id;
            DelivFlowModel rf = _context.DelivFlows.Where(df => df.DocId == docId && df.Status == "?")
                                                   .ToList().FirstOrDefault();
            if (rf != null)
            {
                rf.DocId = id;
                List<SelectListItem> listItem = new List<SelectListItem>();
                listItem.Add(new SelectListItem { Text = "申請者", Value = "申請者" });
                //listItem.Add(new SelectListItem { Text = "採購人員", Value = "採購人員" });
                //listItem.Add(new SelectListItem { Text = "單位主管", Value = "單位主管" });
                listItem.Add(new SelectListItem { Text = "設備工程師", Value = "設備工程師" });
                listItem.Add(new SelectListItem { Text = "設備主管", Value = "設備主管" });
                listItem.Add(new SelectListItem { Text = "得標廠商", Value = "得標廠商" });
                //listItem.Add(new SelectListItem { Text = "使用單位", Value = "使用單位" });
                listItem.Add(new SelectListItem { Text = "設備經辦", Value = "設備經辦" });
                //listItem.Add(new SelectListItem { Text = "採購主管", Value = "採購主管" });
                if (rf.Cls == "設備主管")
                    listItem.Add(new SelectListItem { Text = "結案", Value = "結案" });
                ViewData["Item"] = new SelectList(listItem, "Value", "Text", "");
                //
                List<SelectListItem> listItem2 = new List<SelectListItem>();
                listItem2.Add(new SelectListItem { Text = "", Value = "" });
                ViewData["Item2"] = new SelectList(listItem2, "Value", "Text", "");
                rf.Cls = "";
                //
                //DeliveryModel ra = _context.Deliveries.Find(id);
                //if (ra != null)
                //{
                //    string cid = _context.AppUsers.Find(ra.UserId).DptId;
                //    string gid = _context.Departments.Find(cid).GroupId;
                //    if (gid != null)
                //    {
                //        rf.FlowHint = db.Groups.Find(gid).FlowHint4;
                //    }
                //}
                rf.SelOpin = "同意";
            }

            return View(rf);
        }
    }
}
