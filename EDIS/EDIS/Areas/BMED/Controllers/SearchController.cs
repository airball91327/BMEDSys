using EDIS.Areas.BMED.Data;
using EDIS.Areas.BMED.Models.RepairModels;
using EDIS.Areas.BMED.Models.KeepModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDIS.Models.Identity;
using Microsoft.EntityFrameworkCore;
using EDIS.Areas.BMED.Models.DeliveryModels;
using Microsoft.AspNetCore.Http;
using EDIS.Repositories;
using EDIS.Areas.BMED.Models.MedTransRd;

namespace EDIS.Areas.BMED.Controllers
{
    [Area("BMED")]
    [Authorize]
    public class SearchController : Controller
    {
        private readonly BMEDDbContext _context;
        private readonly IRepository<AppUserModel, int> _userRepo;
        private readonly CustomUserManager userManager;
        private readonly CustomRoleManager roleManager;

        public SearchController(BMEDDbContext context,
                                IRepository<AppUserModel, int> userRepo,
                                CustomUserManager customUserManager,
                                CustomRoleManager customRoleManager)
        {
            _context = context;
            _userRepo = userRepo;
            userManager = customUserManager;
            roleManager = customRoleManager;
        }

        /// <summary>
        /// The Index of searching all repair docs.
        /// </summary>
        /// <returns></returns>
        // GET: BMED/Search/RepIndex
        public IActionResult RepIndex()
        {
            List<SelectListItem> FlowlistItem = new List<SelectListItem>();
            FlowlistItem.Add(new SelectListItem { Text = "未結案", Value = "未結案" });
            FlowlistItem.Add(new SelectListItem { Text = "已結案", Value = "已結案" });
            ViewData["FLOWTYPE"] = new SelectList(FlowlistItem, "Value", "Text");

            /* 成本中心 & 申請部門的下拉選單資料 */
            var dptList = new[] { "K", "P", "C" };  //本院部門
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

            ViewData["ACCDPT"] = new SelectList(listItem, "Value", "Text");
            ViewData["APPLYDPT"] = new SelectList(listItem, "Value", "Text");

            /* 處理狀態的下拉選單 */
            var dealStatuses = _context.BMEDDealStatuses.ToList();
            List<SelectListItem> listItem2 = new List<SelectListItem>();
            foreach (var item in dealStatuses)
            {
                listItem2.Add(new SelectListItem
                {
                    Text = item.Title,
                    Value = item.Id.ToString()
                });
            }
            ViewData["DealStatus"] = new SelectList(listItem2, "Value", "Text");

            /* 處理有無費用的下拉選單 */
            List<SelectListItem> listItem3 = new List<SelectListItem>();
            listItem3.Add(new SelectListItem { Text = "有", Value = "Y" });
            listItem3.Add(new SelectListItem { Text = "無", Value = "N" });
            ViewData["IsCharged"] = new SelectList(listItem3, "Value", "Text");

            /* 處理日期查詢的下拉選單 */
            List<SelectListItem> listItem4 = new List<SelectListItem>();
            listItem4.Add(new SelectListItem { Text = "申請日", Value = "申請日" });
            listItem4.Add(new SelectListItem { Text = "完工日", Value = "完工日" });
            listItem4.Add(new SelectListItem { Text = "結案日", Value = "結案日" });
            ViewData["DateType"] = new SelectList(listItem4, "Value", "Text", "申請日");

            /* 處理工程師查詢的下拉選單 */
            var engs = roleManager.GetUsersInRole("MedEngineer").ToList();
            List<SelectListItem> listItem5 = new List<SelectListItem>();
            foreach (string l in engs)
            {
                var u = _context.AppUsers.Where(r => r.UserName == l).FirstOrDefault();
                if (u != null)
                {
                    listItem5.Add(new SelectListItem
                    {
                        Text = u.FullName + "(" + u.UserName + ")",
                        Value = u.Id.ToString()
                    });
                }
            }
            ViewData["BMEDEngs"] = new SelectList(listItem5, "Value", "Text");
            /* 擷取該使用者單位底下所有人員 */
            var ur = _userRepo.Find(u => u.UserName == this.User.Identity.Name).FirstOrDefault();
            var dptUsers = _context.AppUsers.Where(a => a.DptId == ur.DptId).ToList();
            List<SelectListItem> dptMemberList = new List<SelectListItem>();
            foreach (var item in dptUsers)
            {
                dptMemberList.Add(new SelectListItem
                {
                    Text = item.FullName,
                    Value = item.Id.ToString()
                });
            }
            ViewData["DptMembers"] = new SelectList(dptMemberList, "Value", "Text");
            QryRepListData data = new QryRepListData();

            return View(data);
        }

        public IActionResult MedTransRdIdx()
        {
            List<SelectListItem> FlowlistItem = new List<SelectListItem>();
            FlowlistItem.Add(new SelectListItem { Text = "送件", Value = "送件" });
            FlowlistItem.Add(new SelectListItem { Text = "取件", Value = "取件" });
            ViewData["STATUS"] = new SelectList(FlowlistItem, "Value", "Text");
            /* 成本中心 & 申請部門的下拉選單資料 */
            var dptList = new[] { "K", "P", "C" };  //本院部門
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

            ViewData["ACCDPT"] = new SelectList(listItem, "Value", "Text");
            ViewData["APPLYDPT"] = new SelectList(listItem, "Value", "Text");

            MedTransRdQModel data = new MedTransRdQModel();

            return View(data);
        }

        /// <summary>
        /// The Index of searching all keep docs.
        /// </summary>
        /// <returns></returns>
        // GET: BMED/Search/KeepIndex
        public IActionResult KeepIndex()
        {
            List<SelectListItem> FlowlistItem = new List<SelectListItem>();
            FlowlistItem.Add(new SelectListItem { Text = "未結案", Value = "未結案" });
            FlowlistItem.Add(new SelectListItem { Text = "已結案", Value = "已結案" });
            ViewData["FLOWTYPE"] = new SelectList(FlowlistItem, "Value", "Text");

            /* 成本中心 & 申請部門的下拉選單資料 */
            var dptList = new[] { "K", "P", "C" };  //本院部門
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

            ViewData["ACCDPT"] = new SelectList(listItem, "Value", "Text");
            ViewData["APPLYDPT"] = new SelectList(listItem, "Value", "Text");

            /* 處理保養狀態的下拉選單 */
            var keepResults = _context.BMEDKeepResults.ToList();
            List<SelectListItem> listItem2 = new List<SelectListItem>();
            foreach (var item in keepResults)
            {
                listItem2.Add(new SelectListItem
                {
                    Text = item.Title,
                    Value = item.Id.ToString()
                });
            }
            ViewData["KeepResults"] = new SelectList(listItem2, "Value", "Text");

            /* 處理有無費用的下拉選單 */
            List<SelectListItem> listItem3 = new List<SelectListItem>();
            listItem3.Add(new SelectListItem { Text = "有", Value = "Y" });
            listItem3.Add(new SelectListItem { Text = "無", Value = "N" });
            ViewData["IsCharged"] = new SelectList(listItem3, "Value", "Text");

            /* 處理日期查詢的下拉選單 */
            List<SelectListItem> listItem4 = new List<SelectListItem>();
            listItem4.Add(new SelectListItem { Text = "送單日", Value = "送單日" });
            listItem4.Add(new SelectListItem { Text = "完工日", Value = "完工日" });
            listItem4.Add(new SelectListItem { Text = "結案日", Value = "結案日" });
            ViewData["DateType"] = new SelectList(listItem4, "Value", "Text", "申請日");

            /* 處理工程師查詢的下拉選單 */
            var engs = roleManager.GetUsersInRole("MedEngineer").ToList();
            List<SelectListItem> listItem5 = new List<SelectListItem>();
            foreach (string l in engs)
            {
                var u = _context.AppUsers.Where(ur => ur.UserName == l).FirstOrDefault();
                if (u != null)
                {
                    listItem5.Add(new SelectListItem
                    {
                        Text = u.FullName + "(" + u.UserName + ")",
                        Value = u.Id.ToString()
                    });
                }
            }
            ViewData["BMEDEngs"] = new SelectList(listItem5, "Value", "Text");

            QryKeepListData data = new QryKeepListData();

            return View(data);
        }

        /// <summary>
        /// The Index of searching all delivery docs.
        /// </summary>
        /// <returns></returns>
        // GET: BMED/Search/DelivIndex
        public IActionResult DelivIndex()
        {
            //
            List<SelectListItem> listItem = new List<SelectListItem>();
            listItem.Add(new SelectListItem { Text = "所有", Value = "所有" });
            listItem.Add(new SelectListItem { Text = "已處理", Value = "已處理" });
            listItem.Add(new SelectListItem { Text = "已結案", Value = "已結案" });
            ViewData["FLOWTYP"] = new SelectList(listItem, "Value", "Text", "所有");
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
            if (userManager.IsInRole(User, "Usual"))
            {
                //listItem2.Clear();
                //AppUserModel u = _context.AppUsers.Find(WebSecurity.CurrentUserId);
                //if (u != null)
                //{
                //    li = new SelectListItem();
                //    li.Text = _context.Departments.Find(u.DptId).Name_C;
                //    li.Value = u.DptId;
                //    listItem2.Add(li);
                //}
            }
            ViewData["APPLYDPT"] = new SelectList(listItem2, "Value", "Text");

            return View();
        }

        [HttpPost]
        public IActionResult GetMedTransRdQList(MedTransRdQModel qdata)
        {
            List<MedTransRd> rv = new List<MedTransRd>();

            return View("MedTransRdQList", rv);
        }
        /// <summary>
        /// Get the query result list of repair docs.
        /// </summary>
        /// <param name="qdata"></param>
        /// <returns></returns>
        // POST: BMED/Search/GetRepQryList
        [HttpPost]
        public IActionResult GetRepQryList(QryRepListData qdata)
        {
            string docid = qdata.BMEDqtyDOCID;
            string ano = qdata.BMEDqtyASSETNO;
            string acc = qdata.BMEDqtyACCDPT;
            string aname = qdata.BMEDqtyASSETNAME;
            string ftype = qdata.BMEDqtyFLOWTYPE;
            string dptid = qdata.BMEDqtyDPTID;
            string qtyDate1 = qdata.BMEDqtyApplyDateFrom;
            string qtyDate2 = qdata.BMEDqtyApplyDateTo;
            string qtyIsCharged = qdata.BMEDqtyIsCharged;
            string qtyDealStatus = qdata.BMEDqtyDealStatus;
            string qtyDateType = qdata.BMEDqtyDateType;
            string qtyEngCode = qdata.BMEDqtyEngCode;
            string qtyTicketNo = qdata.BMEDqtyTicketNo;
            string qtyVendor = qdata.BMEDqtyVendor;
            string qtyTroubledes = qdata.BMEDqtyTROUBLEDES;
            string qtyUserId = qdata.BMEDqtyUserId;
            DateTime applyDateFrom = DateTime.Now;
            DateTime applyDateTo = DateTime.Now;
            /* Dealing search by date. */
            if (qtyDate1 != null && qtyDate2 != null)// If 2 date inputs have been insert, compare 2 dates.
            {
                DateTime date1 = DateTime.Parse(qtyDate1);
                DateTime date2 = DateTime.Parse(qtyDate2);
                int result = DateTime.Compare(date1, date2);
                if (result < 0)
                {
                    applyDateFrom = date1.Date;
                    applyDateTo = date2.Date;
                }
                else if (result == 0)
                {
                    applyDateFrom = date1.Date;
                    applyDateTo = date1.Date;
                }
                else
                {
                    applyDateFrom = date2.Date;
                    applyDateTo = date1.Date;
                }
            }
            else if (qtyDate1 == null && qtyDate2 != null)
            {
                applyDateFrom = DateTime.Parse(qtyDate2);
                applyDateTo = DateTime.Parse(qtyDate2);
            }
            else if (qtyDate1 != null && qtyDate2 == null)
            {
                applyDateFrom = DateTime.Parse(qtyDate1);
                applyDateTo = DateTime.Parse(qtyDate1);
            }


            List<RepairSearchListVModel> rv = new List<RepairSearchListVModel>();

            /* Querying data. */
            var rps = _context.BMEDRepairs.ToList();
            var repairFlows = _context.BMEDRepairFlows.ToList();
            var repairDtls = _context.BMEDRepairDtls.ToList();
            if (!string.IsNullOrEmpty(docid))   //表單編號
            {
                docid = docid.Trim();
                rps = rps.Where(v => v.DocId == docid).ToList();
            }
            if (!string.IsNullOrEmpty(ano))     //財產編號
            {
                rps = rps.Where(v => v.AssetNo == ano).ToList();
            }
            if (!string.IsNullOrEmpty(dptid))   //所屬部門編號
            {
                rps = rps.Where(v => v.DptId == dptid).ToList();
            }
            if (!string.IsNullOrEmpty(acc))     //成本中心
            {
                rps = rps.Where(v => v.AccDpt == acc).ToList();
            }
            if (!string.IsNullOrEmpty(aname))   //財產名稱
            {
                rps = rps.Where(v => v.AssetName != null)
                         .Where(v => v.AssetName.Contains(aname)).ToList();
            }
            if (!string.IsNullOrEmpty(qtyTroubledes))   //故障描述
            {
                rps = rps.Where(v => v.TroubleDes != null)
                         .Where(v => v.TroubleDes.Contains(qtyTroubledes)).ToList();
            }
            if (!string.IsNullOrEmpty(qtyUserId))     //申請人
            {
                int uid = Convert.ToInt32(qtyUserId);
                rps = rps.Where(v => v.UserId == uid).ToList();
            }
            if (!string.IsNullOrEmpty(qtyTicketNo))   //發票號碼
            {
                qtyTicketNo = qtyTicketNo.ToUpper();
                var resultDocIds = _context.BMEDRepairCosts.Include(rc => rc.TicketDtl)
                                                           .Where(rc => rc.TicketDtl.TicketDtlNo == qtyTicketNo)
                                                           .Select(rc => rc.DocId).Distinct();
                rps = (from r in rps
                       where resultDocIds.Any(val => r.DocId.Contains(val))
                       select r).ToList();
            }
            if (!string.IsNullOrEmpty(qtyVendor))   //廠商關鍵字
            {
                var resultDocIds = _context.BMEDRepairCosts.Include(rc => rc.TicketDtl)
                                                           .Where(rc => rc.VendorName.Contains(qtyVendor))
                                                           .Select(rc => rc.DocId).Distinct();
                rps = (from r in rps
                       where resultDocIds.Any(val => r.DocId.Contains(val))
                       select r).ToList();
            }
            if (!string.IsNullOrEmpty(qtyEngCode))     //負責工程師
            {
                rps = rps.Where(v => v.EngId == Convert.ToInt32(qtyEngCode)).ToList();
            }
            if (string.IsNullOrEmpty(qtyDate1) == false || string.IsNullOrEmpty(qtyDate2) == false)  //申請日
            {
                if (qtyDateType == "申請日")
                {
                    rps = rps.Where(v => v.ApplyDate >= applyDateFrom && v.ApplyDate <= applyDateTo).ToList();
                }
            }
            if (!string.IsNullOrEmpty(ftype))   //流程狀態
            {
                switch (ftype)
                {
                    case "未結案":
                        repairFlows = repairFlows.GroupBy(f => f.DocId).Where(group => group.Last().Status == "?")
                                                                       .Select(group => group.Last()).ToList();
                        break;
                    case "已結案":
                        repairFlows = repairFlows.GroupBy(f => f.DocId).Where(group => group.Last().Status == "2")
                                                                       .Select(group => group.Last()).ToList();
                        break;
                }
            }
            else
            {
                repairFlows = repairFlows.GroupBy(f => f.DocId).Where(group => group.Last().Status != "3")
                                                               .Select(group => group.Last()).ToList(); ;
            }
            if (!string.IsNullOrEmpty(qtyDealStatus))   //處理狀態
            {
                repairDtls = repairDtls.Where(r => r.DealState == Convert.ToInt32(qtyDealStatus)).ToList();
            }
            if (!string.IsNullOrEmpty(qtyIsCharged))    //有無費用
            {
                repairDtls = repairDtls.Where(r => r.IsCharged == qtyIsCharged).ToList();
            }

            /* If no search result. */
            if (rps.Count() == 0)
            {
                return View("RepQryList", rv);
            }

            rps.Join(repairFlows, r => r.DocId, f => f.DocId,
                (r, f) => new
                {
                    repair = r,
                    flow = f
                })
                .Join(repairDtls, m => m.repair.DocId, d => d.DocId,
                (m, d) => new
                {
                    repair = m.repair,
                    flow = m.flow,
                    repdtl = d
                })
                .Join(_context.Departments, j => j.repair.AccDpt, d => d.DptId,
                (j, d) => new
                {
                    repair = j.repair,
                    flow = j.flow,
                    repdtl = j.repdtl,
                    dpt = d
                })
                .ToList()
                .ForEach(j => rv.Add(new RepairSearchListVModel
                {
                    DocType = "醫工請修",
                    RepType = j.repair.RepType,
                    DocId = j.repair.DocId,
                    ApplyDate = j.repair.ApplyDate,
                    PlaceLoc = j.repair.PlaceLoc,
                    ApplyDpt = j.repair.DptId,
                    AccDpt = j.repair.AccDpt,
                    AccDptName = j.dpt.Name_C,
                    TroubleDes = j.repair.TroubleDes,
                    DealState = _context.BMEDDealStatuses.Find(j.repdtl.DealState).Title,
                    DealDes = j.repdtl.DealDes,
                    Cost = j.repdtl.Cost,
                    Days = DateTime.Now.Subtract(j.repair.ApplyDate).Days,
                    Flg = j.flow.Status,
                    FlowUid = j.flow.UserId,
                    FlowCls = j.flow.Cls,
                    FlowUidName = _context.AppUsers.Find(j.flow.UserId).FullName,
                    EndDate = j.repdtl.EndDate,
                    CloseDate = j.repdtl.CloseDate,
                    IsCharged = j.repdtl.IsCharged,
                    repdata = j.repair
                }));

            /* 設備編號"有"、"無"的對應，"有"讀取table相關data，"無"只顯示申請人輸入的設備名稱 */
            foreach (var item in rv)
            {
                var repairDoc = _context.BMEDRepairs.Find(item.DocId);
                if (repairDoc.AssetNo != null)
                {
                    var asset = _context.BMEDAssets.Where(a => a.AssetNo == repairDoc.AssetNo).FirstOrDefault();
                    if (asset != null)
                    {
                        item.AssetNo = asset.AssetNo;
                        item.AssetName = asset.Cname;
                        item.Brand = asset.Brand;
                        item.Type = asset.Type;
                    }
                }
                else
                {
                    item.AssetName = repairDoc.AssetName;
                }
            }

            /* Search date by DateType. */
            if (string.IsNullOrEmpty(qtyDate1) == false || string.IsNullOrEmpty(qtyDate2) == false)
            {
                if (qtyDateType == "結案日")
                {
                    rv = rv.Where(v => v.CloseDate >= applyDateFrom && v.CloseDate <= applyDateTo).ToList();
                }
                else if (qtyDateType == "完工日")
                {
                    rv = rv.Where(v => v.EndDate >= applyDateFrom && v.EndDate <= applyDateTo).ToList();
                }
            }

            /* Sorting search result. */
            if (rv.Count() != 0)
            {
                if (qtyDateType == "結案日")
                {
                    rv = rv.OrderByDescending(r => r.CloseDate).ThenByDescending(r => r.DocId).ToList();
                }
                else if (qtyDateType == "完工日")
                {
                    rv = rv.OrderByDescending(r => r.EndDate).ThenByDescending(r => r.DocId).ToList();
                }
                else
                {
                    rv = rv.OrderByDescending(r => r.ApplyDate).ThenByDescending(r => r.DocId).ToList();
                }
            }

            return View("RepQryList", rv);
        }

        /// <summary>
        /// Get the query result list of keep docs.
        /// </summary>
        /// <param name="qdata"></param>
        /// <returns></returns>
        // POST: BMED/Search/GetKeepQryList
        [HttpPost]
        public IActionResult GetKeepQryList(QryKeepListData qdata)
        {
            string docid = qdata.BMEDKqtyDOCID;
            string ano = qdata.BMEDKqtyASSETNO;
            string acc = qdata.BMEDKqtyACCDPT;
            string aname = qdata.BMEDKqtyASSETNAME;
            string ftype = qdata.BMEDKqtyFLOWTYPE;
            string dptid = qdata.BMEDKqtyDPTID;
            string qtyDate1 = qdata.BMEDKqtyApplyDateFrom;
            string qtyDate2 = qdata.BMEDKqtyApplyDateTo;
            string qtyKeepResult = qdata.BMEDKqtyKeepResult;
            string qtyIsCharged = qdata.BMEDKqtyIsCharged;
            string qtyDateType = qdata.BMEDKqtyDateType;
            bool searchAllDoc = qdata.BMEDKqtySearchAllDoc;
            string qtyEngCode = qdata.BMEDKqtyEngCode;
            string qtyTicketNo = qdata.BMEDKqtyTicketNo;
            string qtyVendor = qdata.BMEDKqtyVendor;

            DateTime applyDateFrom = DateTime.Now;
            DateTime applyDateTo = DateTime.Now;
            /* Dealing search by date. */
            if (qtyDate1 != null && qtyDate2 != null)// If 2 date inputs have been insert, compare 2 dates.
            {
                DateTime date1 = DateTime.Parse(qtyDate1);
                DateTime date2 = DateTime.Parse(qtyDate2);
                int result = DateTime.Compare(date1, date2);
                if (result < 0)
                {
                    applyDateFrom = date1.Date;
                    applyDateTo = date2.Date;
                }
                else if (result == 0)
                {
                    applyDateFrom = date1.Date;
                    applyDateTo = date1.Date;
                }
                else
                {
                    applyDateFrom = date2.Date;
                    applyDateTo = date1.Date;
                }
            }
            else if (qtyDate1 == null && qtyDate2 != null)
            {
                applyDateFrom = DateTime.Parse(qtyDate2);
                applyDateTo = DateTime.Parse(qtyDate2);
            }
            else if (qtyDate1 != null && qtyDate2 == null)
            {
                applyDateFrom = DateTime.Parse(qtyDate1);
                applyDateTo = DateTime.Parse(qtyDate1);
            }


            List<KeepSearchListViewModel> kv = new List<KeepSearchListViewModel>();

            /* Querying data. */
            var kps = _context.BMEDKeeps.ToList();
            var keepFlows = _context.BMEDKeepFlows.ToList();
            var keepDtls = _context.BMEDKeepDtls.ToList();
            if (!string.IsNullOrEmpty(docid))   //表單編號
            {
                docid = docid.Trim();
                kps = kps.Where(v => v.DocId == docid).ToList();
            }
            if (!string.IsNullOrEmpty(ano))     //財產編號
            {
                kps = kps.Where(v => v.AssetNo == ano).ToList();
            }
            if (!string.IsNullOrEmpty(dptid))   //所屬部門編號
            {
                kps = kps.Where(v => v.DptId == dptid).ToList();
            }
            if (!string.IsNullOrEmpty(acc))     //成本中心
            {
                kps = kps.Where(v => v.AccDpt == acc).ToList();
            }
            if (!string.IsNullOrEmpty(aname))   //財產名稱
            {
                kps = kps.Where(v => v.AssetName != null)
                         .Where(v => v.AssetName.Contains(aname))
                         .ToList();
            }
            if (!string.IsNullOrEmpty(qtyTicketNo))   //發票號碼
            {
                qtyTicketNo = qtyTicketNo.ToUpper();
                var resultDocIds = _context.BMEDKeepCosts.Include(kc => kc.TicketDtl)
                                                         .Where(kc => kc.TicketDtl.TicketDtlNo == qtyTicketNo)
                                                         .Select(kc => kc.DocId).Distinct();
                kps = (from k in kps
                       where resultDocIds.Any(val => k.DocId.Contains(val))
                       select k).ToList();
            }
            if (!string.IsNullOrEmpty(qtyVendor))   //廠商關鍵字
            {
                var resultDocIds = _context.BMEDKeepCosts.Include(kc => kc.TicketDtl)
                                                         .Where(kc => kc.VendorName.Contains(qtyVendor))
                                                         .Select(kc => kc.DocId).Distinct();
                kps = (from k in kps
                       where resultDocIds.Any(val => k.DocId.Contains(val))
                       select k).ToList();
            }
            if (!string.IsNullOrEmpty(qtyEngCode))     //負責工程師
            {
                kps = kps.Where(v => v.EngId == Convert.ToInt32(qtyEngCode)).ToList();
            }
            /* Search date by DateType.(ApplyDate) */
            if (string.IsNullOrEmpty(qtyDate1) == false || string.IsNullOrEmpty(qtyDate2) == false) //送單日
            {
                if (qtyDateType == "送單日")
                {
                    kps = kps.Where(v => v.SentDate >= applyDateFrom && v.SentDate <= applyDateTo).ToList();
                }
            }

            if (!string.IsNullOrEmpty(ftype))   //流程狀態
            {
                switch (ftype)
                {
                    case "未結案":
                        keepFlows = keepFlows.GroupBy(f => f.DocId).Where(group => group.Last().Status == "?")
                                                                   .Select(group => group.Last()).ToList();
                        break;
                    case "已結案":
                        keepFlows = keepFlows.GroupBy(f => f.DocId).Where(group => group.Last().Status == "2")
                                                                   .Select(group => group.Last()).ToList();
                        break;
                }
            }
            else
            {
                keepFlows = keepFlows.GroupBy(f => f.DocId).Where(group => group.Last().Status != "3")
                                                           .Select(group => group.Last()).ToList(); ;
            }

            /* If no search result. */
            if (kps.Count() == 0)
            {
                return View("KeepQryList", kv);
            }

            kps.Join(keepFlows, k => k.DocId, f => f.DocId,
                (k, f) => new
                {
                    keep = k,
                    flow = f
                })
                .Join(_context.BMEDAssets, k => k.keep.AssetNo, a => a.AssetNo,
                (k, a) => new
                {
                    keep = k.keep,
                    asset = a,
                    flow = k.flow
                })
                .Join(_context.BMEDKeepDtls, m => m.keep.DocId, d => d.DocId,
                (m, d) => new
                {
                    keep = m.keep,
                    flow = m.flow,
                    asset = m.asset,
                    keepdtl = d
                })
                .Join(_context.Departments, j => j.keep.AccDpt, d => d.DptId,
                (j, d) => new
                {
                    keep = j.keep,
                    flow = j.flow,
                    asset = j.asset,
                    keepdtl = j.keepdtl,
                    dpt = d
                }).ToList()
                .ForEach(j => kv.Add(new KeepSearchListViewModel
                {
                    DocType = "醫工保養",
                    DocId = j.keep.DocId,
                    AssetNo = j.keep.AssetNo,
                    AssetName = j.keep.AssetName,
                    Brand = j.asset.Brand,
                    Type = j.asset.Type,
                    PlaceLoc = j.keep.PlaceLoc,
                    ApplyDpt = j.keep.DptId,
                    AccDpt = j.keep.AccDpt,
                    AccDptName = j.dpt.Name_C,
                    Result = (j.keepdtl.Result == null || j.keepdtl.Result == 0) ? "" : _context.BMEDKeepResults.Find(j.keepdtl.Result).Title,
                    InOut = j.keepdtl.InOut == "0" ? "自行" :
                    j.keepdtl.InOut == "1" ? "委外" :
                    j.keepdtl.InOut == "2" ? "租賃" :
                    j.keepdtl.InOut == "3" ? "保固" : "",
                    Memo = j.keepdtl.Memo,
                    Cost = j.keepdtl.Cost,
                    Days = DateTime.Now.Subtract(j.keep.SentDate.GetValueOrDefault()).Days,
                    Flg = j.flow.Status,
                    FlowUid = j.flow.UserId,
                    FlowCls = j.flow.Cls,
                    FlowUidName = _context.AppUsers.Find(j.flow.UserId).FullName,
                    Src = j.keep.Src,
                    SentDate = j.keep.SentDate,
                    EndDate = j.keepdtl.EndDate,
                    CloseDate = j.keepdtl.CloseDate,
                    IsCharged = j.keepdtl.IsCharged,
                    keepdata = j.keep
                }));

            /* Search date by DateType. */
            if (string.IsNullOrEmpty(qtyDate1) == false || string.IsNullOrEmpty(qtyDate2) == false)
            {
                if (qtyDateType == "結案日")
                {
                    kv = kv.Where(v => v.CloseDate >= applyDateFrom && v.CloseDate <= applyDateTo).ToList();
                }
                else if (qtyDateType == "完工日")
                {
                    kv = kv.Where(v => v.EndDate >= applyDateFrom && v.EndDate <= applyDateTo).ToList();
                }
            }

            /* Sorting search result. */
            if (kv.Count() != 0)
            {
                if (qtyDateType == "結案日")
                {
                    kv = kv.OrderByDescending(r => r.CloseDate).ThenByDescending(r => r.DocId).ToList();
                }
                else if (qtyDateType == "完工日")
                {
                    kv = kv.OrderByDescending(r => r.EndDate).ThenByDescending(r => r.DocId).ToList();
                }
                else
                {
                    kv = kv.OrderByDescending(r => r.SentDate).ThenByDescending(r => r.DocId).ToList();
                }
            }

            /* Search KeepResults. */
            if (!string.IsNullOrEmpty(qtyKeepResult))
            {
                kv = kv.Where(r => r.Result == _context.BMEDKeepResults.Find(Convert.ToInt32(qtyKeepResult)).Title).ToList();
            }
            /* Search IsCharged. */
            if (!string.IsNullOrEmpty(qtyIsCharged))
            {
                kv = kv.Where(r => r.IsCharged == qtyIsCharged).ToList();
            }

            return View("KeepQryList", kv);
        }

        /// <summary>
        /// Get the query result list of delivery docs.
        /// </summary>
        /// <param name="qdata"></param>
        /// <returns></returns>
        // POST: BMED/Search/GetDelivQryList
        [HttpPost]
        public IActionResult GetDelivQryList(IFormCollection form)
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            listItem.Add(new SelectListItem { Text = "待處理", Value = "待處理" });
            listItem.Add(new SelectListItem { Text = "已處理", Value = "已處理" });
            listItem.Add(new SelectListItem { Text = "已結案", Value = "已結案" });
            ViewData["FLOWTYP"] = new SelectList(listItem, "Value", "Text", "待處理");
            //
            List<DeliveryListVModel> vm;
            vm = GetDeliveryList(form["qtyFLOWTYP"]);
            if (!string.IsNullOrEmpty(form["qtyDOCID"]))
            {
                vm = vm.Where(m => m.DocId == form["qtyDOCID"]).ToList();
            }
            if (!string.IsNullOrEmpty(form["qtyPURCHASENO"]))
            {
                vm = vm.Where(m => m.PurchaseNo == form["qtyPURCHASENO"]).ToList();
            }
            if (!string.IsNullOrEmpty(form["qtyDPTID"]))
            {
                vm = vm.Where(m => m.Company == form["qtyDPTID"]).ToList();
            }
            if (!string.IsNullOrEmpty(form["qtyACCDPT"]))
            {
                vm = vm.Where(m => m.AccDpt == form["qtyACCDPT"]).ToList();
            }
            if (!string.IsNullOrEmpty(form["qtyBUDGETID"]))
            {
                vm = vm.Where(m => m.BudgetId == form["qtyBUDGETID"]).ToList();
            }
            if (!string.IsNullOrEmpty(form["qtyCONTRACTNO"]))
            {
                vm = vm.Where(m => m.ContractNo == form["qtyCONTRACTNO"]).ToList();
            }
            if (!string.IsNullOrEmpty(form["qtyASSETNO"]))
            {
                AssetModel at = _context.BMEDAssets.Find(form["qtyASSETNO"]);
                if (at != null)
                {
                    vm = vm.Where(m => m.DocId == at.Docid).ToList();
                }
                else
                    vm.Clear();
            }
            if (form["qtyVENDOR"] != "")
            {
                string s = form["qtyVENDOR"];
                List<VendorModel> vs = _context.BMEDVendors.Where(a => a.VendorName.Contains(s))
                    .ToList();
                if (vs != null)
                {
                    vm = vm.Join(vs, v => Convert.ToInt32(v.VendorNo), a => a.VendorId,
                        (v, a) => v).ToList();
                }
                else
                    vm.Clear();
            }
            foreach (var item in vm)
            {
                var u = _context.AppUsers.Find(item.UserId);
                item.Contact = u == null ? "" : u.Ext;
                u = _context.AppUsers.Find(item.FlowUid);
                item.FlowUname = u == null ? "" : u.FullName;
                item.CompanyNam = _context.Departments.Find(item.Company) == null ? "" : _context.Departments.Find(item.Company).Name_C;
            }
            vm = vm.OrderByDescending(v => v.ApplyDate).ToList();
            return PartialView("DelivQryList", vm);
        }

        public List<DeliveryListVModel> GetDeliveryList(string cls = null)
        {
            List<DeliveryListVModel> dv = new List<DeliveryListVModel>();
            List<DelivFlowModel> rf = new List<DelivFlowModel>();
            List<DelivFlowModel> rf2;
            // Get Login User's details.
            var ur = _userRepo.Find(usr => usr.UserName == User.Identity.Name).FirstOrDefault();
            switch (cls)
            {
                case "已處理":
                    rf2 = _context.DelivFlows.Where(df => df.Status == "?")
                                             .Where(m => m.UserId != ur.Id).ToList();
                    if (!userManager.IsInRole(User, "Usual"))
                    {
                        rf.AddRange(rf2);
                    }
                    else
                    {
                        foreach (DelivFlowModel f in rf2)
                        {
                            if (_context.DelivFlows.Where(m => m.DocId == f.DocId).Where(m => m.UserId == ur.Id).Count() > 0)
                            {
                                rf.Add(f);
                            }
                        }
                    }

                    break;
                case "已結案":
                    rf2 = _context.DelivFlows.Where(df => df.Status == "2").ToList();
                    if (!userManager.IsInRole(User, "Usual"))
                    {
                        rf.AddRange(rf2);
                    }
                    else
                    {
                        foreach (DelivFlowModel f in rf2)
                        {
                            if (_context.DelivFlows.Where(m => m.DocId == f.DocId).Where(m => m.UserId == ur.Id).Count() > 0)
                            {
                                rf.Add(f);
                            }
                        }
                    }
                    break;
                case "所有":
                    rf2 = _context.DelivFlows.Where(df => df.Status == "2" || df.Status == "?").ToList();
                    if (!userManager.IsInRole(User, "Usual"))
                    {
                        rf.AddRange(rf2);
                    }
                    else
                    {
                        foreach (DelivFlowModel f in rf2)
                        {
                            if (_context.DelivFlows.Where(m => m.DocId == f.DocId).Where(m => m.UserId == ur.Id).Count() > 0)
                            {
                                rf.Add(f);
                            }
                        }
                    }
                    break;
                case "查詢":
                    rf2 = _context.DelivFlows.Where(df => df.Status == "?").ToList();
                    DeliveryModel r;
                    foreach (DelivFlowModel f in rf2)
                    {
                        r = _context.Deliveries.Find(f.DocId);
                        rf.Add(f);
                    }
                    break;
                default:
                    rf = _context.DelivFlows.Where(df => df.Status == "?")
                                            .Where(m => m.UserId == ur.Id).ToList();
                    break;
            }
            rf.OrderByDescending(m => m.Rtt);
            DeliveryListVModel i;
            foreach (DelivFlowModel f in rf)
            {
                DeliveryModel r = _context.Deliveries.Find(f.DocId);
                AppUserModel p = _context.AppUsers.Find(r.UserId);
                DepartmentModel c = _context.Departments.Find(p.DptId);
                //BuyEvaluate b = db.BuyEvaluates.Find(r.PurchaseNo);
                i = new DeliveryListVModel();
                i.DocType = "驗收";
                i.DocId = r.DocId;
                i.UserId = r.UserId;
                i.UserName = r.UserName;
                if (p != null && p.DptId != null)
                {
                    i.Company = p.DptId;
                    i.CompanyNam = c == null ? "" : c.Name_C;
                }
                i.ContractNo = r.ContractNo;
                i.PurchaseNo = r.PurchaseNo;
                i.CrlItemNo = r.CrlItemNo;
                i.AccDpt = r.AccDpt;
                i.AccDptNam = _context.Departments.Find(r.AccDpt) == null ? "" : _context.Departments.Find(r.AccDpt).Name_C;
                i.BudgetId = "";
                if (f.Status == "?")
                    i.Days = DateTime.Now.Subtract(r.ApplyDate.GetValueOrDefault()).Days;
                else
                    i.Days = null;
                i.Flg = f.Status;
                i.FlowUid = f.UserId;
                i.VendorNo = r.VendorId;
                i.ApplyDate = r.ApplyDate;
                dv.Add(i);
            }
            //
            return dv;
        }

    }
}