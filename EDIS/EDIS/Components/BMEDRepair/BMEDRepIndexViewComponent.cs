using EDIS.Areas.BMED.Data;
using EDIS.Areas.BMED.Models.RepairModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDIS.Components.Repair
{
    public class BMEDRepIndexViewComponent : ViewComponent
    {
        private readonly BMEDDbContext _context;

        public BMEDRepIndexViewComponent(BMEDDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<SelectListItem> FlowlistItem = new List<SelectListItem>();
            FlowlistItem.Add(new SelectListItem { Text = "待簽核", Value = "待簽核" });
            FlowlistItem.Add(new SelectListItem { Text = "流程中", Value = "流程中" });
            FlowlistItem.Add(new SelectListItem { Text = "已結案", Value = "已結案" });
            ViewData["BMEDFlowType"] = new SelectList(FlowlistItem, "Value", "Text", "待簽核");

            /* 成本中心 & 申請部門的下拉選單資料 */
            var dptList = new[] { "K", "P", "C" };   //本院部門
            var departments = _context.Departments.Where(d => dptList.Contains(d.Loc)).ToList();
            List<SelectListItem> listItem = new List<SelectListItem>();
            foreach (var item in departments)
            {
                listItem.Add(new SelectListItem
                {
                    Text = item.Name_C + "(" + item.DptId + ")",    //show DptName(DptId)
                    Value = item.DptId
                });
            }
            ViewData["BMEDAccDpt"] = new SelectList(listItem, "Value", "Text");
            ViewData["BMEDApplyDpt"] = new SelectList(listItem, "Value", "Text");

            /* 處理狀態的下拉選單 */
            var dealStatuses = _context.BMEDDealStatuses.ToList();
            List<SelectListItem> listItem2 = new List<SelectListItem>();
            foreach (var item in dealStatuses)
            {
                listItem2.Add(new SelectListItem
                {
                    Text = item.Title,
                    Value = item.Title
                });
            }
            ViewData["BMEDDealStatus"] = new SelectList(listItem2, "Value", "Text");

            /* 處理有無費用的下拉選單 */
            List<SelectListItem> listItem3 = new List<SelectListItem>();
            listItem3.Add(new SelectListItem { Text = "有", Value = "Y" });
            listItem3.Add(new SelectListItem { Text = "無", Value = "N" });
            ViewData["BMEDIsCharged"] = new SelectList(listItem3, "Value", "Text");

            QryRepListData data = new QryRepListData();
            /* Set Default search value. */
            data.BMEDqtyDateType = "申請日";

            return View(data);
        }
    }
}
