using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Bank.Pages.Account
{
    public class Transactions_LogModel : PageModel
    {
        private readonly BankDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public Transactions_LogModel(BankDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        public Profile Profile { get; set; }
        public ApplicationUser MyUser { get; set; }
        public List<Transfer> Transfer { get; set; }
        public async Task OnGet()
        {
            if (signInManager.IsSignedIn(User))
            {
                MyUser = userManager.Users.FirstOrDefault(m => m.Email == User.Identity.Name);
                Profile = await context.Profiles.FirstOrDefaultAsync(m => m.Email == MyUser.Email);
                Transfer = await context.Transfers.Where(m => m.MAccountNo == Profile.AccountNo).ToListAsync();




            }

        }
    }
}
