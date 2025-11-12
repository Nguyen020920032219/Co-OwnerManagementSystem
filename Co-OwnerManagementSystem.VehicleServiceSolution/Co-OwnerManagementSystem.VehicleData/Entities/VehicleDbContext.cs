using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Co_OwnerManagementSystem.VehicleInfrastructure.Entities;

public partial class VehicleDbContext : DbContext
{
    public VehicleDbContext()
    {
    }

    public VehicleDbContext(DbContextOptions<VehicleDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    private string GetConnectionString()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true).Build();
        return configuration["ConnectionStrings:DefaultConnectionString"];
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(GetConnectionString());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.VehicleId).HasName("PK__Vehicle__476B5492863831F6");

            entity.ToTable("Vehicle");

            entity.HasIndex(e => e.CoOwnerGroupId, "IX_Vehicle_Group");

            entity.HasIndex(e => e.LicensePlate, "UQ__Vehicle__026BC15C55B9CDCD").IsUnique();

            entity.HasIndex(e => e.Vin, "UQ__Vehicle__C5DF234CFBD70AB8").IsUnique();

            entity.Property(e => e.VehicleId).ValueGeneratedNever();
            entity.Property(e => e.BatteryCapacity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ChargingType).HasMaxLength(255);
            entity.Property(e => e.Color).HasMaxLength(255);
            entity.Property(e => e.LicensePlate).HasMaxLength(255);
            entity.Property(e => e.Make).HasMaxLength(255);
            entity.Property(e => e.Model).HasMaxLength(255);
            entity.Property(e => e.PurchasePrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Vin)
                .HasMaxLength(255)
                .HasColumnName("VIN");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
