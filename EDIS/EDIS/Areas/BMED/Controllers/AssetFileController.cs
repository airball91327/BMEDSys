using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using EDIS.Areas.BMED.Data;
using EDIS.Areas.BMED.Models.DeliveryModels;
using EDIS.Areas.BMED.Models.KeepModels;
using EDIS.Areas.BMED.Models.RepairModels;
using EDIS.Models.Identity;
using EDIS.Repositories;
using EDIS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace EDIS.Areas.BMED.Controllers
{
    [Area("BMED")]
    [Authorize]
    public class AssetFileController : Controller
    {
        private readonly BMEDDbContext _context;
        private readonly CustomUserManager userManager;
        private readonly CustomRoleManager roleManager;
        private readonly IRepository<AppUserModel, int> _userRepo;
        private readonly IHostingEnvironment _hostingEnvironment;
        private int pageSize = 100;

        public AssetFileController(BMEDDbContext context,
                                   IRepository<AppUserModel, int> userRepo,
                                   CustomRoleManager customRoleManager,
                                   CustomUserManager customUserManager,
                                   IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _userRepo = userRepo;
            roleManager = customRoleManager;
            userManager = customUserManager;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: /BMED/AssetFile/
        public IActionResult Index()
        {
            return View(_context.AssetFiles.ToList());
        }

        // GET: /BMED/AssetFile/Create
        public IActionResult Create(string id = null, int sno = 1, string title = null)
        {
            // Get Login User's details.
            var ur = _userRepo.Find(usr => usr.UserName == User.Identity.Name).FirstOrDefault();
            AssetFileModel a = new AssetFileModel();
            a.AssetNo = id;
            a.Title = title;
            a.SeqNo = sno;
            a.Rtp = ur.Id.ToString();
            a.Rtt = DateTime.Now;
            return View(a);
        }

        // POST: /BMED/AssetFile/Create
        [HttpPost]
        public async Task<IActionResult> Create(AssetFileModel assetfile, IEnumerable<IFormFile> file)
        {
            if (ModelState.IsValid)
            {
                string s = "~/Files";
                s += "/Asset";

#if DEBUG
                s = "~/App_Data";
#endif
                int? i = _context.AssetFiles.FromSql("SELECT MAX(Fid) FROM AssetFile WHERE ASSETNO = @ano AND SEQNO = @sno ",
                        assetfile.AssetNo, assetfile.SeqNo).FirstOrDefault().Fid;
                if (i == null)
                    assetfile.Fid = 1;
                else
                    assetfile.Fid = Convert.ToInt32(i + 1);
                string WebRootPath = _hostingEnvironment.WebRootPath;
                string path = Path.Combine(WebRootPath + s, assetfile.AssetNo + "_"
                    + assetfile.SeqNo.ToString() + "_" + assetfile.Fid.ToString() + Path.GetExtension(Request.Form.Files[0].FileName));
                string filelink = assetfile.AssetNo + "_" + assetfile.SeqNo.ToString() + "_"
                    + assetfile.Fid.ToString() + Path.GetExtension(Request.Form.Files[0].FileName);
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
                    ModelState.AddModelError("", e.Message);
                }
                assetfile.FileLink = "Asset/" + filelink;
                assetfile.Rtt = DateTime.Now;
                _context.AssetFiles.Add(assetfile);
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
                return View();
            }

            return Content("錯誤!!");
        }

        // GET: /BMED/AssetFile/CopyTo
        public IActionResult CopyTo(string id, int sno = 1)
        {
            AssetModel t = _context.BMEDAssets.Find(id);
            List<AssetModel> at = new List<AssetModel>();
            if (t != null)
            {
                at = _context.BMEDAssets.Where(a => a.Docid == t.Docid).ToList();
                AssetFileModel af = _context.AssetFiles.Where(f => f.AssetNo == id && f.SeqNo == sno)
                    .FirstOrDefault();
                ViewData["ano"] = id;
                ViewData["sno"] = sno;
                ViewData["cname"] = t.Cname;
                ViewData["title"] = af.Title;
            }
            return View(at);
        }

        [HttpPost]
        public IActionResult CopyTo(IFormCollection fm)
        {
            // Get Login User's details.
            var ur = _userRepo.Find(usr => usr.UserName == User.Identity.Name).FirstOrDefault();
            //
            AssetFileModel af;
            string ano = fm["ano"];
            int sno = Convert.ToInt32(fm["sno"]);
            List<AssetFileModel> fs = _context.AssetFiles.Where(f => f.AssetNo == ano && f.SeqNo == sno).ToList();
            string[] slist = this.Request.Form["IsCopy"];
            int fid = 0;
            if (slist != null)
            {
                foreach (string s in slist)
                {
                    fid = _context.AssetFiles.Where(f => f.AssetNo == s && f.SeqNo == sno)
                        .DefaultIfEmpty()
                        .Max(p => p == null ? 0 : p.Fid);

                    foreach (AssetFileModel a in fs)
                    {
                        string WebRootPath = _hostingEnvironment.WebRootPath;
                        string filePath = Path.Combine(WebRootPath + "/Files/BMED/");
                        fid++;
                        FileInfo inf = new FileInfo(filePath + a.FileLink);
                        af = new AssetFileModel();
                        af.AssetNo = s;
                        af.SeqNo = sno;
                        af.Fid = fid;
                        af.Title = a.Title;
                        af.FileLink = "Asset/" + af.AssetNo + "_" + af.SeqNo.ToString() + "_"
                                   + af.Fid.ToString() + inf.Extension;
                        af.Rtp = ur.Id.ToString();
                        af.Rtt = DateTime.Now;
                        _context.AssetFiles.Add(af);
                        try
                        {
                            inf.CopyTo(filePath + af.FileLink);
                        }
                        catch (Exception ex)
                        {
                            return Content(ex.Message);
                        }
                    }
                }
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    return Content(ex.Message);
                }
            }

            return Content("");
        }

        // GET: /BMED/AssetFile/Delete/5
        public IActionResult Delete(string ano = null, int seqno = 1, int fid = 1)
        {
            AssetFileModel assetfile = _context.AssetFiles.Find(ano, seqno, fid);
            if (assetfile == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    string WebRootPath = _hostingEnvironment.WebRootPath;
                    string filePath = Path.Combine(WebRootPath + "/Files/BMED/");
                    FileInfo ff = new FileInfo(Path.Combine(filePath, assetfile.FileLink));
                    ff.Delete();
                    _context.AssetFiles.Remove(assetfile);
                    _context.SaveChanges();
                    //return Json(new { msg = "儲存成功!" }, JsonRequestBehavior.AllowGet);
                    return Content("");
                }
                catch (Exception ex)
                {
                    return Content(ex.Message);
                }
                //List<AssetFile> af = db.AssetFiles.Where(f => f.AssetNo == ano).ToList();
                //if (title != null)
                //    af = af.Where(f => f.Title == title).ToList();
                //return PartialView("List", af);

            }
        }

        // POST: /BMED/AssetFile/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(string id)
        {
            AssetFileModel assetfile = _context.AssetFiles.Find(id);
            _context.AssetFiles.Remove(assetfile);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult List(string id = null, string title = null)
        {
            List<AssetFileModel> af = new List<AssetFileModel>();
            if (id != null)
            {
                AppUserModel u;
                af = _context.AssetFiles.Where(f => f.AssetNo == id).ToList();
                if (title != null)
                    af = af.Where(f => f.Title == title).ToList();
                foreach (AssetFileModel a in af)
                {
                    u = _context.AppUsers.Find(Convert.ToInt32(a.Rtp));
                    a.UserName = u.FullName;
                }
            }
            else
            {
                AppUserModel u;
                af = _context.AssetFiles.ToList();
                foreach (AssetFileModel a in af)
                {
                    u = _context.AppUsers.Find(Convert.ToInt32(a.Rtp));
                    a.UserName = u.FullName;
                }
            }
            return View(af);
        }
        //public ActionResult AssetList(string id = null, int sno = 1, string title = null)
        //{
        //    List<AssetFile> af = new List<AssetFile>();
        //    if (id != null)
        //    {
        //        Asset at = db.Assets.Find(id);
        //        ViewData["PlantNo"] = at.AssetNo;
        //        ViewData["PlantName"] = at.Cname;
        //        UserProfile u;
        //        af = db.AssetFiles.Where(f => f.AssetNo == id && f.SeqNo == sno).ToList();
        //        if (title != null)
        //            af = af.Where(f => f.Title == title).ToList();
        //        foreach (AssetFile a in af)
        //        {
        //            u = db.UserProfiles.Find(Convert.ToInt32(a.Rtp));
        //            a.UserName = u.FullName;
        //        }
        //    }
        //    else
        //    {
        //        UserProfile u;
        //        af = db.AssetFiles.ToList();
        //        foreach (AssetFile a in af)
        //        {
        //            u = db.UserProfiles.Find(Convert.ToInt32(a.Rtp));
        //            a.UserName = u.FullName;
        //        }
        //    }
        //    return View(af);
        //}

        public IActionResult AssetList(string id)
        {
            List<AssetFileModel> af = new List<AssetFileModel>();
            if (id != null)
            {
                AssetModel at = _context.BMEDAssets.Find(id);
                ViewData["PlantNo"] = at.AssetNo;
                ViewData["PlantName"] = at.Cname;
                AppUserModel u;
                af = _context.AssetFiles.Where(f => f.AssetNo == id).OrderBy(f => f.SeqNo).ToList();

                foreach (AssetFileModel a in af)
                {
                    u = _context.AppUsers.Find(Convert.ToInt32(a.Rtp));
                    a.UserName = u.FullName;
                }
            }
            else
            {
                AppUserModel u;
                af = _context.AssetFiles.ToList();
                foreach (AssetFileModel a in af)
                {
                    u = _context.AppUsers.Find(Convert.ToInt32(a.Rtp));
                    a.UserName = u.FullName;
                }
            }
            return View(af);
        }

        public IActionResult CheckFiles(string id = null, string cls = null)
        {
            if (id != null)
            {
                List<AssetFileModel> af = _context.AssetFiles.Where(f => f.AssetNo == id).ToList();
                List<NeedFileModel> nf = new List<NeedFileModel>();
                if (cls == "得標廠商")
                    nf = _context.NeedFiles.Where(e => e.Type == "1").ToList();
                else if (cls == "設備工程師")
                    nf = _context.NeedFiles.Where(e => e.Type == "2").ToList();
                foreach (NeedFileModel n in nf)
                {
                    if (af.Where(f => f.Title == n.Title).Count() <= 0)
                        return Content("檔案尚未上載完成!!");
                }
            }
            return Content("");
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}