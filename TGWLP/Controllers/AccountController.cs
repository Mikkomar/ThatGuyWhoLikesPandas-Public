using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TGWLP.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<DAL.Entities.User> SignInManager { get; }
        private ILogger<AccountController> Logger { get; }

        public AccountController(ILogger<AccountController> logger, SignInManager<DAL.Entities.User> signinmanager) {
            Logger = logger;
            SignInManager = signinmanager;
        }

        public IActionResult SingIn() {
            return new OkResult();
        }

        public IActionResult LogOut() {
            SignInManager.SignOutAsync();
            return Redirect("/");
        }
    }
}
