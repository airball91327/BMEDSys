using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;


namespace EDIS.Areas.BMED.Models.BuyEvaluateModels
{
    public class BuyEvaluateListVModel
    {
        [Display(Name = "類別")]
        public string DocType { get; set; }
        [Display(Name = "表單編號")]
        public string DocId { get; set; }
        [Display(Name = "預算編號")]
        public string BudgetId { get; set; }
        [Display(Name = "會簽表單編號")]
        public string DocSid { get; set; }
        [Display(Name = "申請人代號")]
        public int UserId { get; set; }
        [Display(Name = "申請人姓名")]
        public string UserName { get; set; }
        [Display(Name = "機構代號")]
        public string CustId { get; set; }
        [Display(Name = "成本中心")]
        public string CustNam { get; set; }
        [Display(Name = "設備類別")]
        public string PlantType { get; set; }
        [Display(Name = "設備名稱(中文)")]
        public string PlantCnam { get; set; }
        [Display(Name = "設備名稱(英文)")]
        public string PlantEnam { get; set; }
        [Display(Name = "工程師")]
        public string EngName { get; set; }
        [Display(Name = "天數")]
        public decimal Days { get; set; }
        public string Flg { get; set; }
        public int FlowUid { get; set; }
        [Display(Name = "目前關卡")]
        public string FlowUname { get; set; }
        [Display(Name = "確定規格")]
        public bool IsAgree { get; set; }
        [Display(Name = "規格日期")]
        public string AgreeDate { get; set; }
        [Display(Name = "驗收案數目")]
        public int DelivCount { get; set; }
        public BuyEvaluateListVModel() { }

    }
}