using Microsoft.AspNetCore.Mvc;
using MvcMovie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvcMovie.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MvcMovie.Controllers
{
    public class Account : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly SignInManager<IdentityUser> _signInManager;

        public Account(IAccountRepository accountRepository, SignInManager<IdentityUser> signInManager)
        {
            _accountRepository = accountRepository;
            _signInManager = signInManager;
        }
        [Route("Signup")]
        public IActionResult SignUp()
        {
            return View();
        }

        [Route("Signup")]
        [HttpPost]
        public async Task<IActionResult> SignUp(SignupUser signupUser)
        {
            if (ModelState.IsValid)
            {

                //

                var Result = await _accountRepository.CreateUserAsync(signupUser);
                if (!Result.Succeeded)
                {
                    foreach (var errorMessage in Result.Errors)
                    {
                        ModelState.AddModelError("", errorMessage.Description);
                    }
                    return View(signupUser);
                }
                ModelState.Clear();
            }

            return View();
        }

        [Route("SignIn")]
        public IActionResult SignIn()
        {
            return View();
        }

        [Route("SignIn")]
        [HttpPost]
        public async Task<IActionResult> SignIn(LoginUser loginUser)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.PasswordLoginAsync(loginUser);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Movies");
                }
                ModelState.AddModelError(" ", "Invalid Credentials");
            }

            return View();
        }

        /// <summary>
        /// For Google authentication
        /// </summary>
        /// <returns></returns>

        //[HttpGet]
        //[AllowAnonymous]

        //public async Task<IActionResult> SignIn(string returnUrl)
        //{
            
        //    LoginUser model = new LoginUser
        //    {
        //        ReturnUrl = returnUrl,
        //        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
        //    };
        //    return View(model);
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //public IActionResult ExternalLogin(string provider, string returnUrl)
        //{

        //    var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { returnUrl = returnUrl });
        //    var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        //    return new ChallengeResult(provider, properties); 

        //}

        public async  Task Login()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
            {

                RedirectUri = Url.Action("Index", "Movies")

            }) ;
           
        }

        [Route("Logout")]
        public async Task<ActionResult>  Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("SignIn","Account");
        }
    }
}
