using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bank.Models;
using System.Net;
using System.Net.Mail;
using System.IO;

namespace Bank.Pages.Admin.Transactions
{
    public class CreateModel : PageModel
    {
        private readonly BankDbContext context;

        public CreateModel(BankDbContext context)
        {
            this.context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Transfer Transfer { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            context.Transfers.Add(Transfer);
            var reciever = context.Profiles.FirstOrDefault(m => m.AccountNo == Transfer.AccountNo);
            if (Transfer.Bank == "PGC")
            {
                if (Transfer.Type == "Debit")
                {
                    reciever.Balance -= Transfer.Amount;
                }
                reciever.Balance += Transfer.Amount;
            }
            reciever.Balance = reciever.Balance;

            // sed Mail
           
            var Message2 = "You just Recieved a total sum of  "  + Transfer.Amount + ".00 " + "From " + Transfer.MAccountNo + "\n ";         
            string Email2 = reciever.Email;

            //addd tremplate

            StreamReader bd = new StreamReader(@"C:\Projects\NewBank\Bank\Pages\mailbody.html");

            //sender  body
            string Mailtext1 = bd.ReadToEnd();
            Mailtext1 = Mailtext1.Replace("{name}", Email2);
            Mailtext1 = Mailtext1.Replace("{message}", Message2);
            Mailtext1 = Mailtext1.Replace("{subject}", "Transaction Update");

            var credentials = new NetworkCredential("customer-support@bankplc.com", "Money2018$");
            var mail2 = new MailMessage();
            var client = new SmtpClient();

            //mail to reciever 
            mail2 = new MailMessage()
            {
                From = new MailAddress("customer-support@bankplc.com", "Your bank Name"),
                Subject = "Notification from The PGD Bank",
                Body = Mailtext1
            };
            mail2.IsBodyHtml = true;
            mail2.To.Add(new MailAddress(Email2));


            //smtp config
            client = new SmtpClient()
            {
                Port = 8889,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = "mail5013.site4now.net",
                EnableSsl = false,
                Credentials = credentials
            };


            try
            {

            client.Send(mail2); // Send the mail
            }
            catch(Exception e)
            {
                Console.Write(e.Message);
            }


            await context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
