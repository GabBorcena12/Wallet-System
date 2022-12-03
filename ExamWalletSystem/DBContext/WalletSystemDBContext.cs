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
        public DbSet<User> tblUser { get; set; }
        public DbSet<Transaction> tblTransaction { get; set; }
    }
}
