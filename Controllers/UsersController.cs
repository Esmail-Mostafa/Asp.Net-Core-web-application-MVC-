using DemoTest.DL.Extend;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoTest.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> usermanager;

        public UsersController(UserManager<ApplicationUser> usermanager)
        {
            this.usermanager = usermanager;
        }

        public IActionResult Index()
        {
            var users = usermanager.Users;

            return View(users);
        }

    [HttpGet]
        public async Task<IActionResult>  Edit(string Id)
        {
         var userdata = await usermanager.FindByIdAsync(Id);

            return View(userdata);
        }

        [HttpPost]

        public async Task<IActionResult> Edit(IdentityUser model)
        {
            var users = await usermanager.FindByIdAsync(model.Id);

            users.UserName = model.UserName;
            users.Email = model.Email;

           var result = await usermanager.UpdateAsync(users);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(model);
            }


            
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string Id)
        {
            var userdata = await usermanager.FindByIdAsync(Id);

            return View(userdata);
        }

        public async Task<IActionResult> Delete(IdentityUser model)
        {
            var users = await usermanager.FindByIdAsync(model.Id);

            var data = await usermanager.DeleteAsync(users);

            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Detalis(string Id)
        {
            var userdata = await usermanager.FindByIdAsync(Id);

            return View(userdata);
        }



    }
}
