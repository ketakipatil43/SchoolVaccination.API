using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using UM.Core.DBEntities;

namespace UM.Core
{
    public class UmContext(DbContextOptions<UmContext> options) : DbContext(options)
    {
        public DbSet<CreateAccount> CreateAccount { get; set; }
        public DbSet<Students> Students { get; set; }
        public DbSet<VaccinationDrive> VaccinationDrive { get; set; }
        public DbSet<VaccinationStudentMapper> VaccinationStudentMapper { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("");//dbconnectionstring
                optionsBuilder.EnableSensitiveDataLogging();
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var foreignkey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignkey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            //modelBuilder.Entity<VaccinationStudentMapper>()
            //   .HasOne(vsm => vsm.Students)
            //   .WithMany(s => s.VaccinationStudentMapper)
            //   .HasForeignKey(vsm => vsm.StudentUniqueId)
            //   .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<VaccinationStudentMapper>()
            //    .HasOne(vsm => vsm.VaccinationDrive)
            //    .WithMany(s => s.VaccinationStudentMapper)
            //    .HasForeignKey(vsm => vsm.VaccinationDriveUniuqId)
            //    .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
