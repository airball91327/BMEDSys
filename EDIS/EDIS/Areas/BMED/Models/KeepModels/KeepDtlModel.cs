﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDIS.Areas.BMED.Models.KeepModels
{
    [Table("BMEDKeepDtls")]
    public class KeepDtlModel
    {
        [Key]
        public string DocId { get; set; }
        [Display(Name = "保養結果")]
        public int? Result { get; set; }
        [NotMapped]
        public string ResultTitle { get; set; }
        [Display(Name = "保養描述")]
        public string Memo { get; set; }
        [Display(Name = "保養方式")]
        public string InOut { get; set; }
        [Display(Name = "[有][無]費用")]
        public string IsCharged { get; set; }
        [Display(Name = "保養工時")]
        public decimal? Hours { get; set; }
        [Display(Name = "保養費用")]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal? Cost { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "完工日期")]
        public Nullable<DateTime> EndDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "完帳日期")]
        public Nullable<DateTime> CloseDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "關帳日期")]
        public Nullable<DateTime> ShutDate { get; set; }
    }
}