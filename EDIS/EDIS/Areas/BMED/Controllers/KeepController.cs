﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDIS.Areas.BMED.Data;
using EDIS.Areas.BMED.Models.KeepModels;
using EDIS.Areas.BMED.Models.RepairModels;
using EDIS.Models;
using EDIS.Models.Identity;
using EDIS.Repositories;
using EDIS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EDIS.Areas.BMED.Controllers
{
    [Area("BMED")]
    [Authorize]
    public class KeepController : Controller
    {
        private readonly BMEDDbContext _context;
        private readonly IRepository<AppUserModel, int> _userRepo;
        private readonly IRepository<DepartmentModel, string> _dptRepo;
        private readonly IRepository<DocIdStore, string[]> _dsRepo;
        private readonly IEmailSender _emailSender;
        private readonly CustomUserManager userManager;
        private readonly CustomRoleManager roleManager;

        public KeepController(BMEDDbContext context,
                              IRepository<AppUserModel, int> userRepo,
                              IRepository<DepartmentModel, string> dptRepo,
                              IRepository<DocIdStore, string[]> dsRepo,
                              IEmailSender emailSender,
                              CustomUserManager customUserManager,
                              CustomRoleManager customRoleManager)
        {
            _context = context;
            _userRepo = userRepo;
            _dptRepo = dptRepo;
            _dsRepo = dsRepo;
            _emailSender = emailSender;
            userManager = customUserManager;
            roleManager = customRoleManager;
        }

        // GET: BMED/Keep/Create
        public IActionResult Create()
        {
            KeepModel r = new KeepModel();
            AppUserModel ur = _context.AppUsers.Where(u => u.UserName == this.User.Identity.Name).FirstOrDefault();

            r.Email = ur.Email == null ? "" : ur.Email;
            DepartmentModel d = _context.Departments.Find(ur.DptId);
            r.DocId = GetID();
            r.UserId = ur.Id;
            r.UserName = ur.FullName;
            r.SentDate = DateTime.Now;
            r.DptId = d == null ? "" : d.DptId;
            r.Company = d == null ? "" : d.Name_C;
            r.AccDpt = d == null ? "" : d.DptId;
            r.AccDptName = d == null ? "" : d.Name_C;
            r.Contact = ur.Ext ?? ur.Mobile ?? "";
            //
            _context.BMEDKeeps.Add(r);
            _context.SaveChanges();

            List<SelectListItem> listItem = new List<SelectListItem>();
            List<SelectListItem> AccDpt = new List<SelectListItem>();
            _context.Departments.ToList()
                .ForEach(dp =>
                {
                    AccDpt.Add(new SelectListItem
                    {
                        Value = dp.DptId,
                        Text = dp.Name_C,
                        Selected = false
                    });
                });
            ViewData["AccDpt"] = AccDpt;
            //
            List<AssetModel> alist = null;
            if (ur.DptId != null)
                alist = _context.BMEDAssets.Where(at => at.AccDpt == ur.DptId)
                                           .Where(at => at.DisposeKind != "報廢").ToList();
            else if (ur.VendorId > 0)
            {
                string s = Convert.ToString(ur.VendorId);
                alist = _context.BMEDAssets.Where(at => at.AccDpt == s)
                                           .Where(at => at.DisposeKind != "報廢").ToList();
            }

            return View(r);
        }

        // POST: BMED/Keep/Create
        [HttpPost]
        public ActionResult Create(KeepModel keep)
        {
            AppUserModel ur = _context.AppUsers.Where(u => u.UserName == this.User.Identity.Name).FirstOrDefault();

            if (string.IsNullOrEmpty(keep.AssetNo))
            {
                throw new Exception("財產編號不可空白!!");
            }
            string msg = "";
            try
            {
                if (ModelState.IsValid)
                {

                    //更新申請人的Email
                    if (string.IsNullOrEmpty(keep.Email))
                    {
                        throw new Exception("電子信箱不可空白!!");
                    }
                    AppUserModel a = _context.AppUsers.Find(keep.UserId);
                    a.Email = keep.Email;
                    _context.Entry(a).State = EntityState.Modified;
                    _context.SaveChanges();
                    //
                    AssetKeepModel kp = _context.BMEDAssetKeeps.Find(keep.AssetNo);
                    AssetModel at = _context.BMEDAssets.Find(keep.AssetNo);
                    //
                    keep.AssetName = _context.BMEDAssets.Find(keep.AssetNo).Cname;
                    //keep.AccDpt = at.AccDpt;
                    keep.SentDate = DateTime.Now;
                    keep.Cycle = kp == null ? 0 : kp.Cycle.Value;
                    keep.Src = "M";
                    _context.Entry(keep).State = EntityState.Modified;

                    //
                    KeepDtlModel dl = new KeepDtlModel();
                    dl.DocId = keep.DocId;
                    switch (kp == null ? "自行" : kp.InOut)
                    {
                        case "自行":
                            dl.InOut = "0";
                            break;
                        case "委外":
                            dl.InOut = "1";
                            break;
                        case "租賃":
                            dl.InOut = "2";
                            break;
                        case "保固":
                            dl.InOut = "3";
                            break;
                        default:
                            dl.InOut = "0";
                            break;
                    }
                    _context.BMEDKeepDtls.Add(dl);
                    _context.SaveChanges();
                    //
                    KeepFlowModel rf = new KeepFlowModel();
                    rf.DocId = keep.DocId;
                    rf.StepId = 1;
                    rf.UserId = ur.Id;
                    rf.Status = "1";
                    //rf.Role = Roles.GetRolesForUser().FirstOrDefault();
                    rf.Rtp = ur.Id;
                    rf.Rdt = null;
                    rf.Rtt = DateTime.Now;
                    rf.Cls = "申請者";
                    _context.BMEDKeepFlows.Add(rf);
                    //
                    rf = new KeepFlowModel();
                    rf.DocId = keep.DocId;
                    rf.StepId = 2;
                    rf.UserId = kp == null ? ur.Id : kp.KeepEngId;
                    rf.Status = "?";
                    AppUserModel u = _context.AppUsers.Find(rf.UserId);
                    if (u == null)
                    {
                        throw new Exception("無工程師資料!!");
                    }
                    //rf.Role = Roles.GetRolesForUser(u.UserName).FirstOrDefault();
                    rf.Rtp = null;
                    rf.Rdt = null;
                    rf.Rtt = DateTime.Now;
                    rf.Cls = "設備工程師";
                    _context.BMEDKeepFlows.Add(rf);
                    _context.SaveChanges();
                    //send mail
                    Tmail mail = new Tmail();
                    string body = "";
                    u = _context.AppUsers.Find(ur.Id);
                    mail.from = new System.Net.Mail.MailAddress(u.Email); //u.Email
                    u = _context.AppUsers.Find(kp.KeepEngId);
                    mail.to = new System.Net.Mail.MailAddress(u.Email); //u.Email
                    mail.message.Subject = "醫療儀器管理資訊系統[保養案]：儀器名稱： " + keep.AssetName;
                    body += "<p>申請人：" + keep.UserName + "</p>";
                    body += "<p>財產編號：" + keep.AssetNo + "</p>";
                    body += "<p>儀器名稱：" + keep.AssetName + "</p>";
                    body += "<p>放置地點：" + keep.PlaceLoc + "</p>";
                    //body += "<p>故障描述：" + repair.TroubleDes + "</p>";
                    //body += "<p>放置地點：" + repair.PlaceLoc + "</p>";
                    body += "<p><a href=''>處理案件</a></p>";
                    body += "<br/>";
                    body += "<h3>此封信件為系統通知郵件，請勿回覆。</h3>";
                    mail.message.Body = body;
                    mail.message.IsBodyHtml = true;
                    //mail.SendMail();

                    return Ok(keep);
                }
                else
                {
                    msg = "";
                    foreach (var error in ViewData.ModelState.Values.SelectMany(modelState => modelState.Errors))
                    {
                        msg += error.ErrorMessage + Environment.NewLine;
                    }
                    throw new Exception(msg);
                }
            }
            catch(Exception ex)
            {
                msg = ex.Message;
            }
            return BadRequest(msg);
        }

        public string GetID()
        {
            string s = _context.BMEDKeeps.Select(r => r.DocId).Max();
            string did = "";
            int yymm = (System.DateTime.Now.Year - 1911) * 100 + System.DateTime.Now.Month;
            if (!string.IsNullOrEmpty(s))
            {
                did = s;
            }
            if (did != "")
            {
                if (Convert.ToInt64(did) / 10000 == yymm)
                    did = Convert.ToString(Convert.ToInt64(did) + 1);
                else
                    did = Convert.ToString(yymm * 10000 + 1);
            }
            else
            {
                did = Convert.ToString(yymm * 10000 + 1);
            }
            return did;
        }

        [HttpPost]
        public IActionResult Index(QryRepListData qdata)
        {
            string docid = qdata.BMEDqtyDOCID;
            string ano = qdata.BMEDqtyASSETNO;
            string acc = qdata.BMEDqtyACCDPT;
            string aname = qdata.BMEDqtyASSETNAME;
            string ftype = qdata.BMEDqtyFLOWTYPE;
            string dptid = qdata.BMEDqtyDPTID;
            string qtyDate1 = qdata.BMEDqtyApplyDateFrom;
            string qtyDate2 = qdata.BMEDqtyApplyDateTo;
            //string qtyDealStatus = qdata.BMEDqtyDealStatus;
            //string qtyIsCharged = qdata.BMEDqtyIsCharged;
            string qtyDateType = qdata.BMEDqtyDateType;
            bool searchAllDoc = qdata.BMEDqtySearchAllDoc;

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


            List<KeepListVModel> kv = new List<KeepListVModel>();
            /* Get login user. */
            var ur = _userRepo.Find(u => u.UserName == this.User.Identity.Name).FirstOrDefault();

            var rps = _context.BMEDKeeps.ToList();
            if (!string.IsNullOrEmpty(docid))
            {
                docid = docid.Trim();
                rps = rps.Where(v => v.DocId == docid).ToList();
            }
            if (!string.IsNullOrEmpty(ano))
            {
                rps = rps.Where(v => v.AssetNo == ano).ToList();
            }
            if (!string.IsNullOrEmpty(dptid))
            {
                rps = rps.Where(v => v.DptId == dptid).ToList();
            }
            if (!string.IsNullOrEmpty(acc))
            {
                rps = rps.Where(v => v.AccDpt == acc).ToList();
            }
            if (!string.IsNullOrEmpty(aname))
            {
                rps = rps.Where(v => v.AssetName != null)
                         .Where(v => v.AssetName.Contains(aname))
                         .ToList();
            }
            /* Search date by DateType.(ApplyDate) */
            if (string.IsNullOrEmpty(qtyDate1) == false || string.IsNullOrEmpty(qtyDate2) == false)
            {
                if (qtyDateType == "送單日")
                {
                    rps = rps.Where(v => v.SentDate >= applyDateFrom && v.SentDate <= applyDateTo).ToList();
                }
            }

            /* If no search result. */
            if (rps.Count() == 0)
            {
                return View("List", kv);
            }

            switch (ftype)
            {
                /* 與登入者相關且流程不在該登入者身上的文件 */
                case "流程中":
                    rps.Join(_context.BMEDKeepFlows.Where(f2 => f2.UserId == ur.Id && f2.Status == "1")
                       .Select(f => f.DocId).Distinct(),
                               r => r.DocId, f2 => f2, (r, f2) => r)
                       .Join(_context.BMEDKeepFlows.Where(f => f.Status == "?" && f.UserId != ur.Id),
                        r => r.DocId, f => f.DocId,
                        (r, f) => new
                        {
                            keep = r,
                            flow = f
                        }).Join(db.Assets, r => r.keep.AssetNo, a => a.AssetNo,
                (r, a) => new
                {
                    keep = r.keep,
                    asset = a,
                    flow = r.flow
                })
                .Join(db.KeepDtls, m => m.keep.DocId, d => d.DocId,
                (m, d) => new
                {
                    keep = m.keep,
                    flow = m.flow,
                    asset = m.asset,
                    keepdtl = d
                })
                .Join(db.Departments, j => j.keep.AccDpt, d => d.DptId,
                    (j, d) => new
                    {
                        keep = j.keep,
                        flow = j.flow,
                        asset = j.asset,
                        keepdtl = j.keepdtl,
                        dpt = d
                    }).ToList()
                    .ForEach(j => kv.Add(new KeepListVModel
                    {
                        DocType = "保養",
                        DocId = j.keep.DocId,
                        AssetNo = j.keep.AssetNo,
                        AssetName = j.keep.AssetName,
                        Brand = j.asset.Brand,
                        Type = j.asset.Type,
                        PlaceLoc = j.keep.PlaceLoc,
                        ApplyDpt = j.keep.DptId,
                        AccDpt = j.keep.AccDpt,
                        AccDptName = j.dpt.Name_C,
                        Result = (j.keepdtl.Result == null || j.keepdtl.Result == 0) ? "" : db.KeepResults.Find(j.keepdtl.Result).Title,
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
                        Src = j.keep.Src,
                        SentDate = j.keep.SentDate
                    }));
                    break;
                case "已結案":
                    List<KeepFlow> kf = db.KeepFlows.Where(f => f.Status == "2").ToList();
                    if (Roles.IsUserInRole("Admin") || Roles.IsUserInRole("Manager"))
                    {
                        if (Roles.IsUserInRole("Manager"))
                        {
                            kf = kf.Join(db.Keeps.Where(r => r.AccDpt == usr.DptId),
                                f => f.DocId, r => r.DocId, (f, r) => f).ToList();
                        }
                    }
                    else
                    {
                        kf = kf.Join(db.KeepFlows.Where(f2 => f2.UserId == WebSecurity.CurrentUserId),
                             f => f.DocId, f2 => f2.DocId, (f, f2) => f).ToList();
                    }
                    //
                    kf.Select(f => new
                    {
                        f.DocId,
                        f.UserId,
                        f.Cls,
                        f.Status
                    }).Distinct()
                .Join(rps.DefaultIfEmpty(), f => f.DocId, k => k.DocId,
                (f, k) => new
                {
                    keep = k,
                    flow = f
                }).Join(db.Assets, r => r.keep.AssetNo, a => a.AssetNo,
                (r, a) => new
                {
                    keep = r.keep,
                    asset = a,
                    flow = r.flow
                })
                .Join(db.KeepDtls, m => m.keep.DocId, d => d.DocId,
                (m, d) => new
                {
                    keep = m.keep,
                    flow = m.flow,
                    asset = m.asset,
                    keepdtl = d
                })
                .Join(db.Departments, j => j.keep.AccDpt, d => d.DptId,
                    (j, d) => new
                    {
                        keep = j.keep,
                        flow = j.flow,
                        asset = j.asset,
                        keepdtl = j.keepdtl,
                        dpt = d
                    }).ToList()
                    .ForEach(j => kv.Add(new KeepListVModel
                    {
                        DocType = "保養",
                        DocId = j.keep.DocId,
                        AssetNo = j.keep.AssetNo,
                        AssetName = j.keep.AssetName,
                        Brand = j.asset.Brand,
                        Type = j.asset.Type,
                        PlaceLoc = j.keep.PlaceLoc,
                        ApplyDpt = j.keep.DptId,
                        AccDpt = j.keep.AccDpt,
                        AccDptName = j.dpt.Name_C,
                        Result = (j.keepdtl.Result == null || j.keepdtl.Result == 0) ? "" : db.KeepResults.Find(j.keepdtl.Result).Title,
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
                        Src = j.keep.Src,
                        SentDate = j.keep.SentDate
                    }));
                    break;
                case "待簽核":
                    rps.Join(db.KeepFlows.Where(f => f.Status == "?" && f.UserId == WebSecurity.CurrentUserId),
                r => r.DocId, f => f.DocId,
                (r, f) => new
                {
                    keep = r,
                    flow = f
                }).Join(db.Assets, r => r.keep.AssetNo, a => a.AssetNo,
                (r, a) => new
                {
                    keep = r.keep,
                    asset = a,
                    flow = r.flow
                })
                .Join(db.KeepDtls, m => m.keep.DocId, d => d.DocId,
                (m, d) => new
                {
                    keep = m.keep,
                    flow = m.flow,
                    asset = m.asset,
                    keepdtl = d
                })
                .Join(db.Departments, j => j.keep.AccDpt, d => d.DptId,
                    (j, d) => new
                    {
                        keep = j.keep,
                        flow = j.flow,
                        asset = j.asset,
                        keepdtl = j.keepdtl,
                        dpt = d
                    }).ToList()
                    .ForEach(j => kv.Add(new KeepListVModel
                    {
                        DocType = "保養",
                        DocId = j.keep.DocId,
                        AssetNo = j.keep.AssetNo,
                        AssetName = j.keep.AssetName,
                        Brand = j.asset.Brand,
                        Type = j.asset.Type,
                        PlaceLoc = j.keep.PlaceLoc,
                        ApplyDpt = j.keep.DptId,
                        AccDpt = j.keep.AccDpt,
                        AccDptName = j.dpt.Name_C,
                        Result = (j.keepdtl.Result == null || j.keepdtl.Result == 0) ? "" : db.KeepResults.Find(j.keepdtl.Result).Title,
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
                        Src = j.keep.Src,
                        SentDate = j.keep.SentDate
                    }));
                    break;
            };

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

            /* Search dealStatus. */
            //if (!string.IsNullOrEmpty(qtyDealStatus))
            //{
            //    rv = rv.Where(r => r.DealState == qtyDealStatus).ToList();
            //}
            /* Search IsCharged. */
            //if (!string.IsNullOrEmpty(qtyIsCharged))
            //{
            //    rv = rv.Where(r => r.IsCharged == qtyIsCharged).ToList();
            //}

            return View("List", rv);
        }

    }
}