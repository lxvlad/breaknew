using Microsoft.EntityFrameworkCore;
using BreakFree.DAL.Entities;


namespace BreakFree.DAL
{
    public class BreakFreeContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Habit> Habits { get; set; }

        public DbSet<DailyStatus> DailyStatuses { get; set; }

        public DbSet<Quote> Quotes { get; set; }

        public DbSet<SOSAction> SOSActions { get; set; }

        public DbSet<UserSOSLog> UserSOSLogs { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "breakfree.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
    }
}
