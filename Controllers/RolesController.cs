using DemoTest.BL.Model;
using DemoTest.DL.Extend;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoTest.Controllers
{
    public class RolesController : Controller
    {
        private readonly UserManager<ApplicationUser> managerusers;
        private readonly RoleManager<IdentityRole> rolesmanager;

        public RolesController(UserManager<ApplicationUser> managerusers , RoleManager<IdentityRole> rolesmanager)
        {
            this.managerusers = managerusers;
            this.rolesmanager = rolesmanager;
        }
        public IActionResult Index()
        {
            var users = rolesmanager.Roles;

            return View(users);
        }
   

        [HttpGet]
        public  IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>  Create(IdentityRole model)
        {

            var role = new IdentityRole()
            {
                Name = model.Name,
                NormalizedName = model.Name.ToUpper()

             };

            var rusalt = await rolesmanager.CreateAsync(role);
            if (rusalt.Succeeded)
            {
                return RedirectToAction("Index");


            }
            else
            {
                foreach (var item in rusalt.Errors)
                {
                    ModelState.AddModelError("", item.Description);

                }

            }

            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {
            var rusalt = await rolesmanager.FindByIdAsync(Id);
            
            return View(rusalt);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(IdentityRole model)
        {
            var role = await rolesmanager.FindByIdAsync(model.Id);

            role.Name = model.Name;
            role.NormalizedName = model.NormalizedName;
          

            var rusalt = await rolesmanager.UpdateAsync(role);
            if (rusalt.Succeeded)
            {
                return RedirectToAction("Index");


            }
            else
            {
                foreach (var item in rusalt.Errors)
                {
                    ModelState.AddModelError("", item.Description);

                }

            }

            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUsers(string RoleId)
        {

            ViewBag.RoleId = RoleId;

            var role = await rolesmanager.FindByIdAsync(RoleId);

            var model = new List<UserInRolesVM>();

            foreach (var user in managerusers.Users)
            {
                var userInRole = new UserInRolesVM()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await managerusers.IsInRoleAsync(user, role.Name))
                {
                    userInRole.IsSelected = true;
                }
                else
                {
                    userInRole.IsSelected = false;
                }

                model.Add(userInRole);
            }

            return View(model);

        }


        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(List<UserInRolesVM> model, string RoleId)
        {

            var role = await rolesmanager.FindByIdAsync(RoleId);

            for (int i = 0; i < model.Count; i++)
            {

                var user = await managerusers.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await managerusers.IsInRoleAsync(user, role.Name)))
                {

                    result = await managerusers.AddToRoleAsync(user, role.Name);

                }
                else if (!model[i].IsSelected && (await managerusers.IsInRoleAsync(user, role.Name)))
                {
                    result = await managerusers.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (i < model.Count)
                    continue;
            }

            return RedirectToAction("Index", new { id = RoleId });
        }

    }
}
