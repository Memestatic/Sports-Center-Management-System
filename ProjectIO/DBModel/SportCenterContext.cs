using Microsoft.EntityFrameworkCore;

namespace ProjectIO.model
{
    public class SportCenterContext : DbContext
    {
        //nasze tabele jakie będą w bazie
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<FacilityType> FacilityTypes { get; set; }
        public DbSet<Pass> Passes { get; set; }
        public DbSet<PassType> PassTypes { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<SportsCenter> SportsCenters { get; set; }
        public DbSet<TrainingSession>  TrainingSessions { get; set; }
        public DbSet<User> Users { get; set; }
        //public DbSet<UserLoginCredential>  UserLoginCredentials { get; set; }
        public DbSet<Worker> Workers { get; set; }
        //public DbSet<WorkerLoginCredential> WorkerLoginCredentials { get; set; }
        public DbSet<WorkerFunction> WorkerFunctions { get; set; }
        public DbSet<WorkerTrainingSession> WorkerTrainingSessions { get; set; }

        //do SQLServer
        public SportCenterContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkerTrainingSession>()
            .HasKey(wts => new { wts.workerId, wts.sessionId }); 

            modelBuilder.Entity<WorkerTrainingSession>()
                .HasOne(wts => wts.Worker)
                .WithMany()
                .HasForeignKey(wts => wts.workerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkerTrainingSession>()
                .HasOne(wts => wts.TrainingSession)
                .WithMany()
                .HasForeignKey(wts => wts.sessionId)
                .OnDelete(DeleteBehavior.Cascade);
        }


    }
}
