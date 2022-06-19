using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class Transfer
    {
        public int Id { get; set; }
        [Required]
        public string AccountNo { get; set; }
        public string MAccountNo { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public DateTime TxnDate { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Bank { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }


    }
}
