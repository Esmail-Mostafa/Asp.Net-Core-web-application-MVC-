using DemoTest.BL.Helper;
using DemoTest.BL.Model;
using DemoTest.DL.Extend;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoTest.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> usermanager;
        private readonly SignInManager<ApplicationUser> signInmanager;

        public AccountController(UserManager<ApplicationUser> usermanager, SignInManager<ApplicationUser> signInmanager)
        {
            this.usermanager = usermanager;
            this.signInmanager = signInmanager;
        }
        #region Registriation


        [HttpGet]
        public IActionResult Registriation()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Registriation(RegistriationVM model)
        {

            try
            {

                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser()
                    {
                        Email = model.Email,
                        UserName = model.Email,
                        IsAgree = model.IsAgree,

                    };
                    var result = await usermanager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("login");

                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }

                }
                return View(model);

            } catch (Exception ex)
            {
                return View(model);

            }

        }
       


        #endregion

        #region login
        [HttpGet]
        public IActionResult login()
        {
            return View();

        }

        [HttpPost]
        public async Task<IActionResult>  login(loginVM model)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var result = await signInmanager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");

                    }
                    else
                    {
                        ModelState.AddModelError("", "User or password invalid");

                    }


                }

                return View(model);
            }catch(Exception ex){

                return View(model);
            }


        }



        #endregion


        #region logout
        [HttpPost]
        public async Task<IActionResult> Logoff()
        {
         await  signInmanager.SignOutAsync();
            return RedirectToAction("login");
        }



        #endregion


        #region forget password

        [HttpGet]
        public IActionResult forgetpassword()
        {
            return View();

        }
        public async Task<IActionResult> ForgetPassword(forgetpasswordVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await usermanager.FindByEmailAsync(model.Email);

                    if (user != null)
                    {

                        var token = await usermanager.GeneratePasswordResetTokenAsync(user);

                        var passwordResetLink = Url.Action("resetpassword", "Account", new { Email = model.Email, Token = token }, Request.Scheme);

                        SendMaillHelpaer.MailSender(new MailVM() { Title = "Password Reset", Message = passwordResetLink, Email = user.Email });


                        return RedirectToAction("ConfirmForgetPassword");
                    }

                 
                }

                return View(model);

            }
            catch (Exception es)
            {
                return View(model);
            }
        }



        [HttpPost]



        [HttpGet]
        public IActionResult ConfirmForgetPassword()
        {
            return View();
        }

        #endregion



        #region reset password


        [HttpGet]
        public IActionResult resetpassword()
        {
            return View();

        }

        public async Task<IActionResult> ResetPassword(resetpasswordVM model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await usermanager.FindByEmailAsync(model.Email);

                    if (user != null)
                    {
                        var result = await usermanager.ResetPasswordAsync(user, model.Token, model.Password);

                        if (result.Succeeded)
                        {
                            return RedirectToAction("ConfirmResetPassword");
                        }

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                        return View(model);
                    }

                    return RedirectToAction("ConfirmResetPassword");
                }

                return View(model);

            }
            catch (Exception es)
            {
                return View(model);
            }
        }


        [HttpGet]
        public IActionResult ConfirmResetPassword()
        {
            return View();
        }

        #endregion











    }
}
