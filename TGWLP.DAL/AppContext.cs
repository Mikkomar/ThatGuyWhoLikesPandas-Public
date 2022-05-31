using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TGWLP.DAL.Attributes;
using TGWLP.DAL.Configurations;
using TGWLP.DAL.Entities;
using TGWLP.DAL.Entities.HistoryLogs;
using TGWLP.DAL.Interfaces;

namespace TGWLP.DAL
{
    public class AppContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, RoleClaim, IdentityUserToken<Guid>>, IAppContext
    {
        private IHttpContextAccessor _httpContextAccessor { get; }
        private IConfiguration _configuration { get; }
        public AppContext(DbContextOptions<AppContext> options, IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(options){
            _httpContextAccessor = httpContextAccessor;
            _configuration = config;
        }

        public DbSet<User> AppUsers { get; set; }
        public DbSet<UserRole> AppUserRoles { get; set; }
        public DbSet<Role> AppRoles { get; set; }
        public DbSet<AppClaim> AppClaims { get; set; }
        public DbSet<RoleClaim> AppRoleClaims { get; set; }
        public DbSet<Story> Stories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /* USERS AND MEMBERSHIPS */
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new AppClaimConfiguration());
            modelBuilder.ApplyConfiguration(new RoleClaimConfiguration());

            /* STORIES */
            modelBuilder.ApplyConfiguration(new StoryConfiguration());
        }

        public void AddOrUpdate<T>(T entity) where T : class, IEntity
        {
            if (entity.IsNewEntity())
            {
                this.Set<T>().Add(entity);
            }
            else
            {
                this.Set<T>().Update(entity);
            }
        }

        public override int SaveChanges()
        {
            string currentUser = this._configuration.GetSection("DefaultValues")["DefaultUnauthenticatedUserName"].ToString();
            string deletedMessage = this._configuration.GetSection("DefaultValues")["DefaultHistoryLogDeletedMessage"].ToString();

            if (this._httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                currentUser = this._httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            var auditedEntities = ChangeTracker.Entries<BaseEntity>()
            .Where(p => p.State == EntityState.Modified).ToList();

            var addedAuditedEntities = ChangeTracker.Entries<BaseEntity>()
            .Where(p => p.State == EntityState.Added).ToList();

            var deletedAuditedEntities = ChangeTracker.Entries<BaseEntity>()
            .Where(p => p.State == EntityState.Deleted).ToList();

            var modifiedGuid = Guid.NewGuid();

            foreach (var entity in auditedEntities)
            {
                var entityClass = entity.Entity.GetType();
                if (entityClass.Namespace == "Castle.Proxies")
                {
                    entityClass = entityClass.BaseType;
                }
                foreach (var property in entity.Properties)
                {
                    var entityProperty = entityClass.GetProperty(property.Metadata.Name);
                    if (Attribute.IsDefined(entityProperty, typeof(LogHistoryAttribute)))
                    {
                        var originalValue = entity.GetDatabaseValues().GetValue<object>(property.Metadata.Name);
                        var currentValue = property.CurrentValue;
                        if (originalValue?.ToString() != currentValue?.ToString())
                        {
                            var log = HistoryLog.CreateHistoryLog(entity.Entity.GetType());
                            if (log != null)
                            {
                                log.EntityId = entity.Entity.Id;
                                log.ModifierId = currentUser;
                                log.ModifiedDate = DateTime.Now;
                                log.ModifiedGroup = modifiedGuid;
                                log.Property = property.Metadata.Name;
                                log.OldValue = originalValue?.ToString();
                                log.NewValue = currentValue?.ToString();
                                base.Add(log);
                            }
                        }
                    }
                }
            }

            foreach (var entity in addedAuditedEntities)
            {
                var entityClass = entity.Entity.GetType();
                if (entityClass.Namespace == "Castle.Proxies")
                {
                    entityClass = entityClass.BaseType;
                }
                foreach (var property in entity.Properties)
                {
                    var entityProperty = entityClass.GetProperty(property.Metadata.Name);
                    if (Attribute.IsDefined(entityProperty, typeof(LogHistoryAttribute)))
                    {
                        var currentValue = property.CurrentValue;
                        if (currentValue != null)
                        {
                            var log = HistoryLog.CreateHistoryLog(entity.Entity.GetType());
                            if (log != null)
                            {
                                log.EntityId = entity.Entity.Id;
                                log.ModifierId = currentUser;
                                log.ModifiedDate = DateTime.Now;
                                log.ModifiedGroup = modifiedGuid;
                                log.Property = property.Metadata.Name;
                                log.OldValue = null;
                                log.NewValue = currentValue?.ToString();
                                base.Add(log);
                            }
                        }
                    }
                }
            }

            foreach (var entity in deletedAuditedEntities)
            {
                var entityClass = entity.Entity.GetType();
                if (entityClass.Namespace == "Castle.Proxies")
                {
                    entityClass = entityClass.BaseType;
                }
                foreach (var property in entity.Properties)
                {
                    var entityProperty = entityClass.GetProperty(property.Metadata.Name);
                    if (Attribute.IsDefined(entityProperty, typeof(LogHistoryAttribute)))
                    {
                        var log = HistoryLog.CreateHistoryLog(entity.Entity.GetType());
                        if (log != null)
                        {
                            log.EntityId = entity.Entity.Id;
                            log.ModifierId = currentUser;
                            log.ModifiedDate = DateTime.Now;
                            log.ModifiedGroup = modifiedGuid;
                            log.Property = property.Metadata.Name;
                            log.OldValue = property.OriginalValue?.ToString();
                            log.NewValue = deletedMessage;
                            base.Add(log);
                        }
                    }
                }
            }

            return base.SaveChanges();
        }
    }
}
