using Microsoft.EntityFrameworkCore;

namespace EllaSciFair.Data
{
    public class SignUpContext : DbContext
    {
        public DbSet<SignUp>? SignUps { get; set; }
        public DbSet<TakeANumber>? TakeANumbers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=SignUpDB.db;");
        }
    }
}
