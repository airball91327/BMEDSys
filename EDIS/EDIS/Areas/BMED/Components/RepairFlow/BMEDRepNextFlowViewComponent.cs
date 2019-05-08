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

namespace EDIS.Areas.BMED.Components.RepairFlow
{
    public class BMEDRepNextFlowViewComponent : ViewComponent
    {
        private readonly BMEDDbContext _context;
        private readonly BMEDIRepository<RepairFlowModel, string[]> _repflowRepo;
        private readonly IRepository<AppUserModel, int> _userRepo;
        private readonly CustomUserManager userManager;

        public BMEDRepNextFlowViewComponent(BMEDDbContext context,
                                            BMEDIRepository<RepairFlowModel, string[]> repairflowRepo,
                                            IRepository<AppUserModel, int> userRepo,
                                            CustomUserManager customUserManager)
        {
            _context = context;
            _repflowRepo = repairflowRepo;
            _userRepo = userRepo;
            userManager = customUserManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            /* Get repair and flow details. */
            RepairModel repair = _context.BMEDRepairs.Find(id);
            RepairFlowModel repairFlow = _context.BMEDRepairFlows.Where(f => f.DocId == id && f.Status == "?")
                                                                 .FirstOrDefault();
            /* 增設流程 */
            List<SelectListItem> listItem = new List<SelectListItem>();
            if (repair.RepType == "增設")
            {
                listItem.Add(new SelectListItem { Text = "申請人", Value = "申請人" });
                listItem.Add(new SelectListItem { Text = "驗收人", Value = "驗收人" });
                listItem.Add(new SelectListItem { Text = "單位主管", Value = "單位主管" });
                listItem.Add(new SelectListItem { Text = "單位主任", Value = "單位主任" });
                listItem.Add(new SelectListItem { Text = "單位副院長", Value = "單位副院長" });
                listItem.Add(new SelectListItem { Text = "醫工工程師", Value = "醫工工程師" });
                listItem.Add(new SelectListItem { Text = "醫工主管", Value = "醫工主管" });
                listItem.Add(new SelectListItem { Text = "醫工主任", Value = "醫工主任" });
            }
            else  //維修流程
            {
                listItem.Add(new SelectListItem { Text = "申請人", Value = "申請人" });
                listItem.Add(new SelectListItem { Text = "驗收人", Value = "驗收人" });
                listItem.Add(new SelectListItem { Text = "單位主管", Value = "單位主管" });
                listItem.Add(new SelectListItem { Text = "醫工工程師", Value = "醫工工程師" });
                listItem.Add(new SelectListItem { Text = "醫工主管", Value = "醫工主管" });
                listItem.Add(new SelectListItem { Text = "醫工主任", Value = "醫工主任" });
                listItem.Add(new SelectListItem { Text = "醫工經辦", Value = "醫工經辦" });
            }
            listItem.Add(new SelectListItem { Text = "列管財產負責人", Value = "列管財產負責人" });
            listItem.Add(new SelectListItem { Text = "固資財產負責人", Value = "固資財產負責人" });
            listItem.Add(new SelectListItem { Text = "其他", Value = "其他" });
            /* Insert values. */
            AssignModel assign = new AssignModel();
            assign.DocId = id;

            /* 根據當下流程的人員做額外的流程控管 */
            if (repairFlow != null)
            {
                assign.Cls = repairFlow.Cls;

                if (repairFlow.Cls == "驗收人")    //統一驗收人結案
                {
                    listItem.Add(new SelectListItem { Text = "結案", Value = "結案" });
                }
                if (repairFlow.Cls == "醫工工程師")   //醫工工程師自己為驗收人時
                {
                    if (repair.CheckerId == repairFlow.UserId)  //驗收人為自己
                    {
                        listItem.Add(new SelectListItem { Text = "結案", Value = "結案" });
                    }
                }
            }
            ViewData["FlowCls"] = new SelectList(listItem, "Value", "Text", "");

            List<SelectListItem> listItem3 = new List<SelectListItem>();
            listItem3.Add(new SelectListItem { Text = "", Value = "" });
            ViewData["FlowUid"] = new SelectList(listItem3, "Value", "Text", "");

            assign.Hint = "單位請修→工務工程師→工務主管(若有費用)→單位驗收人→結案。";
            ViewData["Hint2"] = "單位請修→單位主管(護理長)→單位主任→工務工程師 [若費用5000元以上：單位主管(護理長)→" +
                "單位直屬副院長→工務工程師] →工務主管→工務工程師→工務主管→單位驗收人→結案";
            ViewData["Hint3"] = "單位請修→工務工程師→工務主管→工務主任→列管/固資財產負責人(有財產編號時)→單位驗收人→結案";

            /* 於流程頁面顯示請修類型、及處理狀態*/
            string hintRepType = repair.RepType;
            string hintRepState = "";
            var repDtl = _context.BMEDRepairDtls.Where(dtl => dtl.DocId == id).FirstOrDefault();
            if (repDtl != null)
            {
                hintRepState = _context.BMEDDealStatuses.Find(repDtl.DealState).Title;
            }
            ViewData["HintRepType"] = hintRepType + " / " + hintRepState;

            return View(assign);
        }
    }
}
