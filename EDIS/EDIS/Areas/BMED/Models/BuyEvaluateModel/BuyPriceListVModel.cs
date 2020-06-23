using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace EDIS.Areas.BMED.Models.DeliveryModels
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

        public BuyPriceListVModel() { }

        public List<BuyPriceListVModel> GetList(string cls = null)
        {
            List<BuyPriceListVModel> rv = new List<BuyPriceListVModel>();
            MvcMedEngMgrContext db = new MvcMedEngMgrContext();
            List<BuyFlow> rf = new List<BuyFlow>();
            List<BuyVendor> bv = new List<BuyVendor>();
            BuyEvaluate e;
            BuyPriceListVModel p;
            UserProfile u;
            CustOrgan c;
            bv = db.BuyVendors.Where(b => b.Status == "?").ToList();
            foreach (BuyVendor f in bv)
            {
                p = new BuyPriceListVModel();
                p.DocType = "評估";
                p.Docid = f.Docid;
                e = db.BuyEvaluates.Find(f.Docid);
                p.UserId = e.UserId;
                p.UserName = e.UserName;
                p.PlantCnam = e.PlantCnam;
                p.PlantEnam = e.PlantEnam;
                u = db.UserProfiles.Find(e.UserId);
                c = db.CustOrgans.Find(u.CustId);
                p.CustId = c.CustId;
                p.CustNam = c.CustNam;
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
            MvcMedEngMgrContext db = new MvcMedEngMgrContext();
            List<BuyFlow> rf = new List<BuyFlow>();
            List<BuyVendor> bv = new List<BuyVendor>();
            List<BuyVendor> bv2;
            BuyEvaluate e;
            BuyPriceListVModel p;
            UserProfile u;
            CustOrgan c;
            string str = "";
            bv = db.BuyVendors.Where(b => b.Status == "?").ToList();
            foreach (BuyVendor f in bv)
            {
                p = new BuyPriceListVModel();
                p.DocType = "評估";
                p.Docid = f.Docid;
                e = db.BuyEvaluates.Find(f.Docid);
                p.UserId = e.UserId;
                p.UserName = e.UserName;
                p.PlantCnam = e.PlantCnam;
                p.PlantEnam = e.PlantEnam;
                u = db.UserProfiles.Find(e.UserId);
                c = db.CustOrgans.Find(u.CustId);
                if (c != null)
                {
                    p.CustId = c.CustId;
                    p.CustNam = c.CustNam;
                }
                p.Amt = e.Amt;
                p.Unit = e.Unit;
                p.VendorNo = f.VendorNo;
                p.UniteNo = f.UniteNo;
                p.VendorNam = f.VendorNam;
                u = db.UserProfiles.Find(e.PurchaserId);
                if(u != null)
                {
                    p.buyer = u.FullName;
                    p.tel = u.Tel;
                    p.email = u.Email;
                }
                if (rv.Where(v => v.Docid == p.Docid).Count() == 0)
                    rv.Add(p);
                else
                {
                    str = rv.Find(v => v.Docid == p.Docid).VendorNam;
                    rv.Find(v => v.Docid == p.Docid).VendorNam = str + "<br />" + p.VendorNam;
                }
            }
            return rv;
        }
    }
}