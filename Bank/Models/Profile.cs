using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public decimal Balance { get; set; }
        public string AccountNo { get; set; }
        public string CardNo { get; set; }
        public string CVV { get; set; }
        public string CardExpireDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        //public string IpAdrress { get; set; }
        public string Status { get; set; }

    }
}
