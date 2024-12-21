using Microsoft.EntityFrameworkCore;

namespace ProjectIO.DBModel
{
    public class SportCenterContext : DbContext
    {
        //nasze tabele jakie będą w bazie
        public DbSet<User> Users { get; set; }

        //do SQLServer
        public SportCenterContext(DbContextOptions options) : base(options)
        {

        }

        //musi być zakomentowane jeśli używamy SQLServer
        //do InMemoryDatabase
        //public SportCenterContext()
        //{

        //}
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseInMemoryDatabase("MyDB");
        //    //moglibyścy tutaj uzywać czegoś takiego że baza się tworzy wraz z życiem programu oraz usuwa jak się kończy
        //    //trzeba dodać wtedy ...FrameworkCore.InMemory
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    userID = 1,
                    userName = "admin",
                    userPassword = "admin"
                }
            );
        }


    }
}
