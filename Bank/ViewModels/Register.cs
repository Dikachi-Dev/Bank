using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.ViewModels
{
    public class Register 
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public  string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage ="Password didnt match")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string MarriageStatus { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Occupation { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public  string PhoneNumber { get; set; }

        public DateTime Created { get; set; }
        public string AccountStatus { get; set; }
        [Required]
        public string AccountType { get; set; }

    }
}
