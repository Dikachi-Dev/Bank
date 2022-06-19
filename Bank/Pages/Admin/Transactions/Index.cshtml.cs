using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Bank.Models;

namespace Bank.Pages.Admin.Transactions
{
    public class IndexModel : PageModel
    {
        private readonly BankDbContext _context;

        public IndexModel(BankDbContext context)
        {
            _context = context;
        }

        public IList<Transfer> Transfer { get;set; }

        public async Task OnGetAsync()
        {
            Transfer = await _context.Transfers.ToListAsync();
        }
    }
}
