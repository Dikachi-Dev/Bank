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
    public class DetailsModel : PageModel
    {
        private readonly BankDbContext _context;

        public DetailsModel(BankDbContext context)
        {
            _context = context;
        }

        public Transfer Transfer { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Transfer = await _context.Transfers.FirstOrDefaultAsync(m => m.Id == id);

            if (Transfer == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
