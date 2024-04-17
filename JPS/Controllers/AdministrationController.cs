using JPS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JPS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id); 

            if (user == null) 
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded) 
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description); 
                }

                return View("ListUsers");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);  

            if (role == null) //if the user is null/cannot be found
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await roleManager.DeleteAsync(role);

                if (result.Succeeded) //if the delete is successfull
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description); //add the errors to the model state 
                }

                return View("ListRoles");
            }
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = userManager.Users; //get a list of all users on the user manager server
            return View(users);
        }


        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            var userClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles.ToList(),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.Id = user.Id;
                user.Email = model.Email;
                user.UserName = model.UserName;

                var result = await userManager.UpdateAsync(user); 

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole (CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole 
                {
                    Name = model.RoleName 
                };

                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded) 
                {
                    return RedirectToAction("ListRoles", "Administration"); 
                }

                foreach (IdentityError error in result.Errors) 
                {
                    ModelState.AddModelError("", error.Description); 
                }
            }

            return View(model); 
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles); //pass the list of all roles 
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string Id) //gets role Id to edit role
        {
            var role = await roleManager.FindByIdAsync(Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id: {Id} cannot be found";
                return View("NotFound");
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            foreach (var user in userManager.Users) //loop through each user and retrieve all the user from the database
            {
                if (await userManager.IsInRoleAsync(user, role.Name)) //if user is in the role
                {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id); //retrieve role from the databse by its Id

            if (role == null) //check if the role is still in the database 
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else //if role is not null
            {
                role.Name = model.RoleName; //update name property of the role object
                var result = await roleManager.UpdateAsync(role); //update the role informaton in the database 

                if (result.Succeeded) //if the result.Succeeded is true, the data is successfully updated
                {
                    return RedirectToAction("ListRoles"); //rediret user to list roles action
                }

                foreach (var error in result.Errors) //if there is any errors updating the dat, look through each error in the errors collection
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId) //using roleId it recieves roleId from the database
        {
            ViewBag.roleId = roleId; //roleid is stored in a viewbag

            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null) //the the role is not found
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRoleViewModel>(); //if the role is found, cerate a list of the user role view model objects

            foreach (var user in userManager.Users) //Loop through each user, create an instance of UserRoleViewModel
            {
                var userRoleViewModel = new UserRoleViewModel //populate userId and username properties
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await userManager.IsInRoleAsync(user, role.Name)) //check if a user is a memnber of the given role
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel); //add the user to the user role thats selected
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId) //see the valuses passed by the parameters when you click update
        {
            //using the roleId to retrieve the respective role from the database
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null) //if the role is not found
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("Not Found");
            }

            for (int i = 0; i < model.Count; i++) //if the role is found, use the for loop to look through each user role view model object that is in the model 
            {
                var user = await userManager.FindByIdAsync(model[i].UserId); //retrieve the respective user from the database

                IdentityResult result = null;

                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name))) //if the user is selected and he is not a member of the given role then the user is added to the role
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name)) //if the user is not selected and he is a member of the given role then the user is removed to the role
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name); //remove user from the role
                }
                else
                {
                    continue; //continue processing 
                }

                if (result.Succeeded) //if succeeded if true 
                {
                    if (i < (model.Count - 1)) //check if the loop variable i is less than model count -1
                        continue; //if it is true then continue 
                    else
                        return RedirectToAction("EditRole", new { Id = roleId });
                }
            }

            //if the model parameter is empty, send the user back to edit role 
            return RedirectToAction("EditROle", new { Id = roleId });
        }
    }
}
