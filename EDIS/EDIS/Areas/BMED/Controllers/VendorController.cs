using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EDIS.Areas.BMED.Data;
using EDIS.Areas.BMED.Models.RepairModels;
using Microsoft.AspNetCore.Authorization;
using EDIS.Models.Identity;

namespace EDIS.Areas.BMED.Controllers
{
    [Area("BMED")]
    [Authorize]
    public class VendorController : Controller
    {
        private readonly BMEDDbContext _context;

        public VendorController(BMEDDbContext context)
        {
            _context = context;
        }

        // GET: Vendor
        public async Task<IActionResult> Index()
        {
            return View(await _context.BMEDVendors.ToListAsync());
        }

        // GET: Vendor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendorModel = await _context.BMEDVendors
                .SingleOrDefaultAsync(m => m.VendorId == id);
            if (vendorModel == null)
            {
                return NotFound();
            }

            return View(vendorModel);
        }

        // GET: Vendor/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vendor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VendorId,VendorName,Address,Tel,Fax,Email,UniteNo,TaxAddress,TaxZipCode,Contact,ContactTel,ContactEmail,StartDate,EndDate,Status,Kind")] VendorModel vendorModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vendorModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vendorModel);
        }

        // GET: Vendor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendorModel = await _context.BMEDVendors.SingleOrDefaultAsync(m => m.VendorId == id);
            if (vendorModel == null)
            {
                return NotFound();
            }
            return View(vendorModel);
        }

        // POST: Vendor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VendorId,VendorName,Address,Tel,Fax,Email,UniteNo,TaxAddress,TaxZipCode,Contact,ContactTel,ContactEmail,StartDate,EndDate,Status,Kind")] VendorModel vendorModel)
        {
            if (id != vendorModel.VendorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vendorModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendorModelExists(vendorModel.VendorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vendorModel);
        }

        // GET: Vendor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendorModel = await _context.BMEDVendors
                .SingleOrDefaultAsync(m => m.VendorId == id);
            if (vendorModel == null)
            {
                return NotFound();
            }

            return View(vendorModel);
        }

        // POST: Vendor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vendorModel = await _context.BMEDVendors.SingleOrDefaultAsync(m => m.VendorId == id);
            _context.BMEDVendors.Remove(vendorModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public ActionResult QryVendor(QryVendor qryVendor)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            if (qryVendor.QryType == "關鍵字")
            {
                if (!string.IsNullOrEmpty(qryVendor.KeyWord))
                {
                    _context.BMEDVendors.Where(v => v.VendorName.Contains(qryVendor.KeyWord.Trim()))
                                        .ToList()
                                        .ForEach(v => {
                                            items.Add(new SelectListItem()
                                            {
                                                Text = v.VendorName,
                                                Value = v.VendorId.ToString()
                                            });
                                        });
                }
            }
            else if (qryVendor.QryType == "統一編號")
            {
                _context.BMEDVendors.Where(v => v.UniteNo == qryVendor.UniteNo)
                        .ToList()
                        .ForEach(v => {
                            items.Add(new SelectListItem()
                            {
                                Text = v.VendorName,
                                Value = v.VendorId.ToString()
                            });
                        });
            }

            qryVendor.VendorList = items;

            return PartialView(qryVendor);
        }

        private bool VendorModelExists(int id)
        {
            return _context.BMEDVendors.Any(e => e.VendorId == id);
        }

        public IActionResult Choose(string KeyWord = "", string UniteNo = "", string src = "ws")
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            List<VendorModel> vr = _context.BMEDVendors.ToList();
            if (KeyWord != "")
                vr = vr.Where(r => r.VendorName.Contains(KeyWord)).ToList();
            if (UniteNo != "")
                vr = vr.Where(r => r.UniteNo == UniteNo).ToList();
            foreach (VendorModel v in vr)
            {
                listItem.Add(new SelectListItem { Text = v.VendorName, Value = v.UniteNo });
            }
            ViewData["Vendors"] = new SelectList(listItem.Distinct(), "Value", "Text", "");
            return PartialView();
        }

        public IActionResult GetMembers(string uniteno = null)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            SelectListItem li;
            VendorModel v = _context.BMEDVendors.Where(vr => vr.UniteNo == uniteno && vr.Status == "Y").FirstOrDefault();
            if (v != null)
            {
                List<AppUserModel> ur = _context.AppUsers.Where(u => u.VendorId == v.VendorId).ToList();
                foreach (AppUserModel p in ur)
                {
                    li = new SelectListItem();
                    li.Text = p.FullName;
                    li.Value = p.Id.ToString();
                    list.Add(li);
                }
            }
            return Json(list);
        }

        public string IsInVendor(string uniteno = null)
        {
            List<VendorModel> vv = _context.BMEDVendors.Where(v => v.UniteNo == uniteno).ToList();
            if (vv.Count > 0)
                return "";
            return "*此廠商尚未建檔";
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}
