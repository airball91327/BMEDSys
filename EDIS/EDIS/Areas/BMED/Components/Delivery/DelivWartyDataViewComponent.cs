using EDIS.Areas.BMED.Data;
using EDIS.Models.Identity;
using EDIS.Areas.BMED.Models.RepairModels;
using EDIS.Areas.BMED.Repositories;
using EDIS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDIS.Areas.BMED.Models.DeliveryModels;
using Microsoft.EntityFrameworkCore;
using EDIS.Areas.BMED.Models.KeepModels;

namespace EDIS.Areas.BMED.Components.Delivery
{
    public class DelivWartyDataViewComponent : ViewComponent
    {
        private readonly BMEDDbContext _context;
        private readonly IRepository<AppUserModel, int> _userRepo;
        private readonly CustomUserManager userManager;

        public DelivWartyDataViewComponent(BMEDDbContext context,
                                           IRepository<AppUserModel, int> userRepo,
                                           CustomUserManager customUserManager)
        {
            _context = context;
            _userRepo = userRepo;
            userManager = customUserManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            DelivDataVModel dv = new DelivDataVModel();
            if (id != null)
            {
                DeliveryModel d = _context.Deliveries.Find(id);
                if (d != null)
                {
                    dv.DocId = d.DocId;
                    dv.WartyNt = d.WartyNt;
                    dv.AcceptDate = d.AcceptDate;
                    if (d.WartySt == null)
                        dv.WartySt = DateTime.Now;
                    else
                        dv.WartySt = d.WartySt;
                    if (d.WartyEd == null)
                    {
                        if (d.WartyMon != null)
                            dv.WartyEd = dv.WartySt.Value.AddMonths(d.WartyMon);
                        else
                            dv.WartyEd = DateTime.Now;
                    }
                    else
                        dv.WartyEd = d.WartyEd;
                    if (d.FileTestDate == null)
                        dv.FileTestDate = DateTime.Now;
                    else
                        dv.FileTestDate = d.FileTestDate;
                    dv.Stype2 = d.Stype2;
                    dv.Code = d.Code;
                    dv.TestUid = d.TestUid;
                    dv.OpenDate = d.OpenDate;
                    dv.OrderDate = d.OrderDate;
                    List<AssetModel> av = _context.BMEDAssets.Where(a => a.Docid == d.DocId).ToList();
                    List<AssetKeepModel> kv = new List<AssetKeepModel>();
                    AssetKeepModel ak;
                    foreach (AssetModel a in av)
                    {
                        ak = _context.BMEDAssetKeeps.Find(a.AssetNo);
                        if (ak != null)
                        {
                            ak.Cname = a.Cname;
                            kv.Add(ak);
                        }
                    }
                    dv.ak = kv;
                }
                List<SelectListItem> listItem = new List<SelectListItem>();
                listItem.Add(new SelectListItem { Text = "自行", Value = "自行" });
                listItem.Add(new SelectListItem { Text = "委外", Value = "委外" });
                listItem.Add(new SelectListItem { Text = "保固", Value = "保固" });
                listItem.Add(new SelectListItem { Text = "租賃", Value = "租賃" });
                ViewData["FMINOUT"] = new SelectList(listItem, "Value", "Text");
                //
                List<SelectListItem> code = new List<SelectListItem>();
                foreach (var item in _context.DelivCodes)
                {
                    code.Add(new SelectListItem { Text = item.DSC, Value = item.Code.ToString() });
                }
                ViewData["Code"] = new SelectList(code, "Value", "Text", dv.Code);
                //
                List<SelectListItem> stype2 = new List<SelectListItem>();
                stype2.Add(new SelectListItem { Text = "一般", Value = "N" });
                stype2.Add(new SelectListItem { Text = "須報備", Value = "S" });
                ViewData["Stype2"] = new SelectList(stype2, "Value", "Text", dv.Stype2);
            }
            return View(dv);
        }
    }
}
