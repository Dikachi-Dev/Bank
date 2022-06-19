using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bank.Pages.Account
{
    public class MyProfileModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<MyProfileModel> logger;
        private readonly BankDbContext context;
        public ApplicationUser MyUser;

        public Profile Profile { get; private set; }

        public MyProfileModel(
            BankDbContext context, UserManager<ApplicationUser> userManager,
            ILogger<MyProfileModel> logger)
        {
            this.userManager = userManager;
            this.logger = logger;
            this.context = context;
        }
        public async Task<IActionResult> OnGet()
        {
             MyUser = await userManager.GetUserAsync(User);
            Profile = await context.Profiles.FirstOrDefaultAsync(m => m.Email == MyUser.Email);
            //if (user == null)
            //{
            //    return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            //}

            return Page();
        }
    }
}
