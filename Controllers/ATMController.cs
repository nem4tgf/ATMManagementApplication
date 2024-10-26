using Microsoft.AspNetCore.Mvc;
using ATMManagementApplication.Models;
using ATMManagementApplication.Data;
using System;
using System.Linq;

namespace ATMManagementApplication.Controller{
    [ApiController]
    [Route("api/atm")]
    public class ATMController : ControllerBase{

        private readonly ATMContext _context;
        public ATMController(ATMContext context){
            _context = context;
        }

        [HttpGet("balance/{customerId}")]
        public IActionResult GetBalance(int customerId){
            var customer = _context.Customers.Find(customerId);
            if (customer == null) return NotFound("Customer not found");
            return Ok(new { balance = customer.Balance });
        }

        [HttpPost("withdraw")]
        public IActionResult Withdraw([FromBody] WithdrawRequest request){
            var customer = _context.Customers.Find(request.CustomerId);
            if (customer == null) return NotFound("Customer not found");

            if (customer.Balance < request.Amount)
                return BadRequest("Insufficient balance");

            customer.Balance -= request.Amount;

            var transaction = new Transaction{
                CustomerId = request.CustomerId,
                Amount = request.Amount,
                Timestamp = DateTime.Now,
                IsSuccessful = true
            };

            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return Ok(new { message = "Withdraw successful", newBalance = customer.Balance });
        }

      
        [HttpPost("deposit")]
        public IActionResult Deposit([FromBody] DepositRequest request){
            var customer = _context.Customers.Find(request.CustomerId);
            if (customer == null) return NotFound("Customer not found");

            customer.Balance += request.Amount;

            var transaction = new Transaction{
                CustomerId = request.CustomerId,
                Amount = request.Amount,
                Timestamp = DateTime.Now,
                IsSuccessful = true
            };

            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return Ok(new { message = "Deposit successful", newBalance = customer.Balance });
        }

        [HttpGet("transactions/{customerId}")]
        public IActionResult GetTransactionHistory(int customerId) {
             var transactions = _context.Transactions
        .Where(t => t.CustomerId == customerId)
        .OrderByDescending(t => t.Timestamp)
        .ToList();
        
    if (!transactions.Any()) return NotFound("No transactions found");

    return Ok(transactions);
    }

    [HttpPost("transfer")]
public IActionResult Transfer([FromBody] TransferRequest request) {
    var sender = _context.Customers.Find(request.SenderId);
    var receiver = _context.Customers.Find(request.ReceiverId);
    
    if (sender == null || receiver == null) return NotFound("Customer not found");
    if (sender.Balance < request.Amount) return BadRequest("Insufficient balance");

    sender.Balance -= request.Amount;
    receiver.Balance += request.Amount;

    // Record transaction for sender and receiver
    var senderTransaction = new Transaction {
        CustomerId = request.SenderId,
        Amount = -request.Amount,
        Timestamp = DateTime.Now,
        IsSuccessful = true
    };

    var receiverTransaction = new Transaction {
        CustomerId = request.ReceiverId,
        Amount = request.Amount,
        Timestamp = DateTime.Now,
        IsSuccessful = true
    };

    _context.Transactions.Add(senderTransaction);
    _context.Transactions.Add(receiverTransaction);
    _context.SaveChanges();

    return Ok(new { message = "Transfer successful", senderBalance = sender.Balance, receiverBalance = receiver.Balance });
}

public class TransferRequest {
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public decimal Amount { get; set; }
}

[HttpPost("change-password")]
public IActionResult ChangePassword([FromBody] ChangePasswordRequest request) {
    var customer = _context.Customers.Find(request.CustomerId);
    if (customer == null) return NotFound("Customer not found");
    if (customer.Password != request.OldPassword) return Unauthorized("Invalid old password");

    customer.Password = request.NewPassword;
    _context.SaveChanges();

    return Ok(new { message = "Password changed successfully" });
}

public class ChangePasswordRequest {
    public int CustomerId { get; set; }
    public required string OldPassword { get; set; }
    public required string NewPassword { get; set; }
}

        }
        

    public class WithdrawRequest{
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
    }

    
    public class DepositRequest{
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
    }
}
