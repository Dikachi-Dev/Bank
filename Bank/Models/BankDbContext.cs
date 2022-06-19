using Bank.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class BankDbContext : IdentityDbContext<ApplicationUser>
    {
        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options)
        {

        }

        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
