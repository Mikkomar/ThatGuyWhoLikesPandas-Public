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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TGWLP.BLL.Interfaces;
using TGWLP.BLL.Models;
using TGWLP.DAL.Entities;
using TGWLP.DAL.Interfaces;

namespace TGWLP.Pages
{
    [AllowAnonymous]
    [Display(Name = "MainPage")]
    public class IndexModel : PageBase
    {
        public List<StoryViewModel> PublishedStories { get; set; } 
        public Dictionary<int, List<StoryViewModel>> PublishedStoriesByYear { get; set; }
        public IBookService _bookService { get; set; }

        public IndexModel(IAppContext context, IConfiguration config, UserManager<User> usermanager, RoleManager<Role> rolemanager, IWebHostEnvironment environment, IHttpContextAccessor httpcontextaccessor, IMapper mapper, CultureLocalizer localizer, ILogger<IndexModel> logger, SignInManager<DAL.Entities.User> signinmanager, IOptions<AppSettings> settings, IBookService bookService) : base(context, config, usermanager, rolemanager, environment, httpcontextaccessor, mapper, localizer, logger, settings) {
            _bookService = bookService;
        }

        public async Task OnGetAsync() {
            await InitializePageAsync();
        }

        private async Task InitializePageAsync() {
            await SetPublishedStoriesAsync();
        }

        /// <summary>
        /// Sets the list of published stories.
        /// </summary>
        /// <returns></returns>
        private async Task SetPublishedStoriesAsync() {
            PublishedStories = new List<StoryViewModel>();
            PublishedStoriesByYear = new Dictionary<int, List<StoryViewModel>>();
            var today = DateTime.Now.Date;
            var storyEntities = AppContext.Stories.Where(x => x.PublishDate.HasValue && x.PublishDate.Value.Date <= today);
            foreach (var story in storyEntities) {
                var storyModel = new StoryViewModel();
                storyModel = Mapper.Map(story, storyModel);
                if (!string.IsNullOrEmpty(storyModel.BookId))
                {
                    storyModel.BookViewModel = await _bookService.GetBookInformationByIdAsync(storyModel.BookId);
                }
                PublishedStories.Add(storyModel);
            }
            PublishedStories = PublishedStories.OrderByDescending(x => x.PublishDate).ToList();
            foreach(var year in PublishedStories.Select(x => x.PublishDate.Value.Year).Distinct().ToList()) {
                PublishedStoriesByYear.Add(year, PublishedStories.Where(x => x.PublishDate.Value.Year == year).ToList());
            }
        }
    }
}
