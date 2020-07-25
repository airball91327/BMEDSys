using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDIS.Models;
using EDIS.Data;
using EDIS.Models.Identity;
using EDIS.Repositories;
using EDIS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using EDIS.Areas.BMED.Data;
using Microsoft.AspNetCore.Http;
using X.PagedList;
using EDIS.Models.AccountViewModels;
using EDIS.Extensions;

namespace EDIS.Controllers
{
    [Authorize]
    public class AppUserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly BMEDDbContext _BMEDcontext;
        private readonly IRepository<AppUserModel, int> _userRepo;
        private readonly IRepository<DepartmentModel, string> _dptRepo;
        private readonly CustomUserManager userManager;
        private readonly CustomRoleManager roleManager;
        private int pageSize = 100;

        public AppUserController(ApplicationDbContext context,
                                 BMEDDbContext BMEDcontext,
                                 IRepository<AppUserModel, int> userRepo,
                                 IRepository<DepartmentModel, string> dptRepo,
                                 CustomUserManager customUserManager,
                                 CustomRoleManager customRoleManager)
        {
            _context = context;
            _BMEDcontext = BMEDcontext;
            _userRepo = userRepo;
            _dptRepo = dptRepo;
            userManager = customUserManager;
            roleManager = customRoleManager;
        }

        // Get: AppUser/Index
        public IActionResult Index()
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            SelectListItem li;
            _context.Departments.ToList()
                .ForEach(d =>
                {
                    li = new SelectListItem();
                    li.Text = d.Name_C;
                    li.Value = d.DptId;
                    listItem.Add(li);

                });
            ViewData["DEPT"] = new SelectList(listItem, "Value", "Text");
            return View();
        }

        // POST: AppUser/Index
        [HttpPost]
        public IActionResult Index(IFormCollection fm, int page = 1)
        {
            /* Get login user. */
            var ur = _userRepo.Find(u => u.UserName == this.User.Identity.Name).FirstOrDefault();

            string qname = fm["qtyNAME"];
            string dpt = fm["qtyDEPT"];
            string uname = fm["qtyUSERNAME"];
            List<AppUserModel> ulist;
            AppUserModel usr = _context.AppUsers.Find(ur.Id);

            if (userManager.IsInRole(User, "Admin") || userManager.IsInRole(User, "Manager"))
            {
                ulist = _context.AppUsers.ToList();
                if (userManager.IsInRole(User, "Manager"))
                {
                    ulist = ulist.Where(u => u.DptId == usr.DptId).ToList();
                }
            }
            else
            {
                ulist = _context.AppUsers.Where(u => u.UserName == ur.UserName).ToList();
            }
            if (!string.IsNullOrEmpty(qname))
            {
                ulist = ulist.Where(u => u.FullName != null)
                             .Where(u => u.FullName.Contains(qname)).ToList();
            }
            if (!string.IsNullOrEmpty(dpt))
            {
                ulist = ulist.Where(u => u.DptId == dpt).ToList();
            }
            if (!string.IsNullOrEmpty(uname))
            {
                ulist = ulist.Where(u => u.UserName == uname).ToList();
            }
            if (ulist.ToPagedList(page, pageSize).Count <= 0)
                return PartialView("List", ulist.ToPagedList(1, pageSize));

            return PartialView("List", ulist.ToPagedList(page, pageSize));
        }

        // GET: AppUser/Details/5
        public IActionResult Details(int id)
        {
            AppUserModel u = _context.AppUsers.Find(id);
            List<UserInRolesViewModel> rv = roleManager.GetRoles();
            UserInRolesViewModel uv;
            foreach (string r in roleManager.GetRolesForUser(u.Id))
            {
                uv = rv.Where(v => v.RoleName == r).ToList().FirstOrDefault();
                if (uv != null)
                {
                    uv.IsSelected = true;
                }
            }
            u.InRoles = rv;
            return View(u);
        }

        // GET: AppUser/Create
        public ActionResult Create()
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            SelectListItem li;
            _context.Departments.ToList()
                    .ForEach(d =>
                    {
                        li = new SelectListItem();
                        li.Text = d.Name_C;
                        li.Value = d.DptId;
                        listItem.Add(li);

                    });
            ViewData["DPTID"] = new SelectList(listItem, "Value", "Text");
            AppUserModel u = new AppUserModel();
            u.InRoles = roleManager.GetRoles();
            return View(u);
        }

        // POST: AppUser/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AppUserModel appUser)
        {
            if (ModelState.IsValid)
            {
                AppUserModel user = _context.AppUsers.Where(u => u.UserName == appUser.UserName).FirstOrDefault();
                if (user != null)
                {
                    ModelState.AddModelError("", "使用者名稱重複");
                    return View(appUser);
                }
                // user's password encrypt by DES.
                string DESKey = "84203025";
                var encryptPW = CryptoExtensions.DESEncrypt(appUser.Password, DESKey);   // Encrypt and check password.
                appUser.Password = encryptPW;
                appUser.DateCreated = DateTime.Now;
                appUser.LastActivityDate = DateTime.Now;
                _context.AppUsers.Add(appUser);
                _context.SaveChanges();
                //
                //// Save log. 
                //SystemLog log = new SystemLog();
                //log.LogClass = "系統管理者紀錄";
                //log.LogTime = DateTime.UtcNow.AddHours(8);
                //log.UserId = WebSecurity.CurrentUserId;
                //log.Action = "使用者維護 > 新增使用者 > " + newUser.FullName + "(" + newUser.UserName + ")";
                //db.SystemLogs.Add(log);
                //db.SaveChanges();
                //
                List<UserInRolesViewModel> uv = appUser.InRoles.Where(v => v.IsSelected == true).ToList();
                foreach (UserInRolesViewModel u in uv)
                {
                    roleManager.AddUserToRole(appUser.UserName, u.RoleName);
                }
                return RedirectToAction("Index");
            }

            appUser.InRoles = roleManager.GetRoles();
            return View(appUser);
        }

        // GET: AppUser/Edit/5
        public ActionResult Edit(int id)
        {
            AppUserModel u = _context.AppUsers.Find(id);
            if (u == null)
            {
                return NotFound();
            }
            else
            {
                if (u.VendorId != null)
                    u.VendorName = _context.Vendors.Find(u.VendorId).VendorName;
                List<UserInRolesViewModel> rv = roleManager.GetRoles();
                UserInRolesViewModel uv;
                foreach (string r in roleManager.GetRolesForUser(u.Id))
                {
                    uv = rv.Where(v => v.RoleName == r).ToList().FirstOrDefault();
                    if (uv != null)
                    {
                        uv.IsSelected = true;
                    }
                }
                u.InRoles = rv;
            }
            List<SelectListItem> listItem = new List<SelectListItem>();
            SelectListItem li;
            _context.Departments.ToList()
                    .ForEach(d =>
                    {
                        li = new SelectListItem();
                        li.Text = d.Name_C;
                        li.Value = d.DptId;
                        listItem.Add(li);

                    });
            ViewData["DPTID"] = new SelectList(listItem, "Value", "Text");
            return View(u);
        }

        // POST: AppUser/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AppUserModel appUser)
        {
            if (string.IsNullOrEmpty(appUser.Password))
            {
                ModelState.AddModelError("", "該使用者尚未設定密碼");
            }
            if (ModelState.IsValid)
            {
                AppUserModel user = _context.AppUsers.Find(appUser.Id);
                string checkResult = "";
                if (user != null)
                {
                    if (userManager.IsInRole(User, "Admin"))
                    {
                        var oriRoles = roleManager.GetRolesForUser(appUser.Id).ToList();  // Origin user roles.
                        if (roleManager.GetRolesForUser(appUser.Id).Count() > 0)
                            roleManager.RemoveUserFromRoles(appUser.UserName, roleManager.GetRolesForUser(appUser.Id));
                        List<UserInRolesViewModel> uv = appUser.InRoles.Where(v => v.IsSelected == true).ToList();
                        foreach (UserInRolesViewModel u in uv)
                        {
                            roleManager.AddUserToRole(appUser.UserName, u.RoleName);
                        }
                        var newRoles = roleManager.GetRolesForUser(appUser.Id).ToList();  // Updated user roles.
                        var removeRoles = oriRoles.Except(newRoles).ToList();
                        var addRoles = newRoles.Except(oriRoles).ToList();
                        if (removeRoles.Count() > 0 || addRoles.Count() > 0)
                        {
                            checkResult += "設定角色：";
                        }
                        if (removeRoles.Count() > 0)
                        {
                            checkResult += "刪除";
                            foreach (var name in removeRoles)
                            {
                                var roleDes = _context.AppRoles.Where(r => r.RoleName == name).ToList().First().Description;
                                checkResult += "【" + roleDes + "】";
                            }
                            checkResult += ";";
                        }
                        if (addRoles.Count() > 0)
                        {
                            checkResult += "新增";
                            foreach (var name in addRoles)
                            {
                                var roleDes = _context.AppRoles.Where(r => r.RoleName == name).ToList().First().Description;
                                checkResult += "【" + roleDes + "】";
                            }
                            checkResult += ";";
                        }
                    }
                    //Checking the modified columns.
                    var oriUser = _context.AppUsers.Where(u => u.Id == appUser.Id).ToList().FirstOrDefault();
                    if (oriUser.UserName != appUser.UserName)
                    {
                        checkResult += "使用者名稱；";
                    }
                    if (oriUser.FullName != appUser.FullName)
                    {
                        checkResult += "使用者全名；";
                    }
                    if (oriUser.Email != appUser.Email)
                    {
                        checkResult += "電子信箱；";
                    }
                    if (oriUser.Ext != appUser.Ext)
                    {
                        checkResult += "分機；";
                    }
                    if (oriUser.Mobile != appUser.Mobile)
                    {
                        checkResult += "行動電話；";
                    }
                    if (oriUser.DptId != appUser.DptId)
                    {
                        checkResult += "所屬部門；";
                    }
                    if (oriUser.Status != appUser.Status)
                    {
                        checkResult += "狀態；";
                    }
                    //
                    appUser.LastActivityDate = DateTime.Now;
                    _context.Entry(oriUser).State = EntityState.Detached;
                    _context.Entry(appUser).State = EntityState.Modified;
                    _context.SaveChanges();
                }
                // Save log. 
                //SystemLog log = new SystemLog();
                //log.LogClass = "系統管理者紀錄";
                //log.LogTime = DateTime.UtcNow.AddHours(8);
                //log.UserId = WebSecurity.CurrentUserId;
                //log.Action = "使用者維護 > 編輯使用者 > " + appUser.FullName + "(" + appUser.UserName + ") > " + checkResult;
                //db.SystemLogs.Add(log);
                //db.SaveChanges();
                return RedirectToAction("Index", "AppUser", new { Area = "" });
            }
            // If edit failed.
            if (appUser.VendorId != null)
                appUser.VendorName = _context.Vendors.Find(appUser.VendorId).VendorName;
            List<UserInRolesViewModel> urv = roleManager.GetRoles();
            UserInRolesViewModel uiv;
            foreach (string ss in roleManager.GetRolesForUser(appUser.Id))
            {
                uiv = urv.Where(v => v.RoleName == ss).ToList().FirstOrDefault();
                if (uiv != null)
                {
                    uiv.IsSelected = true;
                }
            }
            appUser.InRoles = urv;
            return View(appUser);
        }

        public JsonResult GetUsersInDpt(string id)
        {
            DepartmentModel d = _context.Departments.Find(id);
            List<AppUserModel> ul;
            List<UserList> us = new List<UserList>();
            string s = "";
            if (d != null)
            {
                ul = _context.AppUsers.Where(p => p.DptId == d.DptId)
                                      .Where(p => p.Status == "Y").ToList();
                //s += "[";
                foreach (AppUserModel f in ul)
                {
                    UserList u = new UserList();
                    u.uid = f.Id;
                    u.uname = "(" + f.UserName + ")" + f.FullName;
                    us.Add(u);
                }
                s = JsonConvert.SerializeObject(us);
            }
            return Json(s);
        }

        public JsonResult GetUsersByKeyname(string keyname)
        {
            List<AppUserModel> ul;
            List<UserList> us = new List<UserList>();
            string s = "";
            ul = _context.AppUsers.Where(p => p.FullName.Contains(keyname) || p.UserName == keyname)
                                  .Where(p => p.Status == "Y")
                                  .ToList();
            foreach (AppUserModel f in ul)
            {
                UserList u = new UserList();
                u.uid = f.Id;
                u.uname = "(" + f.UserName + ")" + f.FullName;
                us.Add(u);
            }
            s = JsonConvert.SerializeObject(us);
            return Json(s);
        }

        public JsonResult GetUsersInDptByKeyname(string keyname)
        {
            List<AppUserModel> ul;
            List<UserList> us = new List<UserList>();
            string s = "";
            if (!string.IsNullOrEmpty(keyname))
            {
                _context.Departments.Where(u => u.Name_C.Contains(keyname))
                    .ToList()
                    .ForEach(d =>
                    {
                        ul = _context.AppUsers.Where(p => p.DptId == d.DptId)
                                              .Where(p => p.Status == "Y").ToList();
                        foreach (AppUserModel f in ul)
                        {
                            UserList u = new UserList();
                            u.uid = f.Id;
                            u.uname = "(" + f.UserName + ")" + f.FullName;
                            us.Add(u);
                        }
                        s = JsonConvert.SerializeObject(us);
                    });
            }
            s = JsonConvert.SerializeObject(us);

            return Json(s);
        }

        public JsonResult GetUsersByVendorId(string vendorId)
        {
            List<AppUserModel> ul;
            List<UserList> us = new List<UserList>();
            string s = "";
            ul = _context.AppUsers.Where(p => p.VendorId == Convert.ToInt32(vendorId))
                                  .Where(p => p.Status == "Y")
                                  .ToList();
            foreach (AppUserModel f in ul)
            {
                UserList u = new UserList();
                u.uid = f.Id;
                u.uname = "(" + f.UserName + ")" + f.FullName;
                us.Add(u);
            }
            s = JsonConvert.SerializeObject(us);
            return Json(s);
        }

        public string GetFullName()
        {
            return _context.AppUsers.Where(a => a.UserName == User.Identity.Name).FirstOrDefault().FullName;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class UserList
    {
        public int uid;
        public string uname;
    }

}