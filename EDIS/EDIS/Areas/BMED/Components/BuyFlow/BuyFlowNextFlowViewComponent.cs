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

namespace EDIS.Areas.BMED.Components.BuyFlow
{
    public class BuyFlowNextFlowViewComponent : ViewComponent
    {
        private readonly BMEDDbContext _context;
        private readonly IRepository<AppUserModel, int> _userRepo;
        private readonly CustomUserManager userManager;

        public BuyFlowNextFlowViewComponent(BMEDDbContext context,
                                            IRepository<AppUserModel, int> userRepo,
                                            CustomUserManager customUserManager)
        {
            _context = context;
            _userRepo = userRepo;
            userManager = customUserManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id = null, string sid = null)
        {
            BuyFlowModel rf;
            if (sid != null)
                rf = _context.BuyFlows.Where(bf => bf.DocId == sid && bf.Status == "?").ToList().FirstOrDefault();
            else
                rf = _context.BuyFlows.Where(bf => bf.DocId == id && bf.Status == "?").ToList().FirstOrDefault();

            List<SelectListItem> listItem = new List<SelectListItem>();
            if (sid == null)
            {
                listItem.Add(new SelectListItem { Text = "設備工程師", Value = "設備工程師" });
                listItem.Add(new SelectListItem { Text = "申請者", Value = "申請者" });
                //listItem.Add(new SelectListItem { Text = "採購人員", Value = "採購人員" });
                listItem.Add(new SelectListItem { Text = "評估工程師", Value = "評估工程師" });
                listItem.Add(new SelectListItem { Text = "設備主管", Value = "設備主管" });
                listItem.Add(new SelectListItem { Text = "設備經辦", Value = "設備經辦" });
                //listItem.Add(new SelectListItem { Text = "採購主管", Value = "採購主管" });
                if (rf.Cls == "設備經辦")
                    listItem.Add(new SelectListItem { Text = "結案", Value = "結案" });
            }
            else
            {
                if (rf.Cls == "申請者")
                {
                    listItem.Add(new SelectListItem { Text = "設備工程師", Value = "設備工程師" });
                    listItem.Add(new SelectListItem { Text = "廢除", Value = "廢除" });
                }
                else if (rf.Cls == "設備工程師")
                    listItem.Add(new SelectListItem { Text = "設備主管", Value = "設備主管" });
                else if (rf.Cls == "單位主管")
                {
                    listItem.Add(new SelectListItem { Text = "申請者", Value = "申請者" });
                    //listItem.Add(new SelectListItem { Text = "結案", Value = "結案" });
                }
                else if (rf.Cls == "設備主管")
                {
                    listItem.Add(new SelectListItem { Text = "設備工程師", Value = "設備工程師" });
                    listItem.Add(new SelectListItem { Text = "設備經辦", Value = "設備經辦" });
                    //listItem.Add(new SelectListItem { Text = "結案", Value = "結案" });
                }
                else if (rf.Cls == "設備經辦")
                {
                    listItem.Add(new SelectListItem { Text = "設備主管", Value = "設備主管" });
                    listItem.Add(new SelectListItem { Text = "結案", Value = "結案" });
                }
            }
            ViewData["Item"] = new SelectList(listItem, "Value", "Text", "");
            //
            List<SelectListItem> listItem2 = new List<SelectListItem>();
            listItem2.Add(new SelectListItem { Text = "", Value = "" });
            ViewData["Item2"] = new SelectList(listItem2, "Value", "Text", "");
            rf.Cls = "";
            //
            BuyEvaluateModel ra = _context.BuyEvaluates.Find(id);
            if (ra != null)
            {
                string cid = _context.AppUsers.Find(ra.UserId).DptId;
            }
            rf.SelOpin = "同意";

            return View(rf);
        }
    }
}
