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
using System.Linq;
using System.Threading.Tasks;
using TGWLP.DAL.Entities;
using TGWLP.DAL.Interfaces;

namespace TGWLP.Pages
{
    [AllowAnonymous]
    public class PrivacyModel : PageBase
    {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(IAppContext context, IConfiguration config, UserManager<User> usermanager, RoleManager<Role> rolemanager, IWebHostEnvironment environment, IHttpContextAccessor httpcontextaccessor, IMapper mapper, CultureLocalizer localizer, ILogger<PageBase> logger, IOptions<AppSettings> settings) : base(context, config, usermanager, rolemanager, environment, httpcontextaccessor, mapper, localizer, logger, settings)
        {
        }

        public void OnGet() {
        }
    }
}
