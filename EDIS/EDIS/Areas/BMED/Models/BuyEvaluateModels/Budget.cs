using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace EDIS.Areas.BMED.Models.BuyEvaluateModels
{
    [Table("BMEDBudgets")]
    public class BudgetModel
    {
        [Key]
        [Display(Name = "表單編號")]
        public string DocId { get; set; }
        [Display(Name = "儀器名稱")]
        public string PlantName { get; set; }
        [Display(Name = "成本中心")]
        public string AccDpt { get; set; }
        [Display(Name = "通過數量")]
        public int Amt { get; set; }
        [Display(Name = "單價")]
        [DisplayFormat(DataFormatString="{0:0}")]
        public decimal Price { get; set; }
        [Display(Name = "總價")]
        [DisplayFormat(DataFormatString = "{0:0}")]
        public decimal TotalPrice { get; set; }
        [Display(Name = "單位")]
        public string Unit { get; set; }
        [Display(Name = "規格")]
        public string Standard { get; set; }
        [Display(Name = "工程師")]
        public string EngName { get; set; }
        [Display(Name = "醫工部意見")]
        public string Opinion { get; set; }
        [Display(Name = "小組意見")]
        public string GrpOpin { get; set; }
        [Display(Name = "年度")]
        public string Year { get; set; }
        [Display(Name = "採購評估編號")]
        public string BuyId { get; set; }
        [Display(Name = "院區")]
        public string Loc { get; set; }
    }
}