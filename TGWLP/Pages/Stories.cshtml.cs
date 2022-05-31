using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
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
using TGWLP.BLL.Models;
using TGWLP.DAL.Entities;
using TGWLP.DAL.Interfaces;

namespace TGWLP.Pages
{
    [AllowAnonymous]
    [Display(Name = "Stories")]

    public class StoriesModel : PageBase
    {
        public List<StoryViewModel> Stories { get; set; }
        public StoriesModel(IAppContext context, IConfiguration config, UserManager<User> usermanager, RoleManager<Role> rolemanager, IWebHostEnvironment environment, IHttpContextAccessor httpcontextaccessor, IMapper mapper, CultureLocalizer localizer, ILogger<PageBase> logger, SignInManager<DAL.Entities.User> signinmanager, IOptions<AppSettings> settings) : base(context, config, usermanager, rolemanager, environment, httpcontextaccessor, mapper, localizer, logger, settings) {
        }

        public void OnGet()
        {
            InitializePage();
        }

        private void InitializePage() {
            GetStories();
        }

        /// <summary>
        /// Set the story list for the page.
        /// </summary>
        private void GetStories() {
            if (this.LoggedIn)
            {
                // If user has logged in, they can view all published and unpublished stories
                this.Stories = AppContext.Stories.Select(x => Mapper.Map(x, new StoryViewModel())).ToList().OrderByDescending(x => x.PublishDate).ToList();
            }
            else
            {
                // If user has not logged in, they can only view stories that have already been published
                var today = DateTime.Now.Date;
                this.Stories = AppContext.Stories.Where(s => s.PublishDate.HasValue && s.PublishDate.Value <= today).Select(x => Mapper.Map(x, new StoryViewModel())).ToList().OrderByDescending(x => x.PublishDate).ToList();
            }
        }
    }
}
