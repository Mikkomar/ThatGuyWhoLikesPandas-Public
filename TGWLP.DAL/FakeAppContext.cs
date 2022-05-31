using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TGWLP.DAL.Attributes;
using TGWLP.DAL.Entities;
using TGWLP.DAL.Entities.HistoryLogs;
using TGWLP.DAL.Interfaces;
using TGWLP.DAL.Repositories;

namespace TGWLP.DAL
{
    public class FakeAppContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, RoleClaim, IdentityUserToken<Guid>>, IAppContext
    {
        private IHttpContextAccessor _httpContextAccessor { get; }
        private IConfiguration _configuration { get; }

        public FakeAppContext(DbContextOptions<FakeAppContext> options, IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = config;
            this.Seed();
        }

        public DbSet<User> AppUsers { get; set; }
        public DbSet<UserRole> AppUserRoles { get; set; }
        public DbSet<Role> AppRoles { get; set; }
        public DbSet<AppClaim> AppClaims { get; set; }
        public DbSet<RoleClaim> AppRoleClaims { get; set; }
        public DbSet<Story> Stories { get; set; }

        private void Seed()
        {
            this.Stories.Add(new Story()
            {
                Title = "Lorem Ipsum",
                Text = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis convallis id turpis vel ultrices. Maecenas turpis neque, congue tincidunt massa sed, scelerisque faucibus leo. Quisque laoreet velit in dolor rhoncus vehicula. Aenean euismod, risus eget maximus mattis, velit nulla euismod ligula, eget dapibus ligula justo eget augue. Sed volutpat massa sed fringilla facilisis. Vestibulum laoreet metus sit amet purus dignissim venenatis. Fusce facilisis erat ex, in elementum nisl porta a. Integer ac lectus a odio rutrum bibendum. Etiam nisi felis, vestibulum non dolor at, gravida varius felis. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Suspendisse id erat felis. Sed iaculis neque non purus lobortis, quis consectetur diam eleifend. Cras auctor risus purus, sit amet imperdiet magna faucibus in. Quisque dictum consequat volutpat. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; In eu ipsum sed dui condimentum faucibus eu ac risus.
Praesent lacus orci, condimentum at pharetra et, sodales sed mi. Mauris lorem diam, posuere vitae auctor ac, rhoncus venenatis nunc. Vestibulum nisl tortor, fringilla nec ex vel, scelerisque ornare nisl. Sed fringilla fringilla tellus ac tempus. Vivamus a bibendum lacus, a ornare enim. Nam quis efficitur lectus, sed ultricies tellus. Duis rhoncus nec turpis a malesuada. Quisque vel nibh orci. Vestibulum consequat, orci ac molestie interdum, sem justo pretium metus, a vestibulum leo ex sed enim. Vestibulum rutrum consectetur accumsan. Praesent accumsan nisi justo, a tincidunt odio ultricies in. Nunc ut quam ante. Donec at egestas nulla, sed condimentum nunc.
Donec a orci eu lacus fringilla condimentum eu vitae velit. Phasellus hendrerit ligula ante, nec tincidunt metus vestibulum non. Maecenas nec libero mollis, tempor metus sit amet, iaculis risus. Maecenas leo arcu, faucibus quis velit eget, interdum laoreet odio. Nunc aliquam, sapien at faucibus vestibulum, mauris sem tempus leo, quis tristique quam eros at sem. Sed nec arcu accumsan, luctus odio et, dictum lorem. Quisque et rutrum felis.
Cras dapibus, ligula ac rhoncus condimentum, urna nulla pretium felis, ut sodales enim arcu nec nisi. Sed sit amet tempor purus, eget eleifend felis. Ut sed finibus odio, id volutpat sapien. In congue, metus id pulvinar bibendum, leo leo vulputate dui, eget facilisis velit odio in felis. Vestibulum non porttitor leo. Nunc dictum nibh orci, ut placerat nunc sagittis ac. Mauris vestibulum est vel enim commodo sagittis.
Etiam a est nulla. Donec sem neque, tristique sit amet tristique vel, facilisis eu ante. Vivamus mauris ipsum, dictum quis imperdiet sed, feugiat non lacus. Sed commodo massa quis enim vulputate fermentum. Proin consequat dui eu lacus auctor eleifend. Nam facilisis enim nec ex tincidunt laoreet. Fusce condimentum, nisi id interdum venenatis, dolor arcu consectetur nunc, vel congue eros tellus ut lectus. Vestibulum efficitur consequat libero sit amet elementum. Integer ullamcorper, mauris vitae pretium luctus, justo sapien sagittis mi, sed sollicitudin odio ex ut felis. Quisque ex velit, volutpat at eros a, aliquam sollicitudin sapien. Nunc aliquam elementum nisi eu congue. Aenean non viverra tortor. Nunc quis egestas elit. Vivamus dictum mi semper justo.",
                PublishDate = DateTime.Now.AddDays(-1)

            });

            this.SaveChanges();
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
