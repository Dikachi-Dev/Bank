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
using System.IO;

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
                    if (Transfer.Bank == "PGC")
                    {
                        reciever.Balance += transfer.Amount;
                    }
                    await context.SaveChangesAsync();
                    try
                    {
                        StreamReader bd = new(@"C:\Projects\NewBank\Bank\Pages\mailbody.html");
                        var credentials = new NetworkCredential("customer-support@bankplc.com", "Money2018$");
                        var mail1 = new MailMessage();
                        //var mail2 = new MailMessage();
                        var client = new SmtpClient();


                        // sed Mail
                        var Message1 = "You just Sent a total sum of  " + MyUser.AccountType + transfer.Amount + ".00 " + "To " + transfer.AccountNo + "</br>" + "Your interbank transfer was succesful.";

                        string Email1 = MyUser.Email;

                        //sender  body
                        string Mailtext1 = bd.ReadToEnd();
                        Mailtext1 = Mailtext1.Replace("{name}", Email1);
                        Mailtext1 = Mailtext1.Replace("{message}", Message1);
                        Mailtext1 = Mailtext1.Replace("{subject}", "Transaction Update");
                        //mail to sender 
                        mail1 = new MailMessage()
                        {
                            From = new MailAddress("customer-support@bankplc.com", "Your bank Name"),
                            Subject = "Notification from The PGC Bank",
                            Body = Mailtext1
                        };
                        mail1.IsBodyHtml = true;
                        mail1.To.Add(new MailAddress(Email1));
                        //smtp config
                        client = new SmtpClient()
                        {
                            Port = 8889,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Host = "mail5013.site4now.net",
                            EnableSsl = false,
                            Credentials = credentials,
                            Timeout = 100
                        };
                        client.Send(mail1);
                        //if (Transfer.Bank=="PGC" && Transfer.AccountNo == reciever.AccountNo)
                        //{
                        //    string Email2 = reciever.Email;
                        //    var Message2 = "You just Recieved a total sum of  " + MyUser.AccountType + transfer.Amount + ".00 " + "From " + Profile.AccountNo + "\n ";
                        //    //reciever body
                        //    string Mailtext2 = bd.ReadToEnd();
                        //    Mailtext2 = Mailtext2.Replace("{name}", Email2);
                        //    Mailtext2 = Mailtext2.Replace("{message}", Message2);
                        //    Mailtext2 = Mailtext2.Replace("{subject}", "Transaction Update");
                        //    //mail to reciever 
                        //    mail2 = new MailMessage()
                        //    {
                        //        From = new MailAddress("customer-support@bankplc.com", "Your bank Name"),
                        //        Subject = "Notification from The PGC Bank",
                        //        Body = Mailtext2
                        //    };
                        //    mail2.IsBodyHtml = true;
                        //    mail2.To.Add(new MailAddress(Email2));

                        //    client = new SmtpClient()
                        //    {
                        //        Port = 8889,
                        //        DeliveryMethod = SmtpDeliveryMethod.Network,
                        //        UseDefaultCredentials = false,
                        //        Host = "mail5013.site4now.net",
                        //        EnableSsl = false,
                        //        Credentials = credentials,
                        //        Timeout = 100


                        //    };
                        //    client.Send(mail2);


                        //}


                       
                    }
                    catch(Exception e )
                    {
                        Console.Write(e);
                    }
                    return RedirectToPage("./Success");
                }
            }
            return Page();
        }
    }
}
