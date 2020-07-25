using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using EDIS.Data;
using EDIS.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using EDIS.Models.AccountViewModels;

namespace EDIS.Models.Identity
{
    public class CustomRoleManager : RoleManager<ApplicationRole>
    {
        private readonly ApplicationDbContext _context;

        public IRoleStore<ApplicationRole> _roleStore;
        public CustomRoleManager(ApplicationDbContext context, IRoleStore<ApplicationRole> store, IEnumerable<IRoleValidator<ApplicationRole>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<ApplicationRole>> logger)
            : base(store, roleValidators, keyNormalizer, errors, logger)
        {
            _context = context;
            _roleStore = store;
        }

        public string[] GetUsersInRole(string roleName)
        {
            int roleId = 4; 
            var role = _context.AppRoles.Where(r => r.RoleName == roleName).FirstOrDefault();
            if (role != null)
            {
                roleId = role.RoleId;
            }
            var getUsers = _context.UsersInRoles.Where(r => r.RoleId == roleId).ToList();
            string[] roleUsers = new string[getUsers.Count];
            int i = 0;
            foreach (var user in getUsers)
            {
                roleUsers[i] = user.UserName;
                i++;
            }         
            return roleUsers;
        }

        public string[] GetRolesForUser(int userId)
        {
            int uid = userId;
            var getRoles = _context.UsersInRoles.Include(r => r.AppRoles)
                                                .Where(r => r.UserId == uid).ToList();
            string[] userRoles = new string[getRoles.Count];
            int i = 0;
            foreach (var role in getRoles)
            {
                userRoles[i] = role.AppRoles.RoleName;
                i++;
            }
            return userRoles;
        }

        public List<UserInRolesViewModel> GetRoles()
        {
            List<UserInRolesViewModel> rolelist = new List<UserInRolesViewModel>();
            UserInRolesViewModel rv;

            foreach (AppRoleModel r in _context.AppRoles.ToList())
            {
                rv = new UserInRolesViewModel();
                rv.RoleName = r.RoleName;
                rv.Description = r.Description;
                rv.IsSelected = false;
                rolelist.Add(rv);
            }
            var rst = rolelist.GroupBy(g => g.RoleName).Select(g => g.First());
            return rst.ToList();
        }

        public void RemoveUserFromRoles(string username, string[] roleNames)
        {
            var user = _context.AppUsers.Where(u => u.UserName == username).FirstOrDefault();
            if (user != null)
            {
                var userInRoles = _context.UsersInRoles.Where(r => roleNames.Contains(r.AppRoles.RoleName))
                                                       .Where(r => r.UserName == username).ToList();
                _context.UsersInRoles.RemoveRange(userInRoles);
                _context.SaveChanges();
            }
        }

        public void AddUserToRole(string username, string roleName)
        {
            var user = _context.AppUsers.Where(u => u.UserName == username).FirstOrDefault();
            if (user != null)
            {
                var role = _context.AppRoles.Where(r => r.RoleName == roleName).FirstOrDefault();
                if (role != null)
                {
                    UsersInRolesModel usersInRolesModel = new UsersInRolesModel();
                    usersInRolesModel.UserId = user.Id;
                    usersInRolesModel.UserName = user.UserName;
                    usersInRolesModel.RoleId = role.RoleId;
                    _context.UsersInRoles.Add(usersInRolesModel);
                    _context.SaveChanges();
                }
            }
        }

    }
}
