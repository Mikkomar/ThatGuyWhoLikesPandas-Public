using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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

namespace TGWLP.Pages.Story
{
    public class EditModel : PageBase
    {
        public StoryEditModel Story { get; set; }
        public EditModel(IAppContext context, IConfiguration config, UserManager<User> usermanager, RoleManager<Role> rolemanager, IWebHostEnvironment environment, IHttpContextAccessor httpcontextaccessor, IMapper mapper, CultureLocalizer localizer, ILogger<EditModel> logger, SignInManager<DAL.Entities.User> signinmanager, IOptions<AppSettings> settings) : base(context, config, usermanager, rolemanager, environment, httpcontextaccessor, mapper, localizer, logger, settings) {
        }

        public void OnGet(Guid id)
        {
            InitializePage(id);
        }

        private void InitializePage(Guid id) {
            SetStoryModel(id);
        }

        private void SetStoryModel(Guid id) {
            if (id == Guid.Empty) {
                this.Story = new StoryEditModel();
            }
            else {
                var storyEntity = this.AppContext.Stories.Single(x => x.Id == id);
                this.Story = this.Mapper.Map(storyEntity, this.Story);
            }
        }

        public IActionResult OnPostSave(Guid id, StoryEditModel Story) {
            if (ModelState.IsValid) {

                var storyEntity = AppContext.Stories.SingleOrDefault(x => x.Id == id);
                storyEntity = this.Mapper.Map(Story, storyEntity);

                if (storyEntity.IsNewEntity()) {
                    storyEntity.Created = DateTime.Now;
                    storyEntity.Creator = User.Id;
                }
                else {
                    storyEntity.Edited = DateTime.Now;
                    storyEntity.Editor = User.Id;
                }

                storyEntity.SaveDate = DateTime.Now;

                AppContext.AddOrUpdate(storyEntity);
                AppContext.SaveChanges();

                return Redirect("/Stories");
            }
            else {
                this.Story = Story;
                return Page();
            }
        }
    }
}
