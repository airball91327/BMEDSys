using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using EDIS.Areas.BMED.Data;
using EDIS.Areas.BMED.Models.KeepModels;
using EDIS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EDIS.Areas.BMED.Controllers
{
    [Area("BMED")]
    [Authorize]
    public class KeepFormatController : Controller
    {
        private readonly BMEDDbContext _context;

        public KeepFormatController(BMEDDbContext context)
        {
            _context = context;
        }

        // GET: BMED/KeepFormat
        public IActionResult Index()
        {
            return View(_context.BMEDKeepFormats.ToList());
        }

        // GET: BMED/KeepFormat/Details/5
        public IActionResult Details(string id = null)
        {
            KeepFormatModel keepformat = _context.BMEDKeepFormats.Find(HtmlEncoder.Default.Encode(id));
            if (keepformat == null)
            {
                return StatusCode(404);
            }
            return View(keepformat);
        }

        // GET: BMED/KeepFormat/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BMED/KeepFormat/Create
        [HttpPost]
        public IActionResult Create(KeepFormatModel keepformat)
        {
            KeepFormatModel k = _context.BMEDKeepFormats.Find(keepformat.FormatId);
            if (k != null)
            {
                ModelState.AddModelError("", "保養格式代號重複!!");
                return View(keepformat);
            }
            if (ModelState.IsValid)
            {
                _context.BMEDKeepFormats.Add(keepformat);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(keepformat);
        }

        // GET: BMED/KeepFormat/Edit/5
        public IActionResult Edit(string id = null)
        {
            KeepFormatModel keepformat = _context.BMEDKeepFormats.Find(HtmlEncoder.Default.Encode(id));
            if (keepformat == null)
            {
                return StatusCode(404);
            }
            return View(keepformat);
        }

        // POST: BMED/KeepFormat/Edit/5
        [HttpPost]
        public IActionResult Edit(KeepFormatModel keepformat)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(keepformat).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(keepformat);
        }

        // GET: BMED/KeepFormat/GetPlants
        public IActionResult GetPlants(string id = null)
        {
            if (id != null)
                return Content(_context.BMEDKeepFormats.Find(id).Plants);
            return Content("");
        }

        // GET: BMED//KeepFormat/Delete/5
        public IActionResult Delete(string id = null)
        {
            KeepFormatModel keepformat = _context.BMEDKeepFormats.Find(id);
            if (keepformat == null)
            {
                return StatusCode(404);
            }
            return View(keepformat);
        }

        // POST: BMED/KeepFormat/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(string id)
        {
            List<KeepFormatDtlModel> dtls = _context.BMEDKeepFormatDtls.Where(d => d.FormatId == id).ToList();
            _context.BMEDKeepFormatDtls.RemoveRange(dtls);
            //
            KeepFormatModel keepformat = _context.BMEDKeepFormats.Find(id);
            _context.BMEDKeepFormats.Remove(keepformat);
            //
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}