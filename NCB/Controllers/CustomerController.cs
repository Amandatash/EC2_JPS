using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NCB.Data;
using NCB.Models;
using NCB.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Controllers
{
    public class CustomerController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext _context;

        public CustomerController(UserManager<ApplicationUser> userManager,
                                  ApplicationDbContext context)
        {
            this.userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Details()
        {
            var userName = User.Identity.Name;
            var user = await userManager.FindByNameAsync(userName);
            var account = _context.Accounts.Where(c => c.AccountHolder == user.Email).FirstOrDefault();
            var model = new ViewCustomerViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                AccountNumber = account.AccountNumber,
                AccountHolder = account.AccountHolder,
                CardNumber = account.CardNumber,
                AccountType = account.AccountType,
                Balance = account.Balance
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Transactions()
        {
            var userName = User.Identity.Name;
            var user = await userManager.FindByNameAsync(userName);
            var account = await _context.Accounts.FindAsync(user.Email);
            ViewData["UserAccount"] = account.AccountNumber;
            return View(await _context.Transactions.ToListAsync());
        }
    }
}
