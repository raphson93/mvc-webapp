using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace mvc_webapp.Models
{
    public partial class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Report> Report { get; set; }
        public virtual DbSet<Login> Login { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
                //optionsBuilder.UseSqlServer("Server=rotodb.c6nitrkqqrja.us-east-2.rds.amazonaws.com;Database=cls-db;Trusted_Connection=false;User ID=admin;Password=root12345;");
                //optionsBuilder.UseSqlServer(configuration.GetConnectionString("EntityDatabaseConnection"));
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("EntityDatabaseConnection"),
                    options => options.EnableRetryOnFailure());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Report>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ArrivalTime).HasColumnName("ARRIVAL_TIME");

                entity.Property(e => e.Date)
                    .HasColumnName("DATE")
                    .HasColumnType("date");

                entity.Property(e => e.Description).HasColumnName("DESCRIPTION");

                entity.Property(e => e.FlmSlm)
                    .HasColumnName("FLM_SLM")
                    .HasMaxLength(50);

                entity.Property(e => e.Location).HasColumnName("LOCATION");

                entity.Property(e => e.MachId)
                    .HasColumnName("MACH_ID")
                    .HasMaxLength(50);

                entity.Property(e => e.ProblemCode)
                    .HasColumnName("PROBLEM_CODE")
                    .HasMaxLength(50);

                entity.Property(e => e.RefNo).HasColumnName("REF_NO");

                entity.Property(e => e.SerialNo)
                    .HasColumnName("SERIAL_NO")
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
