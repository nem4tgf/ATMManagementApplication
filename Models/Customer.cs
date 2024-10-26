using System.ComponentModel.DataAnnotations;

namespace ATMManagementApplication.Models{
    public class Customer{
        //Annotation ==> Primary key
        [Key]
        public int CustomerId {get; set;}
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Password { get; set; }
        
        public decimal Balance { get; set; }
    }
}