using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EDIS.Areas.BMED.Models.MedTransRd
{
    public class MedTransRdQModel
    {
        [Display(Name ="類別")]
        public string DOC_TYP { get; set; }
        [Display(Name = "表單編號")]
        public string DOCID { get; set; }
        [Display(Name = "財產編號")]
        public string ASSETNO { get; set; }
        [Display(Name = "設備名稱")]
        public string PLANT_NAM { get; set; }
        [Display(Name = "成本中心")]
        public string ACCDPT { get; set; }
        [Display(Name = "申請部門")]
        public string APPLYDPT { get; set; }
        [Display(Name = "送件/取件日")]
        public DateTime? TF_DATE { get; set; }
        [Display(Name = "狀態")]
        public string STATUS { get; set; }
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public string TF_DATE_From { get; set; }
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public string TF_DATE_To { get; set; }
    }
}