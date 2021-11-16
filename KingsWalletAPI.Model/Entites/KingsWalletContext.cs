using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KingsWalletAPI.Model.Entites
{
   public class KingsWalletContext : IdentityDbContext<User, Role, string>
    {
        public KingsWalletContext(DbContextOptions<KingsWalletContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
           
            base.OnModelCreating(builder);
        }

        public DbSet<Wallet> Wallets{ get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Bill> Bills { get; set; }       
    }
}
