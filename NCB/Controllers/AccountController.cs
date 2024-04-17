using NCB.Models;
using NCB.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using NCB.Data;

namespace NCB.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, 
                                 SignInManager<ApplicationUser> signInManager,
                                 ApplicationDbContext context)
        {
            _context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Logout() 
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateCustomer()
        {
            return View();
        }
        [HttpGet]
        public IActionResult CreateTeller()
        {
            ViewBag.from = "Teller";
            return View("CreateCustomer");
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CreateCutomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { 
                    UserName = model.Email, 
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address
                };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    Random rnd = new Random();
                    int pre = 9505;
                    var account = new Accounts
                    {
                        AccountHolder = model.Email,
                        AccountNumber = rnd.Next(9190000, 9199999),
                        CardNumber = long.Parse(pre.ToString() + rnd.Next(10000000, 99999999).ToString()),
                        AccountType = model.AccountType,
                        Balance = 10000
                    };
                    _context.Add(account);
                    await _context.SaveChangesAsync();
                    await userManager.AddToRoleAsync(user, "Customer");
                    if(signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                        return RedirectToAction("ListUsers", "Administration");
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

       // [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateTeller(CreateCutomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address
                };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Teller");
                    if (signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                        return RedirectToAction("ListUsers", "Administration");
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("CreateCustomer", model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string ReturnURL)
        {
            if(ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    if(!string.IsNullOrEmpty(ReturnURL) && Url.IsLocalUrl(ReturnURL))
                    {
                        return Redirect(ReturnURL);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
