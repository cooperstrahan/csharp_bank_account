using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAccounts.Models
{
    public class UserTransact
    {
        [Required]
        public int Amount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
        public List<Transaction> Transactions { get; set; }
        public UserTransact() {}
        public UserTransact(User user)
        {
            UserId = user.UserID;
            User = user;
            Transactions = user.Transactions;
        }
    }
}