using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bank.Pages
{
    public class ContactModel : PageModel
    {
        public void OnGet()
        {
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Message { get; set; }
            public string Subject { get; set; }
        }
        public void OnPost()
        {
            


                string message = "Mail From : " + "  " + Input.Name + "\n " + "Sender Email : " + "  " + Input.Email + "\n " + " Message : " + Input.Message;
                // Credentials
                var credentials = new NetworkCredential("customer-support@bankplc.com", "Money2018$");


                //string FilePath = "C:\\Users\\YunAir\\source\\repos\\UnitedCreditUnionBank\\mailtemp.html";

                //StreamReader str = new StreamReader(FilePath);

                //string MailText = str.ReadToEnd();

                //str.Close();



                ////Repalce [newusername] = signup user name   
                //MailText = MailText.Replace("[mess]", mess);

                // Mail message
                var mail = new MailMessage()
                {
                    From = new MailAddress("customer-support@bankplc.com"),
                    Subject = Input.Subject,
                    Body = message
                };

                mail.IsBodyHtml = false;
                mail.To.Add(new MailAddress("customer-support@bankplc.com"));

                // Smtp client
                var client = new SmtpClient()
                {
                    Port = 8889,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = "mail5013.site4now.net",
                    EnableSsl = false,
                    Credentials = credentials
                };

                client.Send(mail);

               
        }
    }
}
