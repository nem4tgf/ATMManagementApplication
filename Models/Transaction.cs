using System.ComponentModel.DataAnnotations;
using System;


namespace ATMManagementApplication.Models{
    public class Transaction{
        [Key]
        public int TransactionId { get; set; }
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsSuccessful { get; set; }
    }
}

