using BlogEngineSGC.Models;
using BlogEngineSGC.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using BlogEngineSGC.CommonClasses;

namespace BlogEngineSGC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUserService userServices;

        public AccountController(IUserService userServices) => this.userServices = userServices;

        [Route("/login")]
        [AllowAnonymous]
        [HttpGet]
        [SuppressMessage("Design", "CA1054:Uri parameters should not be strings", Justification = "MVC binding")]
        public IActionResult Login(string? returnUrl = null)
        {
            this.ViewData[Constants.ReturnUrl] = returnUrl;
            return this.View();
        }

        [Route("/login")]
        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        [SuppressMessage("Design", "CA1054:Uri parameters should not be strings", Justification = "MVC binding")]
        public async Task<IActionResult> LoginAsync(string? returnUrl, LoginViewModel? model)
        {
            this.ViewData[Constants.ReturnUrl] = returnUrl;

            if (model is null || model.UserName is null || model.Password is null)
            {
                //this.ModelState.AddModelError(string.Empty, Properties.Resources.UsernameOrPasswordIsInvalid);
                return this.View(nameof(Login), model);
            }

            if (!this.ModelState.IsValid || !this.userServices.ValidateUser(model.UserName, model.Password))
            {
                //this.ModelState.AddModelError(string.Empty, Properties.Resources.UsernameOrPasswordIsInvalid);
                return this.View(nameof(Login), model);
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, model.UserName));

            var principle = new ClaimsPrincipal(identity);
            var properties = new AuthenticationProperties { IsPersistent = model.RememberMe };
            await this.HttpContext.SignInAsync(principle, properties).ConfigureAwait(false);

            return this.LocalRedirect(returnUrl ?? "/");
        }

        [Route("/logout")]
        public async Task<IActionResult> LogOutAsync()
        {
            await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(false);
            return this.LocalRedirect("/");
        }
    }
}
