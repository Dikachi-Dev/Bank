using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Bank.Models;

namespace Bank.Pages.Admin.Profiles
{
    public class IndexModel : PageModel
    {
        private readonly BankDbContext _context;

        public IndexModel(BankDbContext context)
        {
            _context = context;
        }

        public IList<Profile> Profile { get;set; }

        public async Task OnGetAsync()
        {
            Profile = await _context.Profiles.ToListAsync();
        }
    }
}
