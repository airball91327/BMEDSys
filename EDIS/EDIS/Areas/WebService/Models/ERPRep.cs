using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDIS.Areas.WebService.Models
{
    public class ERPRep
    {

    }

    public class ERPRepHead
    {
        public string BIL_NO { get; set; } //表單編號
        public string SAL_NO { get; set; } //工程師代號
        public DateTime PS_DD { get; set; }
        public string CUS_NO { get; set; }
        public string INV_CUS_NO { get; set; } //進貨廠商代號(有進貨廠商代號，則產生進貨單。)
        public string ISPAY { get; set; } //是否立即付款

        public ERPRepHead()
        {
            CUS_NO = "A0280"; //彰化基督教醫療財團法人彰化基督教醫院
        }

    }

    public class ERPRepBody
    {
        public int ITM { get; set; }
        public string PRD_NO { get; set; }
        public string PRD_NAME { get; set; }
        public decimal QTY { get; set; }
        public decimal UP { get; set; } //單價
        public decimal AMTN { get; set; } //未稅額
        public decimal TAX { get; set; } //稅額
        public decimal AMT { get; set; } //金額
    }
}
