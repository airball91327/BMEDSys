using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using EDIS.Models.Identity;
using EDIS.Areas.BMED.Data;

namespace EDIS.Areas.BMED.Models.BuyEvaluateModels
{
    public class BuyPriceListVModel
    {
        [Display(Name = "類別")]
        public string DocType { get; set; }
        [Display(Name = "表單編號")]
        public string DocId { get; set; }
        [Display(Name = "申請人代號")]
        public int UserId { get; set; }
        [Display(Name = "申請人姓名")]
        public string UserName { get; set; }
        [Display(Name = "機構代號")]
        public string CustId { get; set; }
        [Display(Name = "機構名稱")]
        public string CustNam { get; set; }
        [Display(Name = "設備類別")]
        public string PlantType { get; set; }
        [Display(Name = "設備名稱(中文)")]
        public string PlantCnam { get; set; }
        [Display(Name = "設備名稱(英文)")]
        public string PlantEnam { get; set; }
        [Display(Name = "數量")]
        public int Amt { get; set; }
        [Display(Name = "單位")]
        public string Unit { get; set; }
        [Display(Name = "廠商")]
        public string VendorNo { get; set; }
        [Display(Name = "廠商名稱")]
        public string VendorNam { get; set; }
        public string UniteNo { get; set; }
        [Display(Name = "採購人員")]
        public string buyer { get; set; }
        public string tel { get; set; }
        public string email { get; set; }

        private readonly BMEDDbContext db;
        public BuyPriceListVModel(BMEDDbContext context)
        {
            db = context;
        }
        public BuyPriceListVModel()
        {

        }

        public List<BuyPriceListVModel> GetList(string cls = null)
        {
            List<BuyPriceListVModel> rv = new List<BuyPriceListVModel>();
            List<BuyFlowModel> rf = new List<BuyFlowModel>();
            List<BuyVendorModel> bv = new List<BuyVendorModel>();
            BuyEvaluateModel e;
            BuyPriceListVModel p;
            AppUserModel u;
            DepartmentModel c;
            bv = db.BuyVendors.Where(b => b.Status == "?").ToList();
            foreach (BuyVendorModel f in bv)
            {
                p = new BuyPriceListVModel();
                p.DocType = "評估";
                p.DocId = f.DocId;
                e = db.BuyEvaluates.Find(f.DocId);
                p.UserId = e.UserId;
                p.UserName = e.UserName;
                p.PlantCnam = e.PlantCnam;
                p.PlantEnam = e.PlantEnam;
                u = db.AppUsers.Find(e.UserId);
                c = db.Departments.Find(u.DptId);
                p.CustId = c.DptId;
                p.CustNam = c.Name_C;
                p.Amt = e.Amt;
                p.Unit = e.Unit;
                p.VendorNo = f.VendorNo;
                p.UniteNo = f.UniteNo;
                p.VendorNam = f.VendorNam;
                rv.Add(p);
            }
            return rv;
        }
        public List<BuyPriceListVModel> GetHomeList(string cls = null)
        {
            List<BuyPriceListVModel> rv = new List<BuyPriceListVModel>();
            List<BuyFlowModel> rf = new List<BuyFlowModel>();
            List<BuyVendorModel> bv = new List<BuyVendorModel>();
            List<BuyVendorModel> bv2;
            BuyEvaluateModel e;
            BuyPriceListVModel p;
            AppUserModel u;
            DepartmentModel c;
            string str = "";
            bv = db.BuyVendors.Where(b => b.Status == "?").ToList();
            foreach (BuyVendorModel f in bv)
            {
                p = new BuyPriceListVModel();
                p.DocType = "評估";
                p.DocId = f.DocId;
                e = db.BuyEvaluates.Find(f.DocId);
                p.UserId = e.UserId;
                p.UserName = e.UserName;
                p.PlantCnam = e.PlantCnam;
                p.PlantEnam = e.PlantEnam;
                u = db.AppUsers.Find(e.UserId);
                c = db.Departments.Find(u.DptId);
                if (c != null)
                {
                    p.CustId = c.DptId;
                    p.CustNam = c.Name_C;
                }
                p.Amt = e.Amt;
                p.Unit = e.Unit;
                p.VendorNo = f.VendorNo;
                p.UniteNo = f.UniteNo;
                p.VendorNam = f.VendorNam;
                u = db.AppUsers.Find(e.PurchaserId);
                if (u != null)
                {
                    p.buyer = u.FullName;
                    p.tel = u.Mobile;
                    p.email = u.Email;
                }
                if (rv.Where(v => v.DocId == p.DocId).Count() == 0)
                    rv.Add(p);
                else
                {
                    str = rv.Find(v => v.DocId == p.DocId).VendorNam;
                    rv.Find(v => v.DocId == p.DocId).VendorNam = str + "<br />" + p.VendorNam;
                }
            }
            return rv;
        }
    }
}