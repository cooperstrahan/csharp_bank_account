using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankAccounts.Models;
using Microsoft.EntityFrameworkCore;

namespace BankAccounts.Controllers
{
    public class BankController : Controller
    {
        private UserContext dbContext;
        public BankController(UserContext context)
        {
            dbContext = context;
        }
        
        [HttpGet("account/{id}")]
        public IActionResult Account(int id)
        {
            int? UserLoggedIn = LoggedIn.GetUserID(HttpContext);
            if(UserLoggedIn == id)
            {   
                User AUser = dbContext.Users
                    .Include(u => u.Transactions)
                    .FirstOrDefault(user => user.UserID == id);
                
                UserTransact currentUser = new UserTransact(AUser);

                return View(currentUser);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
            
        }

        [HttpPost("transaction")]
        public IActionResult Transact(UserTransact newTransaction)
        {
            User User = dbContext.Users
                .Include(data => data.Transactions)
                .FirstOrDefault(user => user.UserID == newTransaction.UserId);

            int AccountBalance = User.Transactions.Sum(transaction => transaction.Amount);
            newTransaction.User = User;            

            if(ModelState.IsValid)
            {
                if(AccountBalance + newTransaction.Amount < 0)
                {
                    ModelState.AddModelError("Amount", "You may not overdraw from your account");
                    System.Console.WriteLine(newTransaction.User.FirstName);
                    return View("Account", newTransaction);
                }
                else
                {
                    Transaction Debit = new Transaction(newTransaction);
                    dbContext.Transactions.Add(Debit);
                    dbContext.SaveChanges();
                    
                    return RedirectToAction("Account", new {id = newTransaction.UserId});
                }
            }
            else
            {
                return RedirectToAction("Account", new {id = newTransaction.UserId});
            }   
        }

        // public UserTransact RetrieveCurrentUser(int id)
        // {
        //     User AUser = dbContext.Users
        //             .Include(u => u.Transactions)
        //             .FirstOrDefault(user => user.UserID == id);
                
        //     UserTransact currentUser = new UserTransact(AUser);
        //     return currentUser;
        // } 



        [HttpGet("/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }

    }
}
