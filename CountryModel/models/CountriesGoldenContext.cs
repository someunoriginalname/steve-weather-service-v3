using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using CountryModel;
using System;
using CountryModel.models;

namespace CountryModel.models;

public partial class CountriesGoldenContext : DbContext
{
    public CountriesGoldenContext()
    {
    }

    public CountriesGoldenContext(DbContextOptions<CountriesGoldenContext> options)
        : base(options)
    {
    }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }
        IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
        var config = builder.Build();
        optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__Table__F2D21A965FAEB50E");

            entity.ToTable("City");

            entity.HasIndex(e => e.CountryId, "IX_City_CountryID");

            entity.HasIndex(e => e.Latitude, "IX_City_Latitude");

            entity.HasIndex(e => e.Longitude, "IX_City_Longitude");

            entity.HasIndex(e => e.Name, "IX_City_Name");

            entity.HasIndex(e => e.Population, "IX_City_Population");

            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.Latitude).HasColumnType("numeric(18, 4)");
            entity.Property(e => e.Longitude).HasColumnType("numeric(18, 4)");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Country).WithMany(p => p.Cities)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_City_Country");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("Country");

            entity.HasIndex(e => e.Iso2, "IX_Country_Iso2");

            entity.HasIndex(e => e.Iso3, "IX_Country_Iso3");

            entity.HasIndex(e => e.Name, "IX_Country_Name");

            entity.Property(e => e.CountryId)
                .HasColumnName("CountryID");
            entity.Property(e => e.Iso2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Iso3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Name)
                .HasMaxLength(400)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
