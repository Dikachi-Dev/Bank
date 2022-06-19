using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Models;
using Bank.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bank.Pages
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly BankDbContext context;

        [BindProperty]
        public Login  Model { get; set; }
        public ApplicationUser MyUser { get; private set; }

        public LoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, BankDbContext context)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.context = context;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string returnurl = null)
        {
            if (ModelState.IsValid)
            {
                var IdentityResult = await signInManager.PasswordSignInAsync(Model.Email, Model.Password, Model.RememberMe, false);
                if (IdentityResult.Succeeded)
                {

                    MyUser = userManager.Users.FirstOrDefault(m => m.Email == Model.Email);

                    if (await userManager.IsInRoleAsync(MyUser, "Admin"))
                    {
                        if (returnurl == null || returnurl == "/")
                        {                          

                            return RedirectToPage("Index");
                        }
                        else
                        {
                            return RedirectToPage(returnurl);
                        }
                    }
                    else
                    {
                        if (returnurl == null || returnurl == "/")
                        {                           
                            return RedirectToPage("Account/Dashboard");
                        }
                        else
                        {
                            return RedirectToPage(returnurl);
                        }
                    }



                }
                ModelState.AddModelError("", "Username or Password ois incorrect");
            }
            return Page();
        }
    }
}
