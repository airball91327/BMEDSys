﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDIS.Areas.BMED.Models.KeepModels
{
    [Table("BMEDKeeps")]
    public class KeepModel
    {
        [Key]
        [Display(Name = "表單編號")]
        public string DocId { get; set; }
        [Display(Name = "申請人代號")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "必填寫欄位")]
        [Display(Name = "申請人姓名")]
        public string UserName { get; set; }
        [NotMapped]
        [Display(Name = "申請人帳號")]
        public string UserAccount { get; set; }
        [Required(ErrorMessage = "必填寫欄位")]
        [Display(Name = "所屬部門")]
        public string DptId { get; set; }
        [Display(Name = "所屬單位")]
        public string Company { get; set; }
        [NotMapped]
        [DataType(DataType.EmailAddress)]
        [Required]
        [Display(Name = "電子信箱")]
        public string Email { get; set; }
        [Required(ErrorMessage = "必填寫欄位")]
        [Display(Name = "分機")]
        public string Ext { get; set; }
        [Display(Name = "MVPN")]
        public string Mvpn { get; set; }
        [Required(ErrorMessage = "必填寫欄位")]
        [Display(Name = "成本中心")]
        public string AccDpt { get; set; }
        [NotMapped]
        public string AccDptName { get; set; }
        [Required(ErrorMessage = "必填寫欄位")]
        [Display(Name = "保養週期")]
        public int Cycle { get; set; }
        [Required(ErrorMessage = "必填寫欄位")]
        [Display(Name = "財產編號")]
        public string AssetNo { get; set; }
        [Required(ErrorMessage = "必填寫欄位")]
        [Display(Name = "儀器名稱")]
        public string AssetName { get; set; }
        [Display(Name = "放置地點")]
        public string PlaceLoc { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [Display(Name = "送單日期")]
        public DateTime? SentDate { get; set; }
        public string Src { get; set; }
    }
}