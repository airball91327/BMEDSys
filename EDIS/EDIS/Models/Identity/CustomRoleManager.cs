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
            int roleId = _context.AppRoles.Where(r => r.RoleName == roleName).FirstOrDefault().RoleId;
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
    }
}
