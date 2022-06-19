using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class ApplicationUser : IdentityUser
    {
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
        public DateTime Created { get; set; }
        public string AccountStatus { get; set; }
        [Required]
        public string AccountType { get; set; }
    }
}
