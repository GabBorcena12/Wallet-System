using Microsoft.EntityFrameworkCore;
using ExamWalletSystem.Model;
using System.Net.Sockets;
using ExamWalletSystem.Model.Dto;

namespace ExamWalletSystem.DBContext
{
    public class WalletSystemDBContext : DbContext
    {
        public WalletSystemDBContext(DbContextOptions<WalletSystemDBContext> options)
                : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //For unique AccountNumber
            modelBuilder.Entity<User>()
              .HasIndex(x => x.AccountNumber)
              .IsUnique();

            modelBuilder.Entity<User>()
            .Property(u => u.Version)
            .IsRowVersion();

            modelBuilder.Entity<Transaction>()
            .Property(u => u.Version)
            .IsRowVersion();
        }
        public DbSet<User> tblUser { get; set; }
        public DbSet<Transaction> tblTransaction { get; set; }
    }
}
