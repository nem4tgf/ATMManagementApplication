using Microsoft.AspNetCore.Mvc;
using ATMManagementApplication.Models;
using ATMManagementApplication.Data;
using System.Linq;


namespace ATMManagementApplication.Controller{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase{

        private readonly ATMContext _context;
        public AuthController(ATMContext context ){
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Customer login){
            var customer = _context.Customers.FirstOrDefault(
                c => c.Name == login.Name && c.Password == login.Password);
            if(customer == null){
                return Unauthorized("Invalid credentials");

            }
            return Ok(new{message = "Login successful", customerId= customer.CustomerId});

        }
        

        [HttpPost("register")]
        public IActionResult Register([FromBody] Customer request)
            {
        
        var existingCustomer = _context.Customers.FirstOrDefault(c => c.Name == request.Name);
        if (existingCustomer != null)
            return BadRequest("User already exists");

        
        var newCustomer = new Customer
        {
            Name = request.Name,
            Password = request.Password, 
            Balance = 0 
        };

        _context.Customers.Add(newCustomer);
        _context.SaveChanges();

        return Ok(new { message = "Registration successful", customerId = newCustomer.CustomerId });
}

    }
}

