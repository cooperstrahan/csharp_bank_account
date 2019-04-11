using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAccounts.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }
        [Required]
        public int Amount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }     

        public Transaction() {}

        public Transaction(UserTransact newTrans)
        {
            Amount = newTrans.Amount;
            CreatedAt = newTrans.CreatedAt;
            UserId = newTrans.UserId;
            User = newTrans.User;
        }   
    }
}