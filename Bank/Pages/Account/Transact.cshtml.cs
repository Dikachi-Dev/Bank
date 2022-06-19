using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bank.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;

namespace Bank.Pages.Account
{
    public class CreateModel : PageModel
    {
        private readonly BankDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public CreateModel(BankDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        public Profile Profile { get; set; }
        public ApplicationUser MyUser { get; set; }



        public async Task<IActionResult> OnGet()
        {
            if (signInManager.IsSignedIn(User))
            {
                MyUser = userManager.Users.FirstOrDefault(m => m.Email == User.Identity.Name);
                Profile = await context.Profiles.FirstOrDefaultAsync(m => m.Email == MyUser.Email);
            }
            return Page();
        }

        [BindProperty]
        public Transfer Transfer { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                if (signInManager.IsSignedIn(User))
                {
                    MyUser = userManager.Users.FirstOrDefault(m => m.Email == User.Identity.Name);
                    Profile = await context.Profiles.FirstOrDefaultAsync(m => m.Email == MyUser.Email);

                    var transfer = new Transfer()
                    {
                        MAccountNo = Profile.AccountNo,
                        AccountNo = Transfer.AccountNo,
                        Amount = Transfer.Amount,
                        TxnDate = DateTime.Now,

                        Description = Transfer.Description,
                        Bank = Transfer.Bank,

                        Status = "Pending",
                        Type = "Debit"
                    };
                    await context.Transfers.AddAsync(transfer);
                    Profile.Balance -= transfer.Amount;
                    var reciever = context.Profiles.FirstOrDefault(m => m.AccountNo == transfer.AccountNo);
                    if (Transfer.Bank == "Bank")
                    {
                        reciever.Balance += transfer.Amount;
                    }
                    await context.SaveChangesAsync();

                    // sed Mail
                    var Message1 = "You just Sent a total sum of  " + MyUser.AccountType + transfer.Amount + ".00 " + "To " + transfer.AccountNo + "\n " + "Your interbank transfer was succesful.";
                    var Message2 = "You just Recieved a total sum of  " + MyUser.AccountType + transfer.Amount + ".00 " + "From " + Profile.AccountNo + "\n ";
                    string Email1 = MyUser.Email;
                    string Email2 = reciever.Email;
                    var credentials = new NetworkCredential("customer-support@pgcbankplc.com", "Money2018$");
                    var mail1 = new MailMessage();
                    var mail2 = new MailMessage();
                    var client = new SmtpClient();


                    //mail to sender 
                    mail1 = new MailMessage()
                    {
                        From = new MailAddress("customer-support@pgcbankplc.com", "Providence global corporation Bank"),
                        Subject = "Notification from The Bank",
                        Body = Message1
                    };
                    mail1.IsBodyHtml = true;
                    mail1.To.Add(new MailAddress(Email1));

                    //mail to reciever 
                    mail2 = new MailMessage()
                    {
                        From = new MailAddress("customer-support@pgcbankplc.com", "Providence global corporation Bank"),
                        Subject = "Notification from The Bank",
                        Body = Message2
                    };
                    mail2.IsBodyHtml = true;
                    mail2.To.Add(new MailAddress(Email2));


                    //smtp config
                    client = new SmtpClient()
                    {
                        Port = 25,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Host = "mail.pgcbankplc.com",
                        EnableSsl = false,
                        Credentials = credentials
                    };



                    client.SendAsync(mail1, mail2); // Send the mail


                    return RedirectToPage("./Success");
                }
            }
            return Page();
        }
    }
}
