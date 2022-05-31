using System;
using System.Collections.Generic;
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
using TGWLP.BLL.Interfaces;
using TGWLP.BLL.Models;
using TGWLP.DAL.Entities;
using TGWLP.DAL.Interfaces;

namespace TGWLP.Pages.Story
{
    [AllowAnonymous]
    public class IndexModel : PageBase
    {
        public StoryViewModel Story { get; set; }
        private IBookService _bookService { get; }
        public IndexModel(IAppContext context, IConfiguration config, UserManager<User> usermanager, RoleManager<Role> rolemanager, IWebHostEnvironment environment, IHttpContextAccessor httpcontextaccessor, IMapper mapper, CultureLocalizer localizer, ILogger<IndexModel> logger, SignInManager<DAL.Entities.User> signinmanager, IOptions<AppSettings> settings, IBookService bookService) : base(context, config, usermanager, rolemanager, environment, httpcontextaccessor, mapper, localizer, logger, settings) {
            _bookService = bookService;
        }

        public async Task OnGetAsync(Guid id)
        {
            await InitializePageAsync(id);
        }

        private async Task InitializePageAsync(Guid id) {
            await SetStoryAsync(id);
            SetPageTitle();
        }

        private async Task SetStoryAsync(Guid id) {
            var storyEntity = this.AppContext.Stories.Single(x => x.Id == id);
            var story = this.Mapper.Map(storyEntity, this.Story);
            if (!string.IsNullOrEmpty(story.BookId))
            {
                story.BookViewModel = await _bookService.GetBookInformationByIdAsync(story.BookId);
            }
            this.Story = story;
        }

        protected override void SetPageTitle() {
            if (this.Story != null) {
                PageTitle = this.Story.Title;
            }
        }
    }
}
