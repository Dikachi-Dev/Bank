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
            if (Transfer.Bank == "Bank")
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
            var credentials = new NetworkCredential("customer-support@pgcbankplc.com", "Money2018$");
            var mail2 = new MailMessage();
            var client = new SmtpClient();

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



            client.Send(mail2); // Send the mail


            await context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
