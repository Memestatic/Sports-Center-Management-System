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
            base.OnModelCreating(modelBuilder);

            // Dodanie danych testowych dla FacilityType
            modelBuilder.Entity<FacilityType>().HasData(
                new FacilityType { typeId = 1, typeName = "Sala gimnastyczna" },
                new FacilityType { typeId = 2, typeName = "Boisko piłkarskie" },
                new FacilityType { typeId = 3, typeName = "Basen" }
            );

            // Dodanie danych testowych dla SportsCenter
            modelBuilder.Entity<SportsCenter>().HasData(
                new SportsCenter
                {
                    centerId = 1,
                    centerName = "Centrum Sportowe Główne",
                    centerStreet = "Główna",
                    centerStreetNumber = "10A",
                    centerCity = "Warszawa",
                    centerState = "Mazowieckie",
                    centerZip = "00-123"
                },
                new SportsCenter
                {
                    centerId = 2,
                    centerName = "Sportowy Klub Wschodni",
                    centerStreet = "Wschodnia",
                    centerStreetNumber = "15B",
                    centerCity = "Kraków",
                    centerState = "Małopolskie",
                    centerZip = "31-456"
                }
            );

            // Dodanie danych testowych dla Facility
            modelBuilder.Entity<Facility>().HasData(
                new Facility
                {
                    facilityId = 1,
                    facilityName = "Sala 1",
                    isChangingRoomAvailable = true,
                    isEquipmentAvailable = true,
                    promoStart = new DateTime(2024, 1, 1),
                    promoEnd = new DateTime(2024, 12, 31),
                    promoRate = 10.0,
                    // Odniesienia do kluczy obcych
                    sportsCenter = null, // Relacje FK są ustawiane automatycznie przez EF Core
                    facilityType = null
                },
                new Facility
                {
                    facilityId = 2,
                    facilityName = "Boisko 1",
                    isChangingRoomAvailable = false,
                    isEquipmentAvailable = true,
                    promoStart = new DateTime(2024, 3, 1),
                    promoEnd = new DateTime(2024, 9, 30),
                    promoRate = 15.0,
                    sportsCenter = null,
                    facilityType = null
                }
            );
        }



    }
}
