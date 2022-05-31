using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TGWLP.DAL.Entities;
using TGWLP.DAL.Interfaces;

namespace TGWLP
{
    /// <summary>
    /// Works as a base class for a page. Access requires authorization, exceptions need to be defined explicitly. Demands an antiforgery token by default.
    /// </summary>
    [Authorize]
    [ValidateAntiForgeryToken]
    public class PageBase : PageModel
    {
        protected IAppContext AppContext { get; set; }
        protected IConfiguration Configuration { get; set; }
        protected IOptions<AppSettings> Settings { get; set; }
        protected UserManager<User> UserManager { get; set; }
        protected RoleManager<Role> RoleManager { get; set; }
        protected IWebHostEnvironment Environment { get; set; }
        protected IHttpContextAccessor HttpContextAccessor { get; set; }
        protected IMapper Mapper { get; set; }
        protected ILogger Logger { get; set; }
        protected CultureLocalizer Localizer { get; set; }
        public string Layout { get; set; }
        [ViewData]
        public bool LoggedIn { get; set; }
        [ViewData]
        public Guid UserId { get; set; }
        [ViewData]
        public string UserName { get; set; }
        [ViewData]
        public User User { get; set; }
        [ViewData]
        public bool IsAdmin { get; set; }
        public string Error { get; set; }
        public string Message { get; set; }
        public string ReturnURL { get {
                return Request.Query["returnUrl"];
            } }
        [ViewData]
        public string PageTitle { get; set; }

        public PageBase(IAppContext context, IConfiguration config, UserManager<User> usermanager, RoleManager<Role> rolemanager, IWebHostEnvironment environment, IHttpContextAccessor httpcontextaccessor, IMapper mapper, CultureLocalizer localizer, ILogger<PageBase> logger, IOptions<AppSettings> settings) {
            this.AppContext = context;
            this.Configuration = config;
            this.UserManager = usermanager;
            this.RoleManager = rolemanager;
            this.Environment = environment;
            this.HttpContextAccessor = httpcontextaccessor;
            this.Mapper = mapper;
            this.Localizer = localizer;
            this.LoggedIn = this.HttpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
            this.Layout = "_Layout";
            this.Logger = logger;
            this.Settings = settings;
            GetUserInfo();
            SetPageTitle();
        }

        /// <summary>
        /// Parses information for signed-in users.
        /// </summary>
        private void GetUserInfo() {
            if (this.LoggedIn) {
                UserId = Guid.Parse(this.HttpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                UserName = this.HttpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
                this.User = AppContext.AppUsers.SingleOrDefault(x => x.Id == this.UserId);
                if (this.User != null) {
                    this.IsAdmin = this.User.UserRoles.Any() && this.User.UserRoles.Any(x => x.Role.Name == "Admin");
                }
            }
        }

        /// <summary>
        /// Redirects visitor to the page defined in the ReturnURL property.
        /// </summary>
        /// <returns></returns>
        public IActionResult RedirectToReturnURL() {
            return Redirect(this.ReturnURL);
        }

        /// <summary>
        /// Custom function for declaring unauthorized access.
        /// </summary>
        /// <exception cref="UnauthorizedAccessException"></exception>
        protected void DeclareUnauthorized() {
            throw new UnauthorizedAccessException();
        }

        /// <summary>
        /// Sets the page name for browser tabs.
        /// </summary>
        protected virtual void SetPageTitle() {
            var displayName = this.GetType()
            .GetCustomAttributes(typeof(DisplayAttribute), true)
            .FirstOrDefault() as DisplayAttribute;

            if (displayName != null) {
                PageTitle = Localizer[displayName.Name];
            }
        }
    }
}
