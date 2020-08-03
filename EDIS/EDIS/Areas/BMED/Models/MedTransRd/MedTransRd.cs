using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EDIS.Areas.BMED.Models.MedTransRd
{
    public class MedTransRd
    {
        [Display(Name = "表單類別")]
        public string DOC_TYP { get; set; }
        [Display(Name = "表單編號")]
        public string DOCID { get; set; }
        [Display(Name = "財產編號")]
        public string ASSETNO { get; set; }
        [Display(Name = "儀器名稱")]
        public string PLANT_NAM { get; set; }
        [Display(Name = "狀態")]
        public string STATUS { get; set; }
        [Display(Name = "備註描述")]
        public string CMT { get; set; }
        [Display(Name = "傳送人員")]
        public int? TF_EMPNO { get; set; }
        [Display(Name = "傳送人員姓名")]
        public string TF_EMPNAM { get; set; }
        [Display(Name = "傳送日期")]
        public DateTime? TF_DATE { get; set; }
        [Display(Name = "記錄人員")]
        public int? RD_EMPNO { get; set; }
        [Display(Name = "記錄人員姓名")]
        public string RD_EMPNAM { get; set; }
        [Display(Name = "有效")]
        public string EFG { get; set; }
        [Display(Name = "異動人員")]
        public int? RTP { get; set; }
        [Display(Name = "異動時間")]
        public DateTime? RTT { get; set; }
        [Display(Name = "申請部門")]
        public string APPLYDPT { get; set; }
    }
}
