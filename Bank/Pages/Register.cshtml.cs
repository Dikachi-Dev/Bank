using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Bank.Models;
using Bank.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bank.Pages
{

    public class Number
    {
        private readonly Random _random = new Random();
        public string InitialNumber()
        {
            var idBuilder = new StringBuilder();


            int itti = 200;
            // contant account number initial  
            idBuilder.Append(itti);

            // 8-Digits between 1000 and 9999  
            idBuilder.Append(RandomNumber(10000000, 99999999));


            return idBuilder.ToString();
        }

        public int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }

    }
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly BankDbContext context;

        public RegisterModel(UserManager<ApplicationUser>userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, BankDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.context = context;

        }

        [BindProperty]
        public Register Model { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(ModelState.IsValid)
            {
                var firstAccount = userManager.Users.Count() == 0;
                var roleaccount = roleManager.Roles.Count() == 0;
                if(roleaccount)
                {
                    IdentityRole identityRole = new IdentityRole { Name = "Admin" };
                    IdentityRole identityRole1 = new IdentityRole { Name = "User" };

                    await roleManager.CreateAsync(identityRole);
                    await roleManager.CreateAsync(identityRole1);
                    
                }
                
               

                var user = new ApplicationUser()
                {
                    
                    UserName = Model.Email,
                    Email = Model.Email,
                    FirstName = Model.FirstName,
                    LastName = Model.LastName,
                    MarriageStatus = Model.MarriageStatus,
                    Gender = Model.Gender,
                    Occupation = Model.Occupation,
                    Address = Model.Address,
                    State = Model.State,
                    Country = Model.Country,
                    PhoneNumber = Model.PhoneNumber,
                    AccountStatus = "Approved",
                    AccountType = Model.AccountType,
                    Created = DateTime.Now,



                };
                //Account Number generator 
                var generator = new Number();
                string account = generator.InitialNumber();

                //// Retrieve the Name of HOST
                //string hostName = Dns.GetHostName();
                //Console.WriteLine("Host Name is(Your Computer Name): {0}", hostName);

                //// Get the Local IP Address
                //string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();

                var profile = new Profile()
                {
                    Balance = 0,
                    
                    AccountNo = account,
                    CardNo = "423"+ account,
                    CVV = "204",
                    CardExpireDate ="12/26",
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email, 
                    Status = "Approved",
                    
                };
               
                
               var result = await userManager.CreateAsync(user, Model.Password);
                if (result.Succeeded)
                {
                    context.Add(profile);
                    await context.SaveChangesAsync();
                    if (firstAccount)
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                    else
                    {
                        await userManager.AddToRoleAsync(user, "User");
                    }


                    try
                    {
                        StreamReader bd = new(@"~\NewBank\Bank\Pages\mailbody.html");
                        var credentials = new NetworkCredential("test@test.com", "Money2018$");
                        var mail1 = new MailMessage();
                        //var mail2 = new MailMessage();
                        var client = new SmtpClient();
                        string Email = user.Email;
                        string Message = "Thank you for Signing Up." + "</br>"  + " Your Account is Under Review, Please Contact Customer Service on customer-support@bankplc.com for more information";
                        string name = user.FirstName;
                        // sed Mail
                        //sender  body
                        string Mailtext1 = bd.ReadToEnd();
                        Mailtext1 = Mailtext1.Replace("{name}", name);
                        Mailtext1 = Mailtext1.Replace("{message}", Message);
                        Mailtext1 = Mailtext1.Replace("{subject}", "Signup Notification from The bank Plc");
                        //mail to sender 
                        mail1 = new MailMessage()
                        {
                            From = new MailAddress("customer-support@bankplc.com", "Your bank Name"),
                            Subject = "Signup Notification from The bank Plc",
                            Body = Mailtext1
                        };
                        mail1.IsBodyHtml = true;
                        mail1.To.Add(new MailAddress(Email));
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
                    }
                    catch (Exception e)
                    {
                        Console.Write(e);
                    }
                    // sed Mail
                    //string Message = "Thank you for Signing Up."  + "\n " +  " Your Account is Under Review, Please Contact Customer Service on customer-support@bankplc.com for more information";
                    //string subject = "Signup Notification from The bank Plc";
                    //string Email = user.Email;
                    //string name = user.FirstName;

                    //StreamReader bd = new (@"C:\Projects\NewBank\Bank\Pages\mailbody.html");

                    //string Mailtext = bd.ReadToEnd();
                    //Mailtext = Mailtext.Replace("{name}", name);
                    //Mailtext = Mailtext.Replace("{message}", Message);
                    //Mailtext = Mailtext.Replace("{subject}", subject);

                    //// Credentials
                    //var credentials = new NetworkCredential("customer-support@bankplc.com", "Money2018$");

                    // Mail message
                    //var mail = new MailMessage()
                    //{
                    //    From = new MailAddress("customer-support@bankplc.com", " Your bank Name"),
                    //    Subject = subject,
                    //    Body = Mailtext
                    //};

                    //mail.IsBodyHtml = true;
                    //mail.To.Add(new MailAddress(Email));

                    // Smtp client
                    //var client = new SmtpClient()
                    //{
                    //    Port = 8889,
                    //    DeliveryMethod = SmtpDeliveryMethod.Network,
                    //    UseDefaultCredentials = false,
                    //    Host = "mail5013.site4now.net",
                    //    EnableSsl = false,
                    //    Credentials = credentials,
                    //    Timeout=100
                    //};

                    //client.Send(mail); // Send the mail

                    //await signInManager.SignInAsync(user, false);
                    return RedirectToPage("Login");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return Page();
        }
    }
}
