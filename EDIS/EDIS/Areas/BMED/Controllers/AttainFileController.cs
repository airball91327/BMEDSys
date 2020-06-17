using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EDIS.Areas.BMED.Data;
using EDIS.Areas.BMED.Models.RepairModels;
using System.IO;
using EDIS.Repositories;
using EDIS.Models.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Data.SqlClient;

namespace EDIS.Areas.BMED.Controllers
{
    [Area("BMED")]
    [Authorize]
    public class AttainFileController : Controller
    {
        private readonly BMEDDbContext _context;

        private readonly IRepository<AppUserModel, int> _userRepo;
        private readonly IHostingEnvironment _hostingEnvironment;

        public AttainFileController(BMEDDbContext context,
                                    IRepository<AppUserModel, int> userRepo,
                                    IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _userRepo = userRepo;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: AttainFile
        public async Task<IActionResult> Index()
        {
            return View(await _context.BMEDAttainFiles.ToListAsync());
        }

        [HttpPost]
        public IActionResult List(string docid = null, string doctyp = null)
        {
            return ViewComponent("BMEDAttainFileList", new { id = docid, typ = doctyp, viewType="Edit" });
        }

        // Called by Ajax Upload method.
        [HttpPost]
        public IActionResult List3(string docid = null, string doctyp = null)
        {
            return ViewComponent("BMEDAttainFileList", new { id = docid, typ = doctyp, viewType = "AjaxView" });
        }

        [HttpPost]
        public async Task<IActionResult> Upload(AttainFileModel attainFile)
        {
            var ur = _userRepo.Find(u => u.UserName == this.User.Identity.Name).FirstOrDefault();
            long size = attainFile.Files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            if (ModelState.IsValid)
            {
                try
                {

                    string s = "/Files/BMED";
//#if DEBUG
//                    s = "/App_Data";
//#endif
                    switch (attainFile.DocType)
                    {
                        case "0":
                            s += "/Budget/";
                            break;
                        case "1":
                            s += "/Repair/";
                            break;
                        case "2":
                            s += "/Keep/";
                            break;
                        case "3":
                            s += "/BuyEvaluate/";
                            break;
                        case "4":
                            s += "/Delivery/";
                            break;
                        case "5":
                            s += "/Asset/";
                            break;
                    }
                    var i = _context.BMEDAttainFiles
                                    .Where(a => a.DocType == attainFile.DocType)
                                    .Where(a => a.DocId == attainFile.DocId).ToList();
                    attainFile.SeqNo = i.Count == 0 ? 1 : i.Select(a => a.SeqNo).Max() + 1;

                    string WebRootPath = _hostingEnvironment.WebRootPath;
                    string path = Path.Combine(WebRootPath + s + attainFile.DocId + "_"
                    + attainFile.SeqNo.ToString() + Path.GetExtension(attainFile.Files[0].FileName));
                    // Upload files.
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await attainFile.Files[0].CopyToAsync(stream);
                    }

                    // Save file details to AttainFiles table.
                    string filelink = attainFile.DocId + "_"
                    + attainFile.SeqNo.ToString() + Path.GetExtension(attainFile.Files[0].FileName);
                    switch (attainFile.DocType)
                    {
                        case "0":
                            attainFile.FileLink = "Budget/" + filelink;
                            break;
                        case "1":
                            attainFile.FileLink = "Repair/" + filelink;
                            break;
                        case "2":
                            attainFile.FileLink = "Keep/" + filelink;
                            break;
                        case "3":
                            attainFile.FileLink = "BuyEvaluate/" + filelink;
                            break;
                        case "4":
                            attainFile.FileLink = "Delivery/" + filelink;
                            break;
                        case "5":
                            attainFile.FileLink = "Asset/" + filelink;
                            break;
                    }
                    attainFile.Rtt = DateTime.Now;
                    attainFile.Rtp = ur.Id;
                    _context.BMEDAttainFiles.Add(attainFile);
                    _context.SaveChanges();

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            else
            {
                string msg = "";
                foreach (var error in ViewData.ModelState.Values.SelectMany(modelState => modelState.Errors))
                {
                    msg += error.ErrorMessage + Environment.NewLine;
                }
                throw new Exception(msg);
            }

            TempData["SendMsg"] = "上傳成功";
            if(attainFile.DocType == "2")
            {
                return RedirectToAction("Edit", "Keep", new { area = "BMED", id = attainFile.DocId });
            }
            return RedirectToAction("Edit", "Repair", new { area = "BMED", id = attainFile.DocId });

            //return new JsonResult(attainFile)
            //{
            //    Value = new { success = true, error = "" },
            //};
        }

        // Called by Ajax Upload method.
        [HttpPost]
        public async Task<IActionResult> Upload3(AttainFileModel attainFile)
        {
            var ur = _userRepo.Find(u => u.UserName == this.User.Identity.Name).FirstOrDefault();
            long size = attainFile.Files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            if (ModelState.IsValid)
            {
                try
                {

                    string s = "/Files/BMED";
                    //#if DEBUG
                    //                    s = "/App_Data";
                    //#endif
                    switch (attainFile.DocType)
                    {
                        case "0":
                            s += "/Budget/";
                            break;
                        case "1":
                            s += "/Repair/";
                            break;
                        case "2":
                            s += "/Keep/";
                            break;
                        case "3":
                            s += "/BuyEvaluate/";
                            break;
                        case "4":
                            s += "/Delivery/";
                            break;
                        case "5":
                            s += "/Asset/";
                            break;
                    }
                    var i = _context.BMEDAttainFiles
                                    .Where(a => a.DocType == attainFile.DocType)
                                    .Where(a => a.DocId == attainFile.DocId).ToList();
                    attainFile.SeqNo = i.Count == 0 ? 1 : i.Select(a => a.SeqNo).Max() + 1;

                    string WebRootPath = _hostingEnvironment.WebRootPath;
                    string path = Path.Combine(WebRootPath + s + attainFile.DocId + "_"
                    + attainFile.SeqNo.ToString() + Path.GetExtension(attainFile.Files[0].FileName));
                    // Upload files.
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await attainFile.Files[0].CopyToAsync(stream);
                    }

                    // Save file details to AttainFiles table.
                    string filelink = attainFile.DocId + "_"
                    + attainFile.SeqNo.ToString() + Path.GetExtension(attainFile.Files[0].FileName);
                    switch (attainFile.DocType)
                    {
                        case "0":
                            attainFile.FileLink = "Budget/" + filelink;
                            break;
                        case "1":
                            attainFile.FileLink = "Repair/" + filelink;
                            break;
                        case "2":
                            attainFile.FileLink = "Keep/" + filelink;
                            break;
                        case "3":
                            attainFile.FileLink = "BuyEvaluate/" + filelink;
                            break;
                        case "4":
                            attainFile.FileLink = "Delivery/" + filelink;
                            break;
                        case "5":
                            attainFile.FileLink = "Asset/" + filelink;
                            break;
                    }
                    attainFile.Rtt = DateTime.Now;
                    attainFile.Rtp = ur.Id;
                    _context.BMEDAttainFiles.Add(attainFile);
                    _context.SaveChanges();

                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            else
            {
                string msg = "";
                foreach (var error in ViewData.ModelState.Values.SelectMany(modelState => modelState.Errors))
                {
                    msg += error.ErrorMessage + Environment.NewLine;
                }
                throw new Exception(msg);
            }

            return new JsonResult(attainFile)
            {
                Value = new { success = true, error = "" },
            };
        }

        public IActionResult Delete(string id = null, int seq = 0, string typ = null)
        {
            string WebRootPath = _hostingEnvironment.WebRootPath;
            string path1 = Path.Combine(WebRootPath + "/Files/BMED/");
            AttainFileModel attainfiles = _context.BMEDAttainFiles.Find(typ, id, seq);
            if (attainfiles != null)
            {
                FileInfo ff;
                try
                {
                    if (typ == "2")
                    {
                        ff = new FileInfo(Path.Combine(path1, attainfiles.FileLink.Replace("Files/BMED/", "")));
                        ff.Delete();
                    }
                    else
                    {
                        ff = new FileInfo(Path.Combine(path1, attainfiles.FileLink));
                        ff.Delete();
                    }
                }
                catch (Exception e)
                {
                    return Content(e.Message);
                }
                _context.BMEDAttainFiles.Remove(attainfiles);
                _context.SaveChanges();
            }
            List<AttainFileModel> af = _context.BMEDAttainFiles.Where(f => f.DocId == id)
                                                               .Where(f => f.DocType == typ).ToList();

            return ViewComponent("BMEDAttainFileList", new { id = id, typ = typ, viewType = "Edit" });
        }

        /* For Create View's scale.*/
        public IActionResult Delete3(string id = null, int seq = 0, string typ = null)
        {
            string WebRootPath = _hostingEnvironment.WebRootPath;
            string path1 = Path.Combine(WebRootPath + "/Files/BMED/");
            AttainFileModel attainfiles = _context.BMEDAttainFiles.Find(typ, id, seq);
            if (attainfiles != null)
            {
                FileInfo ff;
                try
                {
                    if (typ == "2")
                    {
                        ff = new FileInfo(Path.Combine(path1, attainfiles.FileLink.Replace("Files/BMED/", "")));
                        ff.Delete();
                    }
                    else
                    {
                        ff = new FileInfo(Path.Combine(path1, attainfiles.FileLink));
                        ff.Delete();
                    }
                }
                catch (Exception e)
                {
                    return Content(e.Message);
                }
                _context.BMEDAttainFiles.Remove(attainfiles);
                _context.SaveChanges();
            }
            List<AttainFileModel> af = _context.BMEDAttainFiles.Where(f => f.DocId == id)
                                                               .Where(f => f.DocType == typ).ToList();

            return ViewComponent("BMEDAttainFileList3", new { id = id, typ = typ });
        }

        // GET: /AttainFiles/Create
        public IActionResult Create(string id = null, string typ = null, string title = null, int? vendorId = null)
        {
            var ur = _userRepo.Find(u => u.UserName == this.User.Identity.Name).FirstOrDefault();
            AttainFileModel a = new AttainFileModel();
            a.DocType = typ;
            a.DocId = id;
            a.Title = title;
            if (typ == "3" && vendorId != null)
                a.Rtp = vendorId;
            else
                a.Rtp = ur.Id;
            a.Rtt = DateTime.Now;
            return View(a);
        }

        // POST: /AttainFiles/Create
        [HttpPost]
        public async Task<IActionResult> Create(AttainFileModel attainFile, IEnumerable<IFormFile> file)
        {
            string s = "/Files/BMED";

            switch (attainFile.DocType)
            {
                case "0":
                    s += "/Budget";
                    break;
                case "1":
                    s += "/Repair";
                    break;
                case "2":
                    s += "/Keep";
                    break;
                case "3":
                    s += "/BuyEvaluate";
                    break;
                case "4":
                    s += "/Delivery";
                    break;
                case "5":
                    s += "/Asset";
                    break;
                case "6":
                    s += "/DeptStok";
                    break;
                case "7":
                    s += "/MContract";
                    break;
                case "8":
                    s += "/PContract";
                    break;
                case "9":
                    s += "/News";
                    break;
            }
            string WebRootPath = _hostingEnvironment.WebRootPath;
            var bmedFile = _context.BMEDAttainFiles.Where(af => af.DocType == attainFile.DocType && af.DocId == attainFile.DocId)
                                                   .Select(af => af.SeqNo).ToList();
            int? i = null;
            if (bmedFile.Count() > 0)
            {
                i = bmedFile.Max();
            }
            if (i == null)
                attainFile.SeqNo = 1;
            else
                attainFile.SeqNo = Convert.ToInt32(i + 1);
            string path = Path.Combine(WebRootPath + s, attainFile.DocId + "_"
                + attainFile.SeqNo.ToString() + Path.GetExtension(Request.Form.Files[0].FileName));
            string filelink = attainFile.DocId + "_"
                + attainFile.SeqNo.ToString() + Path.GetExtension(Request.Form.Files[0].FileName);
            try
            {
                // Upload files.
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await Request.Form.Files[0].CopyToAsync(stream);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            switch (attainFile.DocType)
            {
                case "0":
                    attainFile.FileLink = "Budget/" + filelink;
                    break;
                case "1":
                    attainFile.FileLink = "Repair/" + filelink;
                    break;
                case "2":
                    attainFile.FileLink = "Keep/" + filelink;
                    break;
                case "3":
                    attainFile.FileLink = "BuyEvaluate/" + filelink;
                    break;
                case "4":
                    attainFile.FileLink = "Delivery/" + filelink;
                    break;
                case "5":
                    attainFile.FileLink = "Asset/" + filelink;
                    break;
                case "6":
                    attainFile.FileLink = "DeptStok/" + filelink;
                    break;
                case "7":
                    attainFile.FileLink = "MContract/" + filelink;
                    break;
                case "8":
                    attainFile.FileLink = "PContract/" + filelink;
                    break;
                case "9":
                    attainFile.FileLink = "News/" + filelink;
                    break;
            }
            if (attainFile.IsPub)
                attainFile.IsPublic = "Y";
            attainFile.Rtt = DateTime.Now;
            _context.BMEDAttainFiles.Add(attainFile);
            try
            {
                _context.SaveChanges();

                return Content("檔案上載完成");
            }
            catch (Exception e)
            {
                //throw new Exception(e.Message);
                ModelState.AddModelError("", e.Message);
                return Content(e.Message);
            }

        }

        private bool AttainFileModelExists(string id)
        {
            return _context.BMEDAttainFiles.Any(e => e.DocType == id);
        }
    }
}
