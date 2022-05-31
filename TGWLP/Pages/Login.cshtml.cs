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
using TGWLP.DAL.Entities;
using TGWLP.DAL.Interfaces;
using TGWLP.DAL.Repositories;

namespace TGWLP.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageBase
    {
        private SignInManager<DAL.Entities.User> SignInManager { get; }
        private LoginRepository LoginRepository { get; set; }
        public TGWLP.BLL.Models.LoginModel LoginInfoModel { get; set; }

        public LoginModel(IAppContext context, IConfiguration config, UserManager<User> usermanager, RoleManager<Role> rolemanager, IWebHostEnvironment environment, IHttpContextAccessor httpcontextaccessor, IMapper mapper, CultureLocalizer localizer, ILogger<LoginModel> logger, SignInManager<DAL.Entities.User> signinmanager, IOptions<AppSettings> settings) : base(context, config, usermanager, rolemanager, environment, httpcontextaccessor, mapper, localizer, logger, settings) {
            this.LoginRepository = new LoginRepository(context, config);
            this.SignInManager = signinmanager;
        }

        public void OnGet()
        {
            InitializePage();
        }

        private void InitializePage() {
            this.LoginInfoModel = new TGWLP.BLL.Models.LoginModel();
        }

        /// <summary>
        /// Logs the user in using username and password
        /// </summary>
        /// <param name="LoginInfoModel"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostLogin(TGWLP.BLL.Models.LoginModel LoginInfoModel) {
            string RequestIP = Request.Headers["X-Forwarded-For"].ToString();

            var user = await UserManager.FindByNameAsync(LoginInfoModel.UserName);

            // Restrict visitors from brute forcing passwords 
            if (this.LoginRepository.CanTryLogin(DateTime.Now, RequestIP)) {
                if (user != null) {
                    var signInResult = await SignInManager.PasswordSignInAsync(user, LoginInfoModel.Password, true, false);

                    if (signInResult.Succeeded) {
                        // Always logs successful logins
                        this.LoginRepository.LogLoginAttempt(LoginInfoModel.UserName, DateTime.Now, RequestIP, true);
                        return Redirect("/");
                    }
                }

                // Logs unsuccessful logins
                this.LoginRepository.LogLoginAttempt(LoginInfoModel.UserName, DateTime.Now, RequestIP, false);
                ModelState.AddModelError(string.Empty, this.Localizer["Error_Login_WrongPassword"]);
                return Page();
            }
            else {
                ModelState.AddModelError(string.Empty, this.Localizer["Error_Login_TooManyTries"].ToString()
                    .Replace("{0}", this.Configuration.GetSection("LoginSettings")["MaxAttemptedLogins"].ToString())
                    .Replace("{1}", this.Configuration.GetSection("LoginSettings")["TimeBetweenLoginsMinutes"]));
                return Page();
            }
        }

        // DISABLED FOR SECURITY REASONS
        //public async Task<IActionResult> OnPostSignUp(TGWLP.Models.LoginModel LoginInfoModel) {
        //    return Page();
        //}
    }
}
