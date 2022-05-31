using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGWLP.DAL.Entities;

namespace TGWLP.DAL.Interfaces
{
    public interface IAppContext
    {
        public DbSet<User> AppUsers { get; set; }
        public DbSet<UserRole> AppUserRoles { get; set; }
        public DbSet<Role> AppRoles { get; set; }
        public DbSet<AppClaim> AppClaims { get; set; }
        public DbSet<RoleClaim> AppRoleClaims { get; set; }
        public DbSet<Story> Stories { get; set; }

        public void AddOrUpdate<T>(T entity) where T : class, IEntity;
        public int SaveChanges();
    }
}
