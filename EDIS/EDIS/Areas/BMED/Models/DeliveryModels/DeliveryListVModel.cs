using EDIS.Areas.BMED.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EDIS.Areas.BMED.Models.DeliveryModels
{
    public class DeliveryListVModel
    {
        [Display(Name = "類別")]
        public string DocType { get; set; }
        [Display(Name = "表單編號")]
        public string DocId { get; set; }
        [Display(Name = "申請人代號")]
        public int UserId { get; set; }
        [Display(Name = "申請人姓名")]
        public string UserName { get; set; }
        [Display(Name = "申請人分機")]
        public string Contact { get; set; }
        [Display(Name = "申請部門代號")]
        public string Company { get; set; }
        [Display(Name = "申請部門")]
        public string CompanyNam { get; set; }
        [Display(Name = "成本中心")]
        public string AccDpt { get; set; }
        [Display(Name = "成本中心名稱")]
        public string AccDptNam { get; set; }
        [Display(Name = "合約號碼")]
        public string ContractNo { get; set; }
        [Display(Name = "採購案號")]
        public string PurchaseNo { get; set; }
        [Display(Name = "列管編號")]
        public string CrlItemNo { get; set; }
        [Display(Name = "預算編號")]
        public string BudgetId { get; set; }
        [Display(Name = "天數")]
        public int? Days { get; set; }
        public string Flg { get; set; }
        public int FlowUid { get; set; }
        [Display(Name = "目前關卡處理人")]
        public string FlowUname { get; set; }
        public string VendorNo { get; set; }
        
    }
}
