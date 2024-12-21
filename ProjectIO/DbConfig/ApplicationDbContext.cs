
using Microsoft.EntityFrameworkCore;
using ProjectIO.model;
using System.Data.Entity;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSety reprezentujące tabele w bazie danych
    public DbSet<Worker> Workers { get; set; }
    public DbSet<TrainingSession> TrainingSessions { get; set; }

    //...

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Konfiguracja tabeli Worker
        modelBuilder.Entity<Worker>()
            .HasKey(w => w.WorkerId);

}
