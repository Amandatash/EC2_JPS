using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BNS.Data;
using BNS.Models;
using BNS.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BNS.Controllers
{
    //[Authorize(Roles ="Teller, Admin")]
    public class TellerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        public TellerController(ApplicationDbContext context,
                                UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Deposit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Deposit(DepositViewModel model) 
        {
            if (ModelState.IsValid)
            {
                var account = _context.Accounts.Where(c=>c.AccountNumber ==  model.Account).FirstOrDefault();

                if (account != null)
                {
                    account.Balance += model.Amount;
                    _context.Update(account);

                    var transaction = new Transactions
                    {
                        Date = DateTime.Now,
                        Time = DateTime.Now,
                        Receiver = model.Account.ToString(),
                        TransactionType = "Teller Deposit",
                        Amount = model.Amount
                    };

                    _context.Transactions.Add(transaction);
                    await _context.SaveChangesAsync();

                    ViewBag.SuccessMessage = "Deposit Successful";
                    return View("Success");
                }

                ModelState.AddModelError(string.Empty, "This account does not exist");
            }
            return View(model);
        }

            [HttpGet]
        public IActionResult Withdraw()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Withdraw(DepositViewModel model)
        {
            if (ModelState.IsValid)
            {
                var account = _context.Accounts.Where(c => c.AccountNumber == model.Account).FirstOrDefault();

                if (account != null)
                {
                    if (account.Balance < model.Amount)
                    {
                        ModelState.AddModelError(string.Empty, "Account balance is insufficient");
                        return View(model);
                    }

                    account.Balance -= model.Amount;
                    _context.Update(account);

                    var transaction = new Transactions
                    {
                        Date = DateTime.Now,
                        Time = DateTime.Now,
                        Receiver = model.Account.ToString(),
                        TransactionType = "Teller Withdrawal",
                        Amount = model.Amount
                    };

                    _context.Transactions.Add(transaction);
                    await _context.SaveChangesAsync();

                    ViewBag.SuccessMessage = "Deposit Successful";
                    return View("Success");
                }

                ModelState.AddModelError(string.Empty, "This account does not exist");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Transactions()
        {
            return View(await _context.Transactions.ToListAsync());
        }

        public IActionResult ListCustomers()
        {
            var users = userManager.Users;
            return View(users);
        }

        public async Task<IActionResult> ViewCustomer(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var account = await _context.Accounts.FindAsync(user.Email);

                if (account == null)
                {
                    ViewBag.ErrorMessage = $"Account with email = {user.Email} cannot be found";
                    return View("NotFound");
                }
                else
                {
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
            }
        }
    }
}
