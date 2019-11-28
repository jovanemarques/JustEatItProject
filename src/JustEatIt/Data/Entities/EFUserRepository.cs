using JustEatIt.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustEatIt.Data.Entities
{
    public class EFUserRepository : IUserRepository
    {
        private AppDataDbContext appContext;
        private ApplicationDbContext context;

        public EFUserRepository(AppDataDbContext appContext, ApplicationDbContext context)
        {
            this.appContext = appContext;
            this.context = context;
        }

        public IQueryable<UserInfo> GetAll => context.Users.Select(u =>
         new UserInfo
         {
             Id = u.Id,
             UserName = u.UserName,
             Name = u.Name,
             LastAccess = u.LastAccess,
             Use2FA = u.TwoFactorEnabled,
             Local = (u.PasswordHash != null),
             ExternalLogins = context.UserLogins.Count(l => (l.UserId == u.Id)),
             Role = context.Roles.FirstOrDefault(r => r.Id == context.UserRoles.FirstOrDefault(ur => ur.UserId == u.Id).RoleId).Name
         });
    }
}
