using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Data.SqlClient;
using EDIS.Areas.BMED.Data;
using EDIS.Models.Identity;
using EDIS.Areas.BMED.Models.RepairModels;
using Microsoft.EntityFrameworkCore;
using EDIS.Areas.BMED.Models.KeepModels;

namespace EDIS.Areas.BMED.Models
{

    //查詢條件
    public class ReportQryVModel
    {
        [Display(Name = "報表類別")]
        public string ReportClass { get; set; }
        [Display(Name = "醫療儀器")]
        public string AssetClass1 { get; set; }
        [Display(Name = "工程設施")]
        public string AssetClass2 { get; set; }
        [Display(Name = "起始日期")]
        public DateTime? Sdate { get; set; }
        [Display(Name = "迄止日期")]
        public DateTime? Edate { get; set; }
        [Display(Name = "財產編號")]
        public string AssetNo { get; set; }
        [Display(Name = "設備名稱(關鍵字)")]
        public string AssetName { get; set; }
        [Display(Name = "成本中心")]
        public string AccDpt { get; set; }
        [Display(Name = "保管部門")]
        public string DelivDpt { get; set; }
        [Display(Name = "零件名稱")]
        public string StockName { get; set; }
    }
    //
    public class UserHour
    {
        public int Uid { get; set; }
        public decimal Hour { get; set; }
        public DateTime ApplyDate { get; set; }
        public DateTime EndDate { get; set; }
        [Display(Name = "設備類別")]
        public string AssetClass { get; set; }
        public string InOut { get; set; }
        public string AccDpt { get; set; }
    }

    public class Cust
    {
        [Display(Name = "單位代號")]
        public string CustId { get; set; }
        [Display(Name = "單位名稱")]
        public string CustNam { get; set; }
    }
    //維修履歷
    public class RpKpHistoryVModel
    {
        [Display(Name = "表單類別")]
        public string DocType { get; set; }
        [Display(Name = "表單編號")]
        public string DocId { get; set; }
        [Display(Name = "財產編號")]
        public string AssetNo { get; set; }
        [Display(Name = "設備名稱")]
        public string AssetName { get; set; }
        [Display(Name = "送單日期")]
        public DateTime ApplyDate { get; set; }
        [Display(Name = "完工日期")]
        public DateTime? EndDate { get; set; }
        [Display(Name = "工程師")]
        public string EngName { get; set; }
        [Display(Name = "工時")]
        public decimal? Hours { get; set; }
        [Display(Name = "費用")]
        public decimal? Cost { get; set; }
        [Display(Name = "故障原因")]
        public string TroubleDes { get; set; }
        [Display(Name = "處理情形")]
        public string DealDes { get; set; }
    }
    public class AssetAnalysis
    {
        [Display(Name = "保養時數")]
        public decimal KeepHour { get; set; }
        [Display(Name = "保養費用")]
        public decimal KeepCost { get; set; }
        [Display(Name = "維修時數")]
        public decimal RepairHour { get; set; }
        [Display(Name = "維修費用")]
        public decimal RepCost { get; set; }
        [Display(Name = "妥善率")]
        public decimal ProperRate { get; set; }
        [Display(Name = "維修比")]
        public decimal RepRatio { get; set; }
    }
    //妥善率報表
    public class ProperRate
    {
        [Display(Name = "財產編號")]
        public string AssetNo { get; set; }
        [Display(Name = "設備名稱")]
        public string AssetName { get; set; }
        [Display(Name = "廠牌")]
        public string Brand { get; set; }
        [Display(Name = "型號")]
        public string Type { get; set; }
        [Display(Name = "成本中心")]
        public string AccDpt { get; set; }
        [Display(Name = "成本中心名稱")]
        public string AccDptNam { get; set; }
        [Display(Name = "維修日數")]
        public double RepairDays { get; set; }
        [Display(Name = "維修次數")]
        public int RepairCnts { get; set; }
        [Display(Name = "妥善率")]
        public decimal AssetProperRate { get; set; }
    }
    //
    public class UserAsset
    {
        public int Uid { get; set; }
        public string AssetNo { get; set; }
    }
    //
    public class RepTotalPrice
    {
        public string DocId { get; set; }
        public decimal totalprice { get; set; }
    }
    //月故障率報表
    public class MonthFailRateVModel
    {
        [Display(Name = "財產編號")]
        public string AssetNo { get; set; }
        [Display(Name = "中文名稱")]
        public string Cname { get; set; }
        [Display(Name = "單位代號")]
        public string CustId { get; set; }
        [Display(Name = "單位名稱")]
        public string CustNam { get; set; }
        [Display(Name = "維修工時(分)")]
        public decimal RepairMins { get; set; }
        [Display(Name = "總工時(分)")]
        public decimal TotalMins { get; set; }
        [Display(Name = "故障率")]
        public string FailRate { get; set; }
        //[Display(Name = "廠牌")]
        //public string Brand { get; set; }
        //[Display(Name = "設備件數")]
        //public int PlantAmt { get; set; }
        //[Display(Name = "月維修件數")]
        //public int RepairAmt { get; set; }


        //public List<MonthFailRateVModel> GetList(string gid, DateTime sdate, DateTime edate, string cls)
        //{
        //    List<MonthFailRateVModel> mv = new List<MonthFailRateVModel>();
        //    BMEDcontext db = new BMEDcontext();
        //    List<CustOrgan> cv = db.CustOrgans.Where(c => c.GroupId == gid).ToList();
        //    MonthFailRateVModel m;
        //    int rcnt = 0;
        //    int kcnt = 0;
        //    foreach (CustOrgan co in cv)
        //    {
        //        rcnt = 0;
        //        m = new MonthFailRateVModel();
        //        m.CustId = co.CustId;
        //        m.CustNam = co.CustNam;
        //        if (!String.IsNullOrEmpty(cls))
        //        {
        //            rcnt = db.RepairDtls.Where(d => d.EndDate >= sdate)
        //            .Where(d => d.EndDate <= edate)
        //            .Join(db.Repairs, rd => rd.DocId, r => r.DocId,
        //            (rd, r) => new
        //            {
        //                rd.DocId,
        //                r.AccDpt,
        //                r.AssetNo
        //            }).Join(db.Assets, rd => rd.AssetNo, r => r.AssetNo,
        //            (rd, r) => new
        //            {
        //                rd.DocId,
        //                r.AccDpt,
        //                r.AssetClass
        //            }).Where(r => r.AssetClass == cls)
        //            .Where(r => r.AccDpt == co.CustId).Count();
        //            m.RepairAmt = rcnt;
        //            //
        //            kcnt = 0;
        //            kcnt = db.KeepDtls.Where(d => d.EndDate >= sdate)
        //            .Where(d => d.EndDate <= edate)
        //            .Join(db.Keeps, rd => rd.DocId, r => r.DocId,
        //            (rd, r) => new
        //            {
        //                rd.DocId,
        //                r.AccDpt,
        //                r.AssetNo
        //            }).Join(db.Assets, rd => rd.AssetNo, r => r.AssetNo,
        //            (rd, r) => new
        //            {
        //                rd.DocId,
        //                r.AccDpt,
        //                r.AssetClass
        //            }).Where(r => r.AssetClass == cls)
        //            .Where(r => r.AccDpt == co.CustId).Count();
        //            m.KeepAmt = kcnt;
        //        }
        //        else
        //        {
        //            rcnt = db.RepairDtls.Where(d => d.EndDate >= sdate)
        //        .Where(d => d.EndDate <= edate)
        //        .Join(db.Repairs, rd => rd.DocId, r => r.DocId,
        //        (rd, r) => new
        //        {
        //            rd.DocId,
        //            r.AccDpt
        //        }).Where(r => r.AccDpt == co.CustId).Count();
        //            m.RepairAmt = rcnt;
        //            //
        //            kcnt = 0;
        //            kcnt = db.KeepDtls.Where(d => d.EndDate >= sdate)
        //            .Where(d => d.EndDate <= edate)
        //            .Join(db.Keeps, rd => rd.DocId, r => r.DocId,
        //            (rd, r) => new
        //            {
        //                rd.DocId,
        //                r.AccDpt
        //            }).Where(r => r.AccDpt == co.CustId).Count();
        //            m.KeepAmt = kcnt;
        //        }

        //        if (kcnt > 0)
        //            m.FailRate = Convert.ToInt32(decimal.Round(Convert.ToDecimal(rcnt) / Convert.ToDecimal(kcnt) * 100m));
        //        else
        //            m.FailRate = 0;
        //        mv.Add(m);
        //    }
        //    return mv;
        //}
    }

    //設備保養排程年報
    public class AssetKpScheVModel
    {
        [Display(Name = "財產編號")]
        public string AssetNo { get; set; }
        [Display(Name = "設備名稱")]
        public string AssetName { get; set; }
        [Display(Name = "保管部門")]
        public string DelivDptName { get; set; }
        [Display(Name = "廠牌")]
        public string Brand { get; set; }
        [Display(Name = "型號")]
        public string Type { get; set; }
        [Display(Name = "一月")]
        public string Jan { get; set; }
        [Display(Name = "二月")]
        public string Feb { get; set; }
        [Display(Name = "三月")]
        public string Mar { get; set; }
        [Display(Name = "四月")]
        public string Apr { get; set; }
        [Display(Name = "五月")]
        public string May { get; set; }
        [Display(Name = "六月")]
        public string Jun { get; set; }
        [Display(Name = "七月")]
        public string Jul { get; set; }
        [Display(Name = "八月")]
        public string Aug { get; set; }
        [Display(Name = "九月")]
        public string Sep { get; set; }
        [Display(Name = "十月")]
        public string Oct { get; set; }
        [Display(Name = "十一月")]
        public string Nov { get; set; }
        [Display(Name = "十二月")]
        public string Dec { get; set; }

    }

    //維修保養統計表
    public class RepairKeepVModel
    {
        [Display(Name = "單位代號")]
        public string CustId { get; set; }
        [Display(Name = "單位名稱")]
        public string CustNam { get; set; }
        [Display(Name = "應保養")]
        public int KeepAmt { get; set; }
        [Display(Name = "已保養")]
        public int KpEndAmt { get; set; }
        [Display(Name = "保養達成率")]
        public decimal KeepFinishedRate { get; set; }
        [Display(Name = "保養費用")]
        public decimal KeepCost { get; set; }
        [Display(Name = "應維修")]
        public int RepairAmt { get; set; }
        [Display(Name = "已維修")]
        public int RpEndAmt { get; set; }
        [Display(Name = "維修達成率")]
        public decimal RepFinishedRate { get; set; }
        [Display(Name = "維修費用")]
        public decimal RepCost { get; set; }
        [Display(Name = "妥善率")]
        public decimal ProperRate { get; set; }
        [Display(Name = "維修比")]
        public decimal RepRatio { get; set; }

    }

    //重複故障統計表
    public class RepeatFailVModel
    {
        [Display(Name = "表單編號")]
        public string DocId { get; set; }
        [Display(Name = "請修日期")]
        public DateTime ApplyDate { get; set; }
        [Display(Name = "單位代號")]
        public string CustId { get; set; }
        [Display(Name = "單位名稱")]
        public string CustNam { get; set; }
        [Display(Name = "設備財編")]
        public string AssetNo { get; set; }
        [Display(Name = "設備名稱")]
        public string AssetNam { get; set; }
        [Display(Name = "設備型號")]
        public string Type { get; set; }
        [Display(Name = "故障描述")]
        public string TroubleDes { get; set; }
        [Display(Name = "故障原因")]
        public string FailFactor { get; set; }
        [Display(Name = "處理描述")]
        public string DealDes { get; set; }
        [Display(Name = "完工日期")]
        public DateTime EndDate { get; set; }
        [Display(Name = "維修費用")]
        public decimal Cost { get; set; }
        [Display(Name = "工程師")]
        public string EngNam { get; set; }

        private readonly BMEDDbContext db;
        public RepeatFailVModel(BMEDDbContext context)
        {
            db = context;
        }
        public RepeatFailVModel()
        {

        }

        public List<RepeatFailVModel> GetList(string gid, DateTime sdate, DateTime edate, string cls)
        {
            List<RepeatFailVModel> mv = new List<RepeatFailVModel>();
            List<RepeatFailVModel> mv2 = new List<RepeatFailVModel>();
            RepeatFailVModel m;
            RepairModel r;
            RepairEmpModel p;
            AssetModel a;
            DepartmentModel o;
            List<RepairDtlModel> rdtl = db.BMEDRepairDtls.Where(d => d.EndDate >= sdate)
                                                         .Where(d => d.EndDate <= edate).ToList();

            foreach (RepairDtlModel rd in rdtl)
            {
                m = new RepeatFailVModel();
                m.DocId = rd.DocId;
                m.DealDes = rd.DealDes;
                m.EndDate = rd.EndDate.Value;
                m.Cost = rd.Cost;
                m.FailFactor = Convert.ToString(rd.FailFactor);
                r = db.BMEDRepairs.Where(i => i.DocId == rd.DocId).ToList().FirstOrDefault();
                if (r != null)
                {
                    m.TroubleDes = r.TroubleDes;
                    m.CustId = r.AccDpt;
                    o = db.Departments.Where(c => c.DptId == r.AccDpt).ToList().FirstOrDefault();
                    //if (o.GroupId != gid)
                    //    continue;
                    if (o != null)
                        m.CustNam = o.Name_C;
                    m.ApplyDate = r.ApplyDate;
                    m.AssetNo = r.AssetNo;
                    if (!String.IsNullOrEmpty(cls))
                    {
                        a = db.BMEDAssets.Where(s => s.AssetNo == r.AssetNo)
                            .Where(s => s.AssetClass == cls).ToList().FirstOrDefault();
                    }
                    else
                    {
                        a = db.BMEDAssets.Where(s => s.AssetNo == r.AssetNo).ToList().FirstOrDefault();
                    }
                    if (a != null)
                    {
                        m.AssetNam = a.Cname;
                        m.Type = a.Type;
                    }
                }
                p = db.BMEDRepairEmps.Where(e => e.DocId == rd.DocId).ToList().FirstOrDefault();
                if (p != null)
                {
                    AppUserModel u = db.AppUsers.Find(p.UserId);
                    m.EngNam = u.FullName;
                }
                mv2.Add(m);
            }
            IEnumerable<IGrouping<string, RepeatFailVModel>> query = mv2.GroupBy(v => v.AssetNo);
            foreach (var group in query)
            {
                if (group.ToList().Count >= 2)
                {
                    foreach (RepeatFailVModel l in group)
                    {
                        mv.Add(l);
                    }
                }
            }
            return mv;
        }
    }

    //
    //月維修統計表
    public class MonthRepairVModel
    {
        [Display(Name = "表單編號")]
        public string DocId { get; set; }
        [Display(Name = "請修日期")]
        public DateTime ApplyDate { get; set; }
        [Display(Name = "完工日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? EndDate { get; set; }
        [Display(Name = "財產編號")]
        public string AssetNo { get; set; }
        [Display(Name = "設備名稱")]
        public string AssetNam { get; set; }
        [Display(Name = "設備類別")]
        public string AssetClass { get; set; }
        [Display(Name = "型號")]
        public string Type { get; set; }
        [Display(Name = "成本中心")]
        public string AccDpt { get; set; }
        [Display(Name = "成本中心名稱")]
        public string AccDptNam { get; set; }
        [Display(Name = "故障描述")]
        public string TroubleDes { get; set; }
        [Display(Name = "故障原因")]
        public string FailFactor { get; set; }
        [Display(Name = "處理描述")]
        public string DealDes { get; set; }
        [Display(Name = "處理狀況")]
        public string DealState { get; set; }
        [Display(Name = "維修方式")]
        public string InOut { get; set; }
        [Display(Name = "維修費用")]
        public decimal Cost { get; set; }
        [Display(Name = "工程師")]
        public string EngNam { get; set; }
        [Display(Name = "總工時")]
        public decimal Hour { get; set; }
        [Display(Name = "設備類別")]
        public string PlantClass { get; set; }

        private readonly BMEDDbContext db;
        public MonthRepairVModel(BMEDDbContext context)
        {
            db = context;
        }
        public MonthRepairVModel()
        {

        }
        public List<MonthRepairVModel> GetList(string gid, DateTime sdate, DateTime edate, string cls)
        {
            List<MonthRepairVModel> mv = new List<MonthRepairVModel>();
            mv = db.BMEDRepairDtls.Where(d => d.EndDate >= sdate)
            .Where(d => d.EndDate <= edate)
            .Join(db.BMEDRepairs, rd => rd.DocId, k => k.DocId,
            (rd, k) => new
            {
                rd.DocId,
                k.AccDpt,
                k.ApplyDate,
                k.AssetNo,
                rd.Cost,
                rd.EndDate,
                rd.FailFactor,
                rd.DealDes,
                rd.DealState,
                k.TroubleDes
            })
            .Join(db.BMEDAssets, k => k.AssetNo, at => at.AssetNo,
            (k, at) => new
            {
                k.DocId,
                k.AccDpt,
                k.ApplyDate,
                k.AssetNo,
                at.Cname,
                k.Cost,
                k.EndDate,
                k.TroubleDes,
                k.FailFactor,
                k.DealDes,
                k.DealState,
                at.Type,
                at.AssetClass
            })
            .Join(db.Departments, k => k.AccDpt, c => c.DptId,
            (k, c) => new
            {
                k.DocId,
                k.AccDpt,
                c.Name_C,
                ApplyDate = k.ApplyDate,
                k.AssetNo,
                k.Cname,
                Cost = k.Cost,
                EndDate = k.EndDate.Value,
                k.FailFactor,
                k.TroubleDes,
                k.DealDes,
                k.DealState,
                k.Type,
                k.AssetClass
            })
            .Join(db.BMEDRepairEmps, k => k.DocId, ke => ke.DocId,
            (k, ke) => new
            {
                k.DocId,
                k.AccDpt,
                k.Name_C,
                k.ApplyDate,
                k.AssetNo,
                k.Cname,
                k.Cost,
                k.EndDate,
                k.TroubleDes,
                k.FailFactor,
                k.DealDes,
                k.DealState,
                k.Type,
                k.AssetClass,
                ke.UserId
            })
            .Join(db.AppUsers, k => k.UserId, u => u.Id,
            (k, u) => new MonthRepairVModel
            {
                DocId = k.DocId,
                AccDpt = k.AccDpt,
                AccDptNam = k.Name_C,
                ApplyDate = k.ApplyDate,
                AssetNo = k.AssetNo,
                AssetNam = k.Cname,
                Cost = k.Cost,
                EndDate = k.EndDate,
                FailFactor = Convert.ToString(k.FailFactor),
                DealDes = k.DealDes,
                DealState = Convert.ToString(k.DealState),
                TroubleDes = k.TroubleDes,
                Type = k.Type,
                EngNam = u.FullName,
                AssetClass = k.AssetClass
            }).ToList();
            if (cls != "")
                mv = mv.Where(m => m.AssetClass == cls).ToList();
            return mv;
        }
    }

    //
    //月維修統計表
    public class MonthKeepVModel
    {
        [Display(Name = "表單編號")]
        public string DocId { get; set; }
        [Display(Name = "送單日期")]
        public DateTime SentDate { get; set; }
        [Display(Name = "完工日期")]
        public DateTime EndDate { get; set; }
        [Display(Name = "財產編號")]
        public string AssetNo { get; set; }
        [Display(Name = "儀器名稱")]
        public string AssetNam { get; set; }
        [Display(Name = "設備類別")]
        public string AssetClass { get; set; }
        [Display(Name = "成本中心")]
        public string AccDpt { get; set; }
        [Display(Name = "成本中心名稱")]
        public string AccDptNam { get; set; }
        [Display(Name = "意見描述")]
        public string DealDes { get; set; }
        [Display(Name = "保養方式")]
        public string InOut { get; set; }
        [Display(Name = "保養週期")]
        public decimal Cycle { get; set; }
        [Display(Name = "保養費用")]
        public decimal Cost { get; set; }
        [Display(Name = "工程師")]
        public string EngNam { get; set; }

        private readonly BMEDDbContext db;
        public MonthKeepVModel(BMEDDbContext context)
        {
            db = context;
        }
        public MonthKeepVModel()
        {

        }
        public List<MonthKeepVModel> GetList(string gid, DateTime sdate, DateTime edate, string cls)
        {
            List<MonthKeepVModel> mv = new List<MonthKeepVModel>();
            mv = db.BMEDKeepDtls.Where(d => d.EndDate >= sdate)
            .Where(d => d.EndDate <= edate)
            .Join(db.BMEDKeeps, rd => rd.DocId, k => k.DocId,
            (rd, k) => new
            {
                rd.DocId,
                k.AccDpt,
                k.SentDate,
                k.AssetNo,
                k.Cycle,
                rd.Cost,
                rd.EndDate,
                rd.Memo,
                rd.InOut
            })
            .Join(db.BMEDAssets, k => k.AssetNo, at => at.AssetNo,
            (k, at) => new
            {
                k.DocId,
                k.AccDpt,
                k.SentDate,
                k.AssetNo,
                at.Cname,
                k.Cycle,
                k.Cost,
                k.EndDate,
                k.Memo,
                k.InOut,
                at.AssetClass
            })
            .Join(db.Departments, k => k.AccDpt, c => c.DptId,
            (k, c) => new
            {
                k.DocId,
                k.AccDpt,
                c.Name_C,
                SentDate = k.SentDate.Value,
                k.AssetNo,
                k.Cname,
                k.Cycle,
                Cost = k.Cost.Value,
                EndDate = k.EndDate.Value,
                k.Memo,
                k.InOut,
                k.AssetClass
            })
            .GroupJoin(db.BMEDKeepEmps, k => k.DocId, ke => ke.DocId,
            (k, ke) => new { k, ke })
            .SelectMany(oi => oi.ke.DefaultIfEmpty(),
            (k, ke) => new
            {
                k.k.DocId,
                k.k.AccDpt,
                k.k.Name_C,
                k.k.SentDate,
                k.k.AssetNo,
                k.k.Cname,
                k.k.Cycle,
                k.k.Cost,
                k.k.EndDate,
                k.k.Memo,
                k.k.InOut,
                k.k.AssetClass,
                ke.UserId
            })
            .GroupJoin(db.AppUsers, k => k.UserId, u => u.Id,
            (k, u) => new { k, u })
            .SelectMany(ui => ui.u.DefaultIfEmpty(),
            (k, u) => new MonthKeepVModel
            {
                DocId = k.k.DocId,
                AccDpt = k.k.AccDpt,
                AccDptNam = k.k.Name_C,
                SentDate = k.k.SentDate,
                AssetNo = k.k.AssetNo,
                AssetNam = k.k.Cname,
                Cycle = k.k.Cycle,
                Cost = k.k.Cost,
                EndDate = k.k.EndDate,
                DealDes = k.k.Memo,
                InOut = k.k.InOut,
                AssetClass = k.k.AssetClass,
                EngNam = u.FullName
            }).ToList();
            foreach (MonthKeepVModel s in mv)
            {
                switch (s.InOut)
                {
                    case "0":
                        s.InOut = "自行";
                        break;
                    case "1":
                        s.InOut = "委外";
                        break;
                    case "2":
                        s.InOut = "租賃";
                        break;
                    case "3":
                        s.InOut = "保固";
                        break;
                    case "4":
                        s.InOut = "借用";
                        break;
                }
            }
            if (cls != "")
                mv = mv.Where(m => m.AssetClass == cls).ToList();
            return mv;
        }
    }

    public class DoHrSumMonVModel
    {
        [Display(Name = "工程師代號")]
        public int UserId { get; set; }
        [Display(Name = "工程師姓名")]
        public string UserNam { get; set; }
        [Display(Name = "工時")]
        public decimal Hours { get; set; }
        [Display(Name = "件數")]
        public int Cases { get; set; }
        [Display(Name = "轉撥計價費用")]
        public decimal Costs { get; set; }
        [Display(Name = "超過五天件數")]
        public int OverFive { get; set; }
        [Display(Name = "五日完修率")]
        public decimal OverFiveRate { get; set; }
        [Display(Name = "自修率")]
        public decimal SelfRate { get; set; }
        [Display(Name = "三個月維修總件數")]
        public int Case3M { get; set; }
        [Display(Name = "三個月重複故障率")]
        public decimal Fail3MRate { get; set; }
        [Display(Name = "重複故障率")]
        public decimal Fail1MRate { get; set; }

        private readonly BMEDDbContext db;
        public DoHrSumMonVModel(BMEDDbContext context)
        {
            db = context;
        }
        public DoHrSumMonVModel()
        {

        }
        public List<DoHrSumMonVModel> GetList(string gid, DateTime sdate, DateTime edate, string cls)
        {
            List<DoHrSumMonVModel> mv = new List<DoHrSumMonVModel>();
            DoHrSumMonVModel dv;
            List<UserHour> query = db.BMEDRepairDtls.Where(d => d.EndDate >= sdate)
                .Where(d => d.EndDate <= edate)
                .Join(db.BMEDRepairs, rd => rd.DocId, r => r.DocId,
                (rd, r) => new
                {
                    rd.DocId,
                    rd.Hour,
                    rd.EndDate,
                    r.ApplyDate,
                    r.AccDpt,
                    r.AssetNo
                }).Join(db.BMEDAssets, k => k.AssetNo, c => c.AssetNo,
                (k, c) => new
                {
                    k.DocId,
                    k.Hour,
                    k.EndDate,
                    k.ApplyDate,
                    k.AccDpt,
                    c.AssetClass
                }).Join(db.Departments, k => k.AccDpt, c => c.DptId,
            (k, c) => new
            {
                k.DocId,
                k.Hour,
                k.EndDate,
                k.ApplyDate,
                k.AssetClass
            })
                .Join(db.BMEDRepairEmps, rd => rd.DocId, re => re.DocId,
                (rd, re) => new UserHour
                {
                    Uid = re.UserId,
                    Hour = rd.Hour,
                    AssetClass = rd.AssetClass,
                    ApplyDate = rd.ApplyDate,
                    EndDate = rd.EndDate.Value
                }).ToList();
            if (!String.IsNullOrEmpty(cls))
                query = query.Where(k => k.AssetClass == cls).ToList();
            IEnumerable<IGrouping<int, UserHour>> rt = query.GroupBy(v => v.Uid);
            int case1 = 0;
            int case5 = 0;
            int abc = 0;
            DateTime sd = sdate.AddMonths(-2);
            foreach (var g in rt)
            {
                case1 = 0;
                case5 = 0;
                dv = new DoHrSumMonVModel();
                dv.UserId = g.Key;
                dv.UserNam = db.AppUsers.Find(g.Key).FullName;
                dv.Cases = g.Count();
                dv.Hours = g.Sum(s => s.Hour);
                case1 = g.Where(g1 => g1.EndDate.Subtract(g1.ApplyDate).Days < 5).Count();
                case5 = g.Where(g1 => g1.EndDate.Subtract(g1.ApplyDate).Days >= 5).Count();
                dv.OverFive = case5;
                if (case1 + case5 > 0)
                {
                    dv.OverFiveRate = Decimal.Round(Convert.ToDecimal(case1) /
                            Convert.ToDecimal(case1 + case5), 2);
                }
                else
                    dv.OverFiveRate = 0m;
                //
                dv.Case3M = db.BMEDRepairDtls.Where(d => d.EndDate >= sd)
                .Where(d => d.EndDate <= edate)
                .Join(db.BMEDRepairEmps, rd => rd.DocId, re => re.DocId,
                (rd, re) => new
                {
                    re.UserId
                }).Where(re => re.UserId == g.Key).Count();
                //dv.Fail3MRate
                IEnumerable<IGrouping<string, UserAsset>> ob = db.BMEDRepairDtls.Where(d => d.EndDate >= sd)
                .Where(d => d.EndDate <= edate)
                .Join(db.BMEDRepairs, rd => rd.DocId, r => r.DocId,
                (rd, r) => new
                {
                    rd.DocId,
                    r.AssetNo
                })
                .Join(db.BMEDRepairEmps, rd => rd.DocId, re => re.DocId,
                (rd, re) => new UserAsset
                {
                    Uid = re.UserId,
                    AssetNo = rd.AssetNo
                }).Where(re => re.Uid == g.Key).GroupBy(v => v.AssetNo);
                abc = 0;
                foreach (var q in ob)
                {
                    if (q.Count() >= 2)
                        abc += q.Count();
                }
                if (dv.Case3M > 0)
                    dv.Fail3MRate = Decimal.Round(Convert.ToDecimal(abc) / Convert.ToDecimal(dv.Case3M), 2);
                else
                    dv.Fail3MRate = 0m;
                //
                ob = db.BMEDRepairDtls.Where(d => d.EndDate >= sdate)
                .Where(d => d.EndDate <= edate)
                .Join(db.BMEDRepairs, rd => rd.DocId, r => r.DocId,
                (rd, r) => new
                {
                    rd.DocId,
                    r.AssetNo
                })
                .Join(db.BMEDRepairEmps, rd => rd.DocId, re => re.DocId,
                (rd, re) => new UserAsset
                {
                    Uid = re.UserId,
                    AssetNo = rd.AssetNo
                }).Where(re => re.Uid == g.Key).GroupBy(re => re.AssetNo);
                abc = 0;
                foreach (var q in ob)
                {
                    if (q.Count() >= 2)
                        abc += q.Count();
                }
                if (dv.Case3M > 0)
                    dv.Fail1MRate = Decimal.Round(Convert.ToDecimal(abc) / Convert.ToDecimal(dv.Case3M), 2);
                else
                    dv.Fail1MRate = 0m;
                mv.Add(dv);
            }
            return mv;
        }
    }

    //零件帳務清單
    public class StokCostVModel
    {
        [Display(Name = "類別")]
        public string DocTyp { get; set; }
        [Display(Name = "表單編號")]
        public string DocId { get; set; }
        [Display(Name = "送單日期")]
        public DateTime ApplyDate { get; set; }
        [Display(Name = "完工日期")]
        public DateTime? EndDate { get; set; }
        [Display(Name = "財產編號")]
        public string AssetNo { get; set; }
        [Display(Name = "設備名稱")]
        public string AssetNam { get; set; }
        [Display(Name = "型號")]
        public string Type { get; set; }
        [Display(Name = "成本中心")]
        public string AccDpt { get; set; }
        [Display(Name = "成本中心名稱")]
        public string AccDptNam { get; set; }
        [Display(Name = "故障描述/保養週期")]
        public string TroubleDes { get; set; }
        [Display(Name = "故障原因")]
        public string FailFactor { get; set; }
        [Display(Name = "處理描述")]
        public string DealDes { get; set; }
        [Display(Name = "零件廠牌")]
        public string Brand { get; set; }
        [Display(Name = "料號")]
        public string StokNo { get; set; }
        [Display(Name = "零件名稱")]
        public string StokNam { get; set; }
        [Display(Name = "數量")]
        public int Qty { get; set; }
        [Display(Name = "單價")]
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }
        [Display(Name = "合計")]
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal TotalPrice { get; set; }
        [Display(Name = "費用")]
        public decimal Cost { get; set; }
        [Display(Name = "結案日")]
        public Nullable<DateTime> CloseDate { get; set; }
        [Display(Name = "工程師")]
        public string EngNam { get; set; }
        public string AssetClass { get; set; }

        private readonly BMEDDbContext db;
        public StokCostVModel(BMEDDbContext context)
        {
            db = context;
        }
        public StokCostVModel()
        {

        }
        public List<StokCostVModel> GetList(string gid, DateTime sdate, DateTime edate, string cls)
        {
            List<StokCostVModel> sv = new List<StokCostVModel>();
            List<StokCostVModel> sv2 = new List<StokCostVModel>();
            sv = db.BMEDRepairDtls.Where(d => d.CloseDate >= sdate
                && d.CloseDate <= edate)
            .Join(db.BMEDRepairs, rd => rd.DocId, k => k.DocId,
            (rd, k) => new
            {
                rd.DocId,
                k.AccDpt,
                k.ApplyDate,
                k.AssetNo,
                rd.Cost,
                rd.EndDate,
                rd.CloseDate,
                rd.FailFactor,
                rd.DealDes,
                k.TroubleDes
            })
            .Join(db.BMEDAssets, k => k.AssetNo, at => at.AssetNo,
            (k, at) => new
            {
                k.DocId,
                k.AccDpt,
                k.ApplyDate,
                k.AssetNo,
                at.Cname,
                k.Cost,
                k.EndDate,
                k.CloseDate,
                k.FailFactor,
                k.TroubleDes,
                k.DealDes,
                at.Type,
                at.AssetClass
            })
            .GroupJoin(db.BMEDRepairCosts, k => k.DocId, rc => rc.DocId,
            (k, rc) => new { k, rc })
            .SelectMany(oi => oi.rc.DefaultIfEmpty(),
           (k, rc) => new
           {
               k.k.DocId,
               k.k.AccDpt,
               k.k.ApplyDate,
               k.k.AssetNo,
               k.k.Cname,
               k.k.Cost,
               k.k.EndDate,
               k.k.CloseDate,
               k.k.FailFactor,
               k.k.TroubleDes,
               k.k.DealDes,
               k.k.Type,
               k.k.AssetClass,
               PartNo = rc.PartNo != null ? rc.PartNo : "",
               PartNam = rc.PartName != null ? rc.PartName : "",
               Qty = rc.Qty != null ? rc.Qty : 0,
               Price = rc.Price != null ? rc.Price : 0,
               TotalCost = rc.TotalCost != null ? rc.TotalCost : 0
           })
             .Join(db.Departments, k => k.AccDpt, c => c.DptId,
            (k, c) => new
            {
                k.DocId,
                k.AccDpt,
                k.ApplyDate,
                k.AssetNo,
                k.Cname,
                k.Cost,
                k.EndDate,
                k.CloseDate,
                k.FailFactor,
                k.TroubleDes,
                k.DealDes,
                k.Type,
                k.PartNo,
                k.PartNam,
                k.AssetClass,
                k.Qty,
                k.Price,
                k.TotalCost,
                c.Name_C
            })
             .Join(db.BMEDRepairEmps, k => k.DocId, ke => ke.DocId,
            (k, ke) => new
            {
                k.DocId,
                k.AccDpt,
                k.ApplyDate,
                k.AssetNo,
                k.Cname,
                k.Cost,
                k.EndDate,
                k.CloseDate,
                k.FailFactor,
                k.TroubleDes,
                k.DealDes,
                k.Type,
                k.PartNo,
                k.PartNam,
                k.AssetClass,
                k.Qty,
                k.Price,
                k.TotalCost,
                k.Name_C,
                ke.UserId
            })
            .Join(db.AppUsers, k => k.UserId, u => u.Id,
            (k, u) => new StokCostVModel
            {
                DocTyp = "請修",
                DocId = k.DocId,
                AccDpt = k.AccDpt,
                AccDptNam = k.Name_C,
                AssetNo = k.AssetNo,
                AssetNam = k.Cname,
                Type = k.Type,
                ApplyDate = k.ApplyDate,
                EndDate = k.EndDate,
                CloseDate = k.CloseDate,
                FailFactor = Convert.ToString(k.FailFactor),
                TroubleDes = k.TroubleDes,
                DealDes = k.DealDes,
                StokNo = k.PartNo,
                StokNam = k.PartNam,
                Qty = k.Qty,
                Price = k.Price,
                TotalPrice = k.TotalCost,
                EngNam = u.FullName,
                AssetClass = k.AssetClass
            }).ToList();
            //
            foreach (StokCostVModel s in sv)
            {
                if (s.StokNo != "")
                    s.Cost = db.BMEDRepairCosts.Where(r => r.DocId == s.DocId).Sum(r => r.TotalCost);
            }
            //保養
            sv2 = db.BMEDKeepDtls.Where(d => d.CloseDate >= sdate
               && d.CloseDate <= edate)
           .Join(db.BMEDKeeps, rd => rd.DocId, k => k.DocId,
           (rd, k) => new
           {
               rd.DocId,
               k.AccDpt,
               k.SentDate,
               k.AssetNo,
               rd.Cost,
               rd.EndDate,
               rd.CloseDate,
               rd.Result,
               k.Cycle
           })
           .Join(db.BMEDAssets, k => k.AssetNo, at => at.AssetNo,
           (k, at) => new
           {
               k.DocId,
               k.AccDpt,
               k.SentDate,
               k.AssetNo,
               at.Cname,
               k.Cost,
               k.EndDate,
               k.CloseDate,
               k.Result,
               k.Cycle,
               at.Type,
               at.AssetClass
           })
            .GroupJoin(db.BMEDKeepCosts, k => k.DocId, rc => rc.DocId,
            (k, rc) => new { k, rc })
            .SelectMany(oi => oi.rc.DefaultIfEmpty(),
           (k, rc) => new
           {
               k.k.DocId,
               k.k.AccDpt,
               k.k.SentDate,
               k.k.AssetNo,
               k.k.Cname,
               k.k.Cost,
               k.k.EndDate,
               k.k.CloseDate,
               k.k.Result,
               k.k.Cycle,
               k.k.Type,
               k.k.AssetClass,
               PartNo = rc.PartNo != null ? rc.PartNo : "",
               PartNam = rc.PartName != null ? rc.PartName : "",
               Qty = rc.Qty != null ? rc.Qty : 0,
               Price = rc.Price != null ? rc.Price : 0,
               TotalCost = rc.TotalCost != null ? rc.TotalCost : 0
           })
            .Join(db.Departments, k => k.AccDpt, c => c.DptId,
           (k, c) => new
           {
               k.DocId,
               k.AccDpt,
               k.SentDate,
               k.AssetNo,
               k.Cname,
               k.Cost,
               k.EndDate,
               k.CloseDate,
               k.Result,
               k.Cycle,
               k.Type,
               k.PartNo,
               k.PartNam,
               k.Qty,
               k.Price,
               k.TotalCost,
               k.AssetClass,
               c.Name_C
           })
            .GroupJoin(db.BMEDKeepEmps, k => k.DocId, ke => ke.DocId,
           (k, ke) => new { k, ke })
            .SelectMany(ee => ee.ke.DefaultIfEmpty(),
           (k, ke) => new
           {
               k.k.DocId,
               k.k.AccDpt,
               k.k.SentDate,
               k.k.AssetNo,
               k.k.Cname,
               k.k.Cost,
               k.k.EndDate,
               k.k.CloseDate,
               k.k.Result,
               k.k.Cycle,
               k.k.Type,
               k.k.PartNo,
               k.k.PartNam,
               k.k.Qty,
               k.k.Price,
               k.k.TotalCost,
               k.k.Name_C,
               k.k.AssetClass,
               UserId = ke.UserId
           })
           .GroupJoin(db.AppUsers, k => k.UserId, u => u.Id,
           (k, u) => new { k, u })
            .SelectMany(ee => ee.u.DefaultIfEmpty(),
           (k, u) => new StokCostVModel
           {
               DocTyp = "保養",
               DocId = k.k.DocId,
               AccDpt = k.k.AccDpt,
               AccDptNam = k.k.Name_C,
               AssetNo = k.k.AssetNo,
               AssetNam = k.k.Cname,
               Type = k.k.Type,
               ApplyDate = k.k.SentDate.Value,
               EndDate = k.k.EndDate,
               CloseDate = k.k.CloseDate,
               DealDes = Convert.ToString(k.k.Result),
               StokNo = k.k.PartNo,
               StokNam = k.k.PartNam,
               Qty = k.k.Qty,
               Price = k.k.Price,
               TotalPrice = k.k.TotalCost,
               EngNam = u.Id != 0 ? u.FullName : "",
               AssetClass = k.k.AssetClass
           }).ToList();
            //
            foreach (StokCostVModel s in sv2)
            {
                switch (s.DealDes)
                {
                    case "1":
                        s.DealDes = "功能正常";
                        break;
                    case "2":
                        s.DealDes = "預防處理";
                        break;
                    case "3":
                        s.DealDes = "異常處理";
                        break;
                    case "4":
                        s.DealDes = "維修時保養";
                        break;
                    case "5":
                        s.DealDes = "退件";
                        break;
                }
                s.TroubleDes = db.BMEDKeeps.Find(s.DocId).Cycle.ToString();
                s.FailFactor = "";
                if (s.StokNo != "")
                    s.Cost = db.BMEDKeepCosts.Where(r => r.DocId == s.DocId).Sum(r => r.TotalCost);
            }
            sv.AddRange(sv2);
            if (!String.IsNullOrEmpty(cls))
                sv = sv.Where(s => s.AssetClass == cls).ToList();
            return sv;
        }
    }

    //維修保養零件統計表
    public class RepKeepStokVModel
    {
        [Display(Name = "單位代號")]
        public string CustId { get; set; }
        [Display(Name = "單位名稱")]
        public string CustNam { get; set; }
        [Display(Name = "保養件數")]
        public int KeepAmt { get; set; }
        [Display(Name = "保養費用")]
        public decimal KeepCost { get; set; }
        [Display(Name = "維修件數")]
        public int RepairAmt { get; set; }
        [Display(Name = "維修費用")]
        public decimal RepairCost { get; set; }
        [Display(Name = "件數合計")]
        public int TotalAmt { get; set; }
        [Display(Name = "費用合計")]
        public decimal TotalCost { get; set; }

        private readonly BMEDDbContext db;
        public RepKeepStokVModel(BMEDDbContext context)
        {
            db = context;
        }
        public RepKeepStokVModel()
        {

        }
        public List<RepKeepStokVModel> GetList(string gid, DateTime sdate, DateTime edate, string cls)
        {
            List<RepKeepStokVModel> mv = new List<RepKeepStokVModel>();
            //List<CustOrgan> cv = db.CustOrgans.Where(c => c.GroupId == gid).ToList();
            List<Cust> cv;
            if (!String.IsNullOrEmpty(cls))
            {
                cv =
            db.BMEDRepairDtls.Where(d => d.CloseDate >= sdate &&
                d.CloseDate <= edate)
                .Join(db.BMEDRepairs, rd => rd.DocId, r => r.DocId,
                (rd, r) => new
                {
                    rd.DocId,
                    r.AccDpt,
                    r.AssetNo
                }).Join(db.BMEDAssets, rd => rd.AssetNo, r => r.AssetNo,
                (rd, r) => new
                {
                    rd.DocId,
                    r.AccDpt,
                    r.AssetClass
                }).Where(r => r.AssetClass == cls)
                .Join(db.Departments, rd => rd.AccDpt, c => c.DptId,
                (rd, c) => new Cust
                {
                    CustId = c.DptId,
                    CustNam = c.Name_C
                }).Union(
           db.BMEDKeepDtls.Where(d => d.CloseDate >= sdate &&
               d.CloseDate <= edate)
               .Join(db.BMEDKeeps, rd => rd.DocId, r => r.DocId,
               (rd, r) => new
               {
                   rd.DocId,
                   r.AccDpt,
                   r.AssetNo
               }).Join(db.BMEDAssets, rd => rd.AssetNo, r => r.AssetNo,
                (rd, r) => new
                {
                    rd.DocId,
                    r.AccDpt,
                    r.AssetClass
                }).Where(r => r.AssetClass == cls)
               .Join(db.Departments, rd => rd.AccDpt, c => c.DptId,
               (rd, c) => new Cust
               {
                   CustId = c.DptId,
                   CustNam = c.Name_C
               })).Distinct().ToList();
            }
            else
            {
                cv =
            db.BMEDRepairDtls.Where(d => d.CloseDate >= sdate &&
                d.CloseDate <= edate)
                .Join(db.BMEDRepairs, rd => rd.DocId, r => r.DocId,
                (rd, r) => new
                {
                    rd.DocId,
                    r.AccDpt
                })
                .Join(db.Departments, rd => rd.AccDpt, c => c.DptId,
                (rd, c) => new Cust
                {
                    CustId = c.DptId,
                    CustNam = c.Name_C
                }).Union(
           db.BMEDKeepDtls.Where(d => d.CloseDate >= sdate &&
               d.CloseDate <= edate)
               .Join(db.BMEDKeeps, rd => rd.DocId, r => r.DocId,
               (rd, r) => new
               {
                   rd.DocId,
                   r.AccDpt
               })
               .Join(db.Departments, rd => rd.AccDpt, c => c.DptId,
               (rd, c) => new Cust
               {
                   CustId = c.DptId,
                   CustNam = c.Name_C
               })).Distinct().ToList();
            }

            RepKeepStokVModel m;
            int rcnt = 0;
            int kcnt = 0;
            decimal tolcost = 0m;
            List<RepTotalPrice> pp = new List<RepTotalPrice>();
            foreach (Cust co in cv)
            {
                //if (db.Departments.Find(co.CustId).GroupId != gid)
                //    continue;
                rcnt = 0;
                tolcost = 0m;
                pp.Clear();
                m = new RepKeepStokVModel();
                m.CustId = co.CustId;
                m.CustNam = co.CustNam;
                rcnt = db.BMEDRepairDtls.Where(d => d.CloseDate >= sdate &&
                d.CloseDate <= edate)
                .Join(db.BMEDRepairs, rd => rd.DocId, r => r.DocId,
                (rd, r) => new
                {
                    rd.DocId,
                    r.AccDpt
                }).Where(r => r.AccDpt == co.CustId).Count();
                pp = db.BMEDRepairDtls.Where(d => d.CloseDate >= sdate &&
                d.CloseDate <= edate)
                .Join(db.BMEDRepairs, rd => rd.DocId, r => r.DocId,
                (rd, r) => new
                {
                    rd.DocId,
                    r.AccDpt
                }).Where(r => r.AccDpt == co.CustId).Join(db.BMEDRepairCosts, rd => rd.DocId, c => c.DocId,
                (rd, c) => new RepTotalPrice
                {
                    DocId = rd.DocId,
                    totalprice = c.TotalCost
                }).ToList();
                if (pp.Count > 0)
                    tolcost = pp.Sum(p => p.totalprice);
                m.RepairAmt = rcnt;
                m.RepairCost = tolcost;
                //
                kcnt = 0;
                tolcost = 0;
                kcnt = db.BMEDKeepDtls.Where(d => d.CloseDate >= sdate &&
                d.CloseDate <= edate)
               .Join(db.BMEDKeeps, rd => rd.DocId, r => r.DocId,
               (rd, r) => new
               {
                   rd.DocId,
                   r.AccDpt
               }).Where(r => r.AccDpt == co.CustId).Count();
                pp.Clear();
                pp = db.BMEDKeepDtls.Where(d => d.CloseDate >= sdate &&
                d.CloseDate <= edate)
                .Join(db.BMEDKeeps, rd => rd.DocId, r => r.DocId,
                (rd, r) => new
                {
                    rd.DocId,
                    r.AccDpt
                }).Where(r => r.AccDpt == co.CustId).Join(db.BMEDKeepCosts, rd => rd.DocId, c => c.DocId,
                (rd, c) => new RepTotalPrice
                {
                    DocId = rd.DocId,
                    totalprice = c.TotalCost
                }).ToList();
                if (pp.Count > 0)
                    tolcost = pp.Sum(p => p.totalprice);
                m.KeepAmt = kcnt;
                m.KeepCost = tolcost;
                //
                m.TotalAmt = m.RepairAmt + m.KeepAmt;
                m.TotalCost = m.RepairCost + m.KeepCost;
                mv.Add(m);
            }
            return mv;
        }
    }

    //未結案清單
    public class UnSignListVModel
    {
        [Display(Name = "類別")]
        public string DocTyp { get; set; }
        [Display(Name = "表單編號")]
        public string DocId { get; set; }
        [Display(Name = "送單日期")]
        public DateTime ApplyDate { get; set; }
        [Display(Name = "完工日期")]
        public DateTime? EndDate { get; set; }
        [Display(Name = "財產編號")]
        public string AssetNo { get; set; }
        [Display(Name = "設備名稱")]
        public string AssetName { get; set; }
        [Display(Name = "型號")]
        public string Type { get; set; }
        [Display(Name = "成本中心")]
        public string AccDpt { get; set; }
        [Display(Name = "成本中心名稱")]
        public string AccDptNam { get; set; }
        [Display(Name = "故障描述/保養週期")]
        public string TroubleDes { get; set; }
        [Display(Name = "故障原因")]
        public string FailFactor { get; set; }
        [Display(Name = "處理狀況/保養結果")]
        public string DealState { get; set; }
        [Display(Name = "保養方式")]
        public string InOut { get; set; }
        [Display(Name = "處理描述")]
        public string DealDes { get; set; }
        [Display(Name = "費用")]
        public decimal? Cost { get; set; }
        [Display(Name = "工程師")]
        public string EngNam { get; set; }
        [Display(Name = "現在關卡")]
        public string ClsEmp { get; set; }
        public string AssetClass { get; set; }

        private readonly BMEDDbContext db;
        public UnSignListVModel(BMEDDbContext context)
        {
            db = context;
        }
        public UnSignListVModel()
        {

        }
        public List<UnSignListVModel> GetList(string gid, DateTime sdate, DateTime edate, string cls)
        {
            List<UnSignListVModel> sv = new List<UnSignListVModel>();
            List<UnSignListVModel> sv2 = new List<UnSignListVModel>();
            sv = db.BMEDRepairFlows.Where(f => f.Status == "?")
            .Join(db.BMEDRepairDtls, f => f.DocId, rd => rd.DocId,
            (f, rd) => new
            {
                f.DocId,
                f.UserId,
                f.Cls,
                rd.Cost,
                rd.EndDate,
                rd.CloseDate,
                rd.FailFactor,
                rd.DealDes,
                rd.DealState
            })
            .Join(db.BMEDRepairs, rd => rd.DocId, k => k.DocId,
            (rd, k) => new
            {
                rd.DocId,
                rd.UserId,
                rd.Cls,
                k.AccDpt,
                k.ApplyDate,
                k.AssetNo,
                rd.Cost,
                rd.EndDate,
                rd.CloseDate,
                rd.FailFactor,
                rd.DealDes,
                rd.DealState,
                k.TroubleDes
            }).Where(k => k.ApplyDate >= sdate && k.ApplyDate <= edate)
            .Join(db.BMEDAssets, k => k.AssetNo, at => at.AssetNo,
            (k, at) => new
            {
                k.DocId,
                k.UserId,
                k.Cls,
                k.AccDpt,
                k.ApplyDate,
                k.AssetNo,
                at.Cname,
                k.Cost,
                k.EndDate,
                k.CloseDate,
                k.TroubleDes,
                k.FailFactor,
                k.DealDes,
                k.DealState,
                at.Type,
                at.AssetClass
            })
             .Join(db.Departments, k => k.AccDpt, c => c.DptId,
            (k, c) => new
            {
                k.DocId,
                k.UserId,
                k.Cls,
                k.AccDpt,
                k.ApplyDate,
                k.AssetNo,
                k.Cname,
                k.Cost,
                k.EndDate,
                k.CloseDate,
                k.TroubleDes,
                k.FailFactor,
                k.DealDes,
                k.DealState,
                k.Type,
                k.AssetClass,
                c.Name_C
            })
            .Join(db.AppUsers, k => k.UserId, u => u.Id,
            (k, u) => new UnSignListVModel
            {
                DocTyp = "請修",
                DocId = k.DocId,
                AccDpt = k.AccDpt,
                AccDptNam = k.Name_C,
                AssetNo = k.AssetNo,
                AssetName = k.Cname,
                Type = k.Type,
                ApplyDate = k.ApplyDate,
                EndDate = k.EndDate,
                TroubleDes = k.TroubleDes,
                FailFactor = Convert.ToString(k.FailFactor),
                DealDes = k.DealDes,
                DealState = Convert.ToString(k.DealState),
                EngNam = null,
                ClsEmp = u.FullName,
                AssetClass = k.AssetClass
            }).ToList();
            //
            foreach (UnSignListVModel s in sv)
            {
                RepairEmpModel kp = db.BMEDRepairEmps.Where(p => p.DocId == s.DocId).ToList()
                   .FirstOrDefault();
                if (kp != null)
                {
                    s.EngNam = db.AppUsers.Find(kp.UserId).FullName;
                }
                List<RepairCostModel> lk = db.BMEDRepairCosts.Where(r => r.DocId == s.DocId).ToList();
                if (lk != null)
                    s.Cost = lk.Sum(r => r.TotalCost);
            }
            //保養
            string str = "";
            str += "SELECT '保養' AS DOCTYP,B.DocId,B.ASSETNO, B.ASSETNAM,B.SENTDATE AS APPLYDATE,D.FULLNAME AS CLSEMP,";
            str += "B.ACCDPT,E.CUSTNAM AS ACCDPTNAM, C.ENDDATE,C.RESULT AS DEALSTATE,C.INOUT, ";
            str += "STR(G.CYCLE) AS DEALDES, F.ASSETCLASS ";
            str += "FROM BMEDKEEPFLOWS AS A JOIN BMEDKEEPS AS B ON A.DocId = B.DocId ";
            str += "JOIN BMEDKEEPDTLS AS C ON B.DocId = C.DocId ";
            str += "JOIN AppUsers AS D ON B.USERID = D.USERID ";
            str += "JOIN DEPARTMENTS AS E ON B.ACCDPT = E.CUSTID "; ;
            str += "LEFT JOIN BMEDASSETS AS F ON B.AssetNo = F.AssetNo ";
            str += "LEFT JOIN BMEDASSETKEEPS AS G ON B.AssetNo = G.AssetNo ";
            str += "WHERE A.STATUS = '?' AND (B.SENTDATE BETWEEN @D1 AND @D2) ";
            str += "AND E.GROUPID = @GID ";

            sv2 = sv.AsQueryable().FromSql(str,
                new SqlParameter("D1", sdate),
                new SqlParameter("D2", edate),
                new SqlParameter("GID", gid)).ToList();
            /*sv2 = db.KeepFlows.Where(f => f.Status == "?")
            .Join(db.KeepDtls, f => f.DocId, rd => rd.DocId,
            (f, rd) => new
            {
                f.DocId,
                f.Userid,
                f.Cls,
                rd.Cost,
                rd.EndDate,
                rd.CloseDate,
                rd.Result,
                rd.InOut
            })
           .Join(db.Keeps, rd => rd.DocId, k => k.DocId,
           (rd, k) => new
           {
               rd.DocId,
               rd.Userid,
               rd.Cls,
               k.AccDpt,
               k.SentDate,
               k.AssetNo,
               rd.Cost,
               rd.EndDate,
               rd.CloseDate,
               rd.Result,
               rd.InOut,
               k.Cycle
           }).Where(k => k.SentDate >= sdate && k.SentDate <= edate)
                //.Join(db.Assets, k => k.AssetNo, at => at.AssetNo,
                //(k, at) => new
           .GroupJoin(db.Assets, k => k.AssetNo, at => at.AssetNo,
           (k, at) => new { k, at })
           .SelectMany(oi => oi.at.DefaultIfEmpty(),
           (k, at) => new
           {
               k.k.DocId,
               k.k.Userid,
               k.k.Cls,
               k.k.AccDpt,
               k.k.SentDate,
               k.k.AssetNo,
               at.Cname,
               k.k.Cost,
               k.k.EndDate,
               k.k.CloseDate,
               k.k.Result,
               k.k.InOut,
               k.k.Cycle,
               at.Type
           })
           .Join(db.CustOrgans, k => k.AccDpt, c => c.CustId,
           (k, c) => new
           {
               k.DocId,
               k.Userid,
               k.Cls,
               k.AccDpt,
               k.SentDate,
               k.AssetNo,
               k.Cname,
               k.Cost,
               k.EndDate,
               k.CloseDate,
               k.Result,
               k.InOut,
               k.Cycle,
               k.Type,
               //k.PartNo,
               //k.PartNam,
               //k.Qty,
               //k.Price,
               //k.TotalCost,
               c.GroupId,
               c.CustNam
           }).Where(c => c.GroupId == gid)
            .GroupJoin(db.AssetKeeps, k => k.AssetNo, ke => ke.AssetNo,
           (k, ke) => new { k, ke })
           .SelectMany(oi => oi.ke.DefaultIfEmpty(),
           (k, ke) => new
           {
               k.k.DocId,
               k.k.Userid,
               k.k.Cls,
               k.k.AccDpt,
               k.k.SentDate,
               k.k.AssetNo,
               k.k.Cname,
               k.k.Cost,
               k.k.EndDate,
               k.k.CloseDate,
               k.k.Result,
               k.k.InOut,
               k.k.Cycle,
               k.k.Type,
               k.k.CustNam,
               Uid = ke.KeepEng != null ? ke.KeepEng : 0
           })
            .Join(db.AppUsers, k => k.Userid, u => u.UserId,
            (k, u) => new
            {
                k.DocId,
                k.Userid,
                k.Cls,
                k.AccDpt,
                k.SentDate,
                k.AssetNo,
                k.Cname,
                k.Cost,
                k.EndDate,
                k.CloseDate,
                k.Result,
                k.InOut,
                k.Cycle,
                k.Type,
                k.CustNam,
                //k.PartNo,
                //k.PartNam,
                //k.Qty,
                //k.Price,
                //k.TotalCost,               
                //k.UserId,
                k.Uid,
                ClsEmp = u.FullName
            }).AsEnumerable()
           .Join(db.AppUsers, k => k.Uid, u => u.UserId,
           (k, u) => new UnSignListVModel
           {
               DocTyp = "保養",
               DocId = k.DocId,
               AccDpt = k.AccDpt,
               AccDptNam = k.CustNam,
               AssetNo = k.AssetNo,
               AssetNam = k.Cname,
               Type = k.Type,
               ApplyDate = k.SentDate.Value,
               EndDate = k.EndDate,
               DealDes = k.Cycle.ToString(),
               DealState = k.Result,
               InOut = k.InOut,
               EngNam = u.FullName,
               ClsEmp = k.ClsEmp
           }).ToList(); */
            //
            foreach (UnSignListVModel s in sv2)
            {
                switch (s.DealState)
                {
                    case "1":
                        s.DealState = "功能正常";
                        break;
                    case "2":
                        s.DealState = "預防處理";
                        break;
                    case "3":
                        s.DealState = "異常處理";
                        break;
                    case "4":
                        s.DealState = "維修時保養";
                        break;
                    case "5":
                        s.DealState = "退件";
                        break;
                }
                switch (s.InOut)
                {
                    case "0":
                        s.InOut = "自行";
                        break;
                    case "1":
                        s.InOut = "委外";
                        break;
                    case "2":
                        s.InOut = "租賃";
                        break;
                    case "3":
                        s.InOut = "保固";
                        break;
                    case "4":
                        s.InOut = "借用";
                        break;
                }
                s.TroubleDes = db.BMEDKeeps.Find(s.DocId).Cycle.ToString();
                s.FailFactor = "";
                KeepEmpModel kp = db.BMEDKeepEmps.Where(p => p.DocId == s.DocId)
                    .FirstOrDefault();
                if (kp != null)
                {
                    s.EngNam = db.AppUsers.Find(kp.UserId).FullName;
                }
                List<KeepCostModel> lk = db.BMEDKeepCosts.Where(r => r.DocId == s.DocId).ToList();
                if (lk != null)
                    s.Cost = lk.Sum(r => r.TotalCost);
            }
            sv.AddRange(sv2);
            if (!String.IsNullOrEmpty(cls))
                sv = sv.Where(s => s.AssetClass == cls).ToList();
            return sv;
        }
    }
    //public class UnSignListVModel
    //{
    //    [Display(Name = "類別")]
    //    public string DocTyp { get; set; }
    //    [Display(Name = "表單編號")]
    //    public string DocId { get; set; }
    //    [Display(Name = "送單日期")]
    //    public DateTime ApplyDate { get; set; }
    //    [Display(Name = "完工日期")]
    //    public DateTime? EndDate { get; set; }
    //    [Display(Name = "財產編號")]
    //    public string AssetNo { get; set; }
    //    [Display(Name = "設備名稱")]
    //    public string AssetNam { get; set; }
    //    [Display(Name = "型號")]
    //    public string Type { get; set; }
    //    [Display(Name = "成本中心")]
    //    public string AccDpt { get; set; }
    //    [Display(Name = "成本中心名稱")]
    //    public string AccDptNam { get; set; }
    //    [Display(Name = "故障描述/保養週期")]
    //    public string TroubleDes { get; set; }
    //    [Display(Name = "故障原因")]
    //    public string FailFactor { get; set; }
    //    [Display(Name = "處理狀況/保養結果")]
    //    public string DealState { get; set; }
    //    [Display(Name = "保養方式")]
    //    public string InOut { get; set; }
    //    [Display(Name = "處理描述")]
    //    public string DealDes { get; set; }
    //    [Display(Name = "費用")]
    //    public decimal? Cost { get; set; }
    //    public Int32? EngId { get; set; }
    //    [Display(Name = "工程師")]
    //    public string EngNam { get; set; }
    //    [Display(Name = "現在關卡")]
    //    public string ClsEmp { get; set; }
    //    public string GroupId { get; set; }

    //    BMEDcontext db = new BMEDcontext();

    //    public List<UnSignListVModel> GetList(string gid, DateTime sdate, DateTime edate)
    //    {
    //        List<UnSignListVModel> sv = new List<UnSignListVModel>();
    //        List<UnSignListVModel> sv2 = new List<UnSignListVModel>();
    //        /*
    //        sv = db.RepairFlows.Where(f => f.Status == "?")
    //        .Join(db.Repairs, rd => rd.DocId, k => k.DocId,
    //        (rd, k) => new
    //        {
    //            rd.Userid,
    //            rd.Cls,
    //            k.DocId,
    //            k.AccDpt,
    //            k.ApplyDate,
    //            k.AssetNo,
    //            k.TroubleDes
    //        }).Where(k => k.ApplyDate >= sdate && k.ApplyDate <= edate)
    //        .Join(db.RepairDtls, f => f.DocId, rd => rd.DocId,
    //        (f, rd) => new
    //        {
    //            f.DocId,
    //            f.Userid,
    //            f.Cls,
    //            f.AccDpt,
    //            f.ApplyDate,
    //            f.AssetNo,
    //            f.TroubleDes,
    //            rd.Cost,
    //            rd.EndDate,
    //            rd.CloseDate,
    //            rd.FailFactor,
    //            rd.DealDes,
    //            rd.DealState
    //        })
    //        .Join(db.Assets, k => k.AssetNo, at => at.AssetNo,
    //        (k, at) => new
    //        {
    //            k.DocId,
    //            k.Userid,
    //            k.Cls,
    //            k.AccDpt,
    //            k.ApplyDate,
    //            k.AssetNo,
    //            at.Cname,
    //            k.Cost,
    //            k.EndDate,
    //            k.CloseDate,
    //            k.TroubleDes,
    //            k.FailFactor,
    //            k.DealDes,
    //            k.DealState,
    //            at.Type
    //        })
    //         .Join(db.CustOrgans, k => k.AccDpt, c => c.CustId,
    //        (k, c) => new
    //        {
    //            k.DocId,
    //            k.Userid,
    //            k.Cls,
    //            k.AccDpt,
    //            k.ApplyDate,
    //            k.AssetNo,
    //            k.Cname,
    //            k.Cost,
    //            k.EndDate,
    //            k.CloseDate,
    //            k.TroubleDes,
    //            k.FailFactor,
    //            k.DealDes,
    //            k.DealState,
    //            k.Type,
    //            c.GroupId,
    //            c.CustNam
    //        })
    //        .GroupJoin(db.RepairCosts, k => k.DocId, c => c.DocId,
    //        (k, c) => new { k, c })
    //        .SelectMany(cc => cc.c.DefaultIfEmpty(),
    //        (k, c) => new
    //        {
    //            k.k.DocId,
    //            k.k.Userid,
    //            k.k.Cls,
    //            k.k.AccDpt,
    //            k.k.ApplyDate,
    //            k.k.AssetNo,
    //            k.k.Cname,
    //            k.k.Cost,
    //            k.k.EndDate,
    //            k.k.CloseDate,
    //            k.k.TroubleDes,
    //            k.k.FailFactor,
    //            k.k.DealDes,
    //            k.k.DealState,
    //            k.k.Type,
    //            k.k.GroupId,
    //            k.k.CustNam,
    //            CostSum = k.c.Sum(p => p.TotalCost)
    //        })
    //        .GroupJoin(db.RepairEmps, k => k.DocId, c => c.DocId,
    //        (k, c) => new { k, c })
    //        .SelectMany(cc => cc.c.DefaultIfEmpty(),
    //        (k, c) => new
    //        {
    //            k.k.DocId,
    //            k.k.Userid,
    //            k.k.Cls,
    //            k.k.AccDpt,
    //            k.k.ApplyDate,
    //            k.k.AssetNo,
    //            k.k.Cname,
    //            k.k.Cost,
    //            k.k.EndDate,
    //            k.k.CloseDate,
    //            k.k.TroubleDes,
    //            k.k.FailFactor,
    //            k.k.DealDes,
    //            k.k.DealState,
    //            k.k.Type,
    //            k.k.GroupId,
    //            k.k.CustNam,
    //            k.k.CostSum,
    //            EngId = k.c.Count() > 0 ? k.c.FirstOrDefault().UserId : 0
    //        })
    //        .GroupJoin(db.AppUsers, k => k.EngId, c => c.UserId,
    //        (k, c) => new { k, c })
    //        .SelectMany(cc => cc.c.DefaultIfEmpty(),
    //        (k, c) => new
    //        {
    //            k.k.DocId,
    //            k.k.Userid,
    //            k.k.Cls,
    //            k.k.AccDpt,
    //            k.k.ApplyDate,
    //            k.k.AssetNo,
    //            k.k.Cname,
    //            k.k.Cost,
    //            k.k.EndDate,
    //            k.k.CloseDate,
    //            k.k.TroubleDes,
    //            k.k.FailFactor,
    //            k.k.DealDes,
    //            k.k.DealState,
    //            k.k.Type,
    //            k.k.GroupId,
    //            k.k.CustNam,
    //            k.k.CostSum,
    //            k.k.EngId,
    //            EngNam = c.FullName != null ? c.FullName : ""
    //        })
    //        .Join(db.AppUsers, k => k.Userid, u => u.UserId,
    //        (k, u) => new UnSignListVModel
    //        {
    //            DocTyp = "請修",
    //            DocId = k.DocId,
    //            AccDpt = k.AccDpt,
    //            AccDptNam = k.CustNam,
    //            AssetNo = k.AssetNo,
    //            AssetNam = k.Cname,
    //            Type = k.Type,
    //            ApplyDate = k.ApplyDate.Value,
    //            EndDate = k.EndDate,
    //            TroubleDes = k.TroubleDes,
    //            FailFactor = k.FailFactor,
    //            DealDes = k.DealDes,
    //            DealState = k.DealState,
    //            EngId = k.EngId,
    //            EngNam = k.EngNam,
    //            ClsEmp = u.FullName,
    //            GroupId = k.GroupId,
    //            Cost = k.CostSum
    //        }).ToList(); */
    //        //
    //        /*
    //        foreach (UnSignListVModel s in sv)
    //        {
    //            RepairEmp kp = db.RepairEmps.Where(p => p.DocId == s.DocId)
    //               .FirstOrDefault();
    //            if (kp != null)
    //            {
    //                s.EngNam = db.AppUsers.Find(kp.UserId).FullName;
    //            }
    //            List<RepairCost> lk = db.RepairCosts.Where(r => r.DocId == s.DocId).ToList();
    //            if (lk != null)
    //                s.Cost = lk.Sum(r => r.TotalCost);
    //        }*/

    //        int i = sv.Count();
    //        //保養
    //        string str = "";
    //        str += "SELECT '保養' AS DOCTYP,B.DocId,B.SENTDATE AS APPLYDATE,C.ENDDATE,B.ASSETNO, B.ASSETNAM,S.TYPE,";
    //        str += "B.ACCDPT,E.CUSTNAM AS ACCDPTNAM,'' AS TROUBLEDES,'' AS FAILFACTOR, C.RESULT AS DEALSTATE,";
    //        str += "C.INOUT,'' AS DEALDES,Q.COST,R.USERID AS ENGID,R.FULLNAME AS ENGNAM,D.FULLNAME AS CLSEMP,";
    //        str += "E.GROUPID ";
    //        str += "FROM KEEPFLOW AS A JOIN KEEP AS B ON A.DocId = B.DocId ";
    //        str += "JOIN KEEPDTL AS C ON B.DocId = C.DocId ";
    //        str += "LEFT JOIN AppUser AS D ON A.USERID = D.USERID ";
    //        str += "LEFT JOIN CUSTORGANS AS E ON B.ACCDPT = E.CUSTID ";
    //        str += "LEFT JOIN ASSET AS S ON B.ASSETNO = S.ASSETNO ";
    //        str += "LEFT JOIN (SELECT * FROM (";
    //        str += "SELECT DocId,USERID,ROW_NUMBER() OVER(PARTITION BY DocId ";
    //        str += "ORDER BY USERID) AS RID ";
    //        str += "FROM KEEPEMP) AS P WHERE P.RID = 1) AS P ON B.DocId = P.DocId ";
    //        str += "LEFT JOIN AppUser AS R ON P.UserId = R.USERID ";
    //        str += "LEFT JOIN (SELECT DocId, SUM(TOTALCOST) AS COST FROM KEEPCOST GROUP BY DocId) AS Q ";
    //        str += "ON B.DocId = Q.DocId ";
    //        str += "WHERE A.STATUS = '?' ";
    //        str += "AND (B.SENTDATE BETWEEN '2015/1/1' AND '2015/12/31') ";
    //        str += "AND E.GROUPID = 'YSH' ";
    //        //sv2 = db.Database.SqlQuery<UnSignListVModel>(str).ToList();
    //        sv2 = db.KeepFlows.Where(f => f.Status == "?")
    //        .Join(db.Keeps, rd => rd.DocId, k => k.DocId,
    //       (rd, k) => new
    //       {
    //           rd.DocId,
    //           rd.Userid,
    //           rd.Cls,
    //           k.AccDpt,
    //           k.SentDate,
    //           k.AssetNo,
    //           k.Cycle
    //       }).Where(k => k.SentDate >= sdate && k.SentDate <= edate)
    //        .Join(db.KeepDtls, f => f.DocId, rd => rd.DocId,
    //        (f, rd) => new
    //        {
    //            f.DocId,
    //            f.Userid,
    //            f.Cls,
    //            f.AccDpt,
    //            f.SentDate,
    //            f.AssetNo,
    //            f.Cycle,
    //            rd.Cost,
    //            rd.EndDate,
    //            rd.CloseDate,
    //            rd.Result,
    //            rd.InOut
    //        })
    //       .Join(db.Assets, k => k.AssetNo, at => at.AssetNo,
    //       (k, at) => new
    //       {
    //           k.DocId,
    //           k.Userid,
    //           k.Cls,
    //           k.AccDpt,
    //           k.SentDate,
    //           k.AssetNo,
    //           at.Cname,
    //           k.Cost,
    //           k.EndDate,
    //           k.CloseDate,
    //           k.Result,
    //           k.InOut,
    //           k.Cycle,
    //           at.Type
    //       })
    //       .Join(db.CustOrgans, k => k.AccDpt, c => c.CustId,
    //       (k, c) => new
    //       {
    //           k.DocId,
    //           k.Userid,
    //           k.Cls,
    //           k.AccDpt,
    //           k.SentDate,
    //           k.AssetNo,
    //           k.Cname,
    //           k.Cost,
    //           k.EndDate,
    //           k.CloseDate,
    //           k.Result,
    //           k.InOut,
    //           k.Cycle,
    //           k.Type,
    //           c.GroupId,
    //           c.CustNam
    //       })
    //       .Join(db.AssetKeeps.DefaultIfEmpty(), k => k.AssetNo, ke => ke.AssetNo,
    //       (k, ke) => new
    //       {
    //           k.DocId,
    //           k.Userid,
    //           k.Cls,
    //           k.AccDpt,
    //           k.SentDate,
    //           k.AssetNo,
    //           k.Cname,
    //           k.Cost,
    //           k.EndDate,
    //           k.CloseDate,
    //           k.Result,
    //           k.InOut,
    //           k.Cycle,
    //           k.Type,
    //           k.CustNam,
    //           k.GroupId,
    //           Uid = ke.KeepEng != null ? ke.KeepEng : 0
    //       })
    //       .GroupJoin(db.KeepCosts, k => k.DocId, c => c.DocId,
    //        (k, c) => new { k, c })
    //        .SelectMany(cc => cc.c.DefaultIfEmpty(),
    //        (k, c) => new
    //        {
    //            k.k.DocId,
    //            k.k.Userid,
    //            k.k.Cls,
    //            k.k.AccDpt,
    //            k.k.SentDate,
    //            k.k.AssetNo,
    //            k.k.Cname,
    //            k.k.Cost,
    //            k.k.EndDate,
    //            k.k.CloseDate,
    //            k.k.Result,
    //            k.k.InOut,
    //            k.k.Cycle,
    //            k.k.Type,
    //            k.k.CustNam,
    //            k.k.GroupId,
    //            k.k.Uid,
    //            CostSum = k.c.Sum(p => p.TotalCost)
    //        })
    //        .GroupJoin(db.KeepEmps, k => k.DocId, c => c.DocId,
    //        (k, c) => new { k, c })
    //        .SelectMany(cc => cc.c.DefaultIfEmpty(),
    //        (k, c) => new
    //        {
    //            k.k.DocId,
    //            k.k.Userid,
    //            k.k.Cls,
    //            k.k.AccDpt,
    //            k.k.SentDate,
    //            k.k.AssetNo,
    //            k.k.Cname,
    //            k.k.Cost,
    //            k.k.EndDate,
    //            k.k.CloseDate,
    //            k.k.Result,
    //            k.k.InOut,
    //            k.k.Cycle,
    //            k.k.Type,
    //            k.k.CustNam,
    //            k.k.GroupId,
    //            k.k.Uid,
    //            k.k.CostSum,
    //            EngId = k.c.Count() > 0 ? k.c.FirstOrDefault().UserId : 0
    //        })
    //        .GroupJoin(db.AppUsers, k => k.EngId, c => c.UserId,
    //        (k, c) => new { k, c })
    //        .SelectMany(cc => cc.c.DefaultIfEmpty(),
    //        (k, c) => new
    //        {
    //            k.k.DocId,
    //            k.k.Userid,
    //            k.k.Cls,
    //            k.k.AccDpt,
    //            k.k.SentDate,
    //            k.k.AssetNo,
    //            k.k.Cname,
    //            k.k.Cost,
    //            k.k.EndDate,
    //            k.k.CloseDate,
    //            k.k.Result,
    //            k.k.InOut,
    //            k.k.Cycle,
    //            k.k.Type,
    //            k.k.CustNam,
    //            k.k.GroupId,
    //            k.k.Uid,
    //            k.k.CostSum,
    //            k.k.EngId,
    //            EngNam = c.FullName != null ? c.FullName : ""
    //        })
    //        .Join(db.AppUsers, k => k.Userid, u => u.UserId,
    //        (k, u) => new
    //        {
    //            k.DocId,
    //            k.Userid,
    //            k.Cls,
    //            k.AccDpt,
    //            k.SentDate,
    //            k.AssetNo,
    //            k.Cname,
    //            k.Cost,
    //            k.EndDate,
    //            k.CloseDate,
    //            k.Result,
    //            k.InOut,
    //            k.Cycle,
    //            k.Type,
    //            k.CustNam,
    //            k.Uid,
    //            k.GroupId,
    //            k.EngId,
    //            k.EngNam,
    //            k.CostSum,
    //            ClsEmp = u.FullName
    //        })
    //       .Join(db.AppUsers, k => k.Uid, u => u.UserId,
    //       (k, u) => new UnSignListVModel
    //       {
    //           DocTyp = "保養",
    //           DocId = k.DocId,
    //           AccDpt = k.AccDpt,
    //           AccDptNam = k.CustNam,
    //           TroubleDes = "",//k.Cycle.ToString(),
    //           AssetNo = k.AssetNo,
    //           AssetNam = k.Cname,
    //           Type = k.Type,
    //           ApplyDate = k.SentDate.Value,
    //           EndDate = k.EndDate,
    //           DealDes = "",//k.Cycle.ToString(),
    //           DealState = k.Result,
    //           InOut = k.InOut,
    //           EngId = k.EngId,
    //           EngNam = k.EngNam == "" ? u.FullName : k.EngNam,
    //           ClsEmp = k.ClsEmp,
    //           GroupId = k.GroupId,
    //           Cost = k.CostSum
    //       }).ToList(); 
    //        i = sv2.Count();
    //        //
    //        foreach (UnSignListVModel s in sv2)
    //        {
    //            switch (s.DealState)
    //            {
    //                case "1":
    //                    s.DealState = "功能正常";
    //                    break;
    //                case "2":
    //                    s.DealState = "預防處理";
    //                    break;
    //                case "3":
    //                    s.DealState = "異常處理";
    //                    break;
    //                case "4":
    //                    s.DealState = "維修時保養";
    //                    break;
    //                case "5":
    //                    s.DealState = "退件";
    //                    break;
    //            }
    //            switch (s.InOut)
    //            {
    //                case "0":
    //                    s.InOut = "自行";
    //                    break;
    //                case "1":
    //                    s.InOut = "委外";
    //                    break;
    //                case "2":
    //                    s.InOut = "租賃";
    //                    break;
    //                case "3":
    //                    s.InOut = "保固";
    //                    break;
    //            }
    //            s.FailFactor = "";
    //            //KeepEmp kp = db.KeepEmps.Where(p => p.DocId == s.DocId)
    //            //    .FirstOrDefault();
    //            //if (kp != null)
    //            //{
    //            //    s.EngNam = db.AppUsers.Find(kp.UserId).FullName;
    //            //}
    //            //List<KeepCost> lk = db.KeepCosts.Where(r => r.DocId == s.DocId).ToList();
    //            //if (lk != null)
    //            //    s.Cost = lk.Sum(r => r.TotalCost);
    //        }
    //        sv.AddRange(sv2);
    //        if (gid != "")
    //        {
    //            sv = sv.Where(s => s.GroupId == gid).ToList();
    //        }
    //        return sv;
    //    }
    //}
    public class RpKpStokBdVModel
    {
        [Display(Name = "類別")]
        public string DocTyp { get; set; }
        [Display(Name = "單位代號")]
        public string CustId { get; set; }
        [Display(Name = "單位名稱")]
        public string CustNam { get; set; }
        [Display(Name = "零件廠牌")]
        public string Brand { get; set; }
        [Display(Name = "料號")]
        public string StokNo { get; set; }
        [Display(Name = "零件名稱")]
        public string StokNam { get; set; }
        [Display(Name = "數量")]
        public int Amt { get; set; }
        [Display(Name = "費用")]
        public decimal Cost { get; set; }
        [Display(Name = "1000以上")]
        public decimal Up1000 { get; set; }
        [Display(Name = "1000以下")]
        public decimal Dn1000 { get; set; }

        private readonly BMEDDbContext db;
        public RpKpStokBdVModel(BMEDDbContext context)
        {
            db = context;
        }
        public RpKpStokBdVModel()
        {

        }

        public List<RpKpStokBdVModel> GetList(string gid, DateTime sdate, DateTime edate, string cls)
        {
            List<RpKpStokBdVModel> sv = new List<RpKpStokBdVModel>();
            List<RpKpStokBdVModel> sv2 = new List<RpKpStokBdVModel>();
            RpKpStokBdVModel rb;
            if (!String.IsNullOrEmpty(cls))
            {
                var scv = db.BMEDRepairDtls.Where(d => d.CloseDate >= sdate &&
                d.CloseDate <= edate)
                .Join(db.BMEDRepairs, rd => rd.DocId, r => r.DocId,
                (rd, r) => new
                {
                    rd.DocId,
                    r.AccDpt,
                    r.AssetNo
                }).Join(db.BMEDAssets, rd => rd.AssetNo, r => r.AssetNo,
                (rd, r) => new
                {
                    rd.DocId,
                    r.AccDpt,
                    r.AssetClass
                }).Where(r => r.AssetClass == cls)
                .Join(db.Departments, rd => rd.AccDpt, c => c.DptId,
                (rd, c) => new
                {
                    rd.DocId,
                    CustId = c.DptId,
                    CustNam = c.Name_C
                })
                .Join(db.BMEDRepairCosts, rd => rd.DocId, rc => rc.DocId,
                (rd, rc) => new
                {
                    DocTyp = "請修",
                    rd.CustId,
                    rd.CustNam,
                    rc.PartNo,
                    rc.TotalCost
                })
                .Union(
                db.BMEDKeepDtls.Where(d => d.CloseDate >= sdate &&
                d.CloseDate <= edate)
                .Join(db.BMEDKeeps, rd => rd.DocId, r => r.DocId,
                (rd, r) => new
                {
                    rd.DocId,
                    r.AccDpt,
                    r.AssetNo
                }).Join(db.BMEDAssets, rd => rd.AssetNo, r => r.AssetNo,
                (rd, r) => new
                {
                    rd.DocId,
                    r.AccDpt,
                    r.AssetClass
                }).Where(r => r.AssetClass == cls)
                .Join(db.Departments, rd => rd.AccDpt, c => c.DptId,
                (rd, c) => new
                {
                    rd.DocId,
                    CustId = c.DptId,
                    CustNam = c.Name_C
                })
                .Join(db.BMEDKeepCosts, rd => rd.DocId, rc => rc.DocId,
                (rd, rc) => new
                {
                    DocTyp = "保養",
                    rd.CustId,
                    rd.CustNam,
                    rc.PartNo,
                    rc.TotalCost
                })
                ).GroupBy(rd => rd.CustId).ToList();

                foreach (var a in scv)
                {
                    var scv2 = a.Where(z => z.DocTyp == "請修")
                        .Join(db.BMEDDeptStocks, x => x.PartNo, d => d.StockNo,
                        (x, d) => new
                        {
                            d.Brand,
                            x.TotalCost
                        }).GroupBy(x => x.Brand).ToList();
                    foreach (var b in scv2)
                    {
                        rb = new RpKpStokBdVModel();
                        rb.CustId = a.Key;
                        rb.CustNam = db.Departments.Find(a.Key).Name_C;
                        rb.DocTyp = "請修";
                        rb.Brand = b.Key;
                        rb.Up1000 = b.Where(x => x.TotalCost >= 1000).Sum(x => x.TotalCost);
                        rb.Dn1000 = b.Where(x => x.TotalCost < 1000).Sum(x => x.TotalCost);
                        rb.Cost = b.Sum(x => x.TotalCost);
                        sv.Add(rb);
                    }
                    //
                    scv2.Clear();
                    scv2 = a.Where(z => z.DocTyp == "保養")
                        .Join(db.BMEDDeptStocks, x => x.PartNo, d => d.StockNo,
                        (x, d) => new
                        {
                            d.Brand,
                            x.TotalCost
                        }).GroupBy(x => x.Brand).ToList();
                    foreach (var b in scv2)
                    {
                        rb = new RpKpStokBdVModel();
                        rb.CustId = a.Key;
                        rb.CustNam = db.Departments.Find(a.Key).Name_C;
                        rb.DocTyp = "保養";
                        rb.Brand = b.Key;
                        rb.Up1000 = b.Where(x => x.TotalCost >= 1000).Sum(x => x.TotalCost);
                        rb.Dn1000 = b.Where(x => x.TotalCost < 1000).Sum(x => x.TotalCost);
                        rb.Cost = b.Sum(x => x.TotalCost);
                        sv.Add(rb);
                    }
                    //sv.Add(rb);
                }
            }
            else
            {
                var scv = db.BMEDRepairDtls.Where(d => d.CloseDate >= sdate &&
                d.CloseDate <= edate)
                .Join(db.BMEDRepairs, rd => rd.DocId, r => r.DocId,
                (rd, r) => new
                {
                    rd.DocId,
                    r.AccDpt
                })
                .Join(db.Departments, rd => rd.AccDpt, c => c.DptId,
                (rd, c) => new
                {
                    rd.DocId,
                    CustId = c.DptId,
                    CustNam = c.Name_C
                })
                .Join(db.BMEDRepairCosts, rd => rd.DocId, rc => rc.DocId,
                (rd, rc) => new
                {
                    DocTyp = "請修",
                    rd.CustId,
                    rd.CustNam,
                    rc.PartNo,
                    rc.TotalCost
                })
                .Union(
                db.BMEDKeepDtls.Where(d => d.CloseDate >= sdate &&
                d.CloseDate <= edate)
                .Join(db.BMEDKeeps, rd => rd.DocId, r => r.DocId,
                (rd, r) => new
                {
                    rd.DocId,
                    r.AccDpt
                })
                .Join(db.Departments, rd => rd.AccDpt, c => c.DptId,
                (rd, c) => new
                {
                    rd.DocId,
                    CustId = c.DptId,
                    CustNam = c.Name_C
                })
                .Join(db.BMEDKeepCosts, rd => rd.DocId, rc => rc.DocId,
                (rd, rc) => new
                {
                    DocTyp = "保養",
                    rd.CustId,
                    rd.CustNam,
                    rc.PartNo,
                    rc.TotalCost
                })
                ).GroupBy(rd => rd.CustId).ToList();

                foreach (var a in scv)
                {
                    var scv2 = a.Where(z => z.DocTyp == "請修")
                        .Join(db.BMEDDeptStocks, x => x.PartNo, d => d.StockNo,
                        (x, d) => new
                        {
                            d.Brand,
                            x.TotalCost
                        }).GroupBy(x => x.Brand).ToList();
                    foreach (var b in scv2)
                    {
                        rb = new RpKpStokBdVModel();
                        rb.CustId = a.Key;
                        rb.CustNam = db.Departments.Find(a.Key).Name_C;
                        rb.DocTyp = "請修";
                        rb.Brand = b.Key;
                        rb.Up1000 = b.Where(x => x.TotalCost >= 1000).Sum(x => x.TotalCost);
                        rb.Dn1000 = b.Where(x => x.TotalCost < 1000).Sum(x => x.TotalCost);
                        rb.Cost = b.Sum(x => x.TotalCost);
                        sv.Add(rb);
                    }
                    //
                    scv2.Clear();
                    scv2 = a.Where(z => z.DocTyp == "保養")
                        .Join(db.BMEDDeptStocks, x => x.PartNo, d => d.StockNo,
                        (x, d) => new
                        {
                            d.Brand,
                            x.TotalCost
                        }).GroupBy(x => x.Brand).ToList();
                    foreach (var b in scv2)
                    {
                        rb = new RpKpStokBdVModel();
                        rb.CustId = a.Key;
                        rb.CustNam = db.Departments.Find(a.Key).Name_C;
                        rb.DocTyp = "保養";
                        rb.Brand = b.Key;
                        rb.Up1000 = b.Where(x => x.TotalCost >= 1000).Sum(x => x.TotalCost);
                        rb.Dn1000 = b.Where(x => x.TotalCost < 1000).Sum(x => x.TotalCost);
                        rb.Cost = b.Sum(x => x.TotalCost);
                        sv.Add(rb);
                    }
                    //sv.Add(rb);
                }
            }
            return sv;
        }
    }
    //儀器設備保養清單
    public class AssetKeepListVModel
    {
        [Display(Name = "財產編號")]
        public string AssetNo { get; set; }
        [Display(Name = "儀器名稱")]
        public string AssetName { get; set; }
        [Display(Name = "廠牌")]
        public string Brand { get; set; }
        [Display(Name = "規格")]
        public string Standard { get; set; }
        [Display(Name = "型號")]
        public string Type { get; set; }
        [Display(Name = "保養方式")]
        public string InOut { get; set; }
        [Display(Name = "成本中心名稱")]
        public string AccDptName { get; set; }
        [Display(Name = "存放地點")]
        public string LeaveSite { get; set; }
        [Display(Name = "保固起始日")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? WartySt { get; set; }
        [Display(Name = "保固終止日")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? WartyEd { get; set; }
        [Display(Name = "合約起始日")]
        public string Sdate { get; set; }
        [Display(Name = "合約終止日")]
        public string Edate { get; set; }
        [Display(Name = "備註")]
        public string Note { get; set; }
    }
    //成效指標
    public class EffectRatio
    {
        [Display(Name = "自行維修案件數")]
        public int TotalInRepCases { set; get; }
        [Display(Name = "七日自行維修案件數")]
        public int FiveDaysInRepCases { set; get; }
        [Display(Name = "七日自行維修率")]
        public decimal FiveInRepRatio { set; get; }
        [Display(Name = "保養案件數")]
        public int TotalKeepCases { set; get; }
        [Display(Name = "已完工保養案件數")]
        public int KpFinishedCases { set; get; }
        [Display(Name = "保養完成率")]
        public decimal KeepFinishedRatio { set; get; }
        [Required]
        [Display(Name = "查詢日期(起)")]
        public DateTime Sdate { set; get; }
        [Required]
        [Display(Name = "查詢日期(止)")]
        public DateTime Edate { set; get; }
    }
}