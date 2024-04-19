using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CountryModel.models;

public partial class CountriesGoldenContext : IdentityDbContext<WorldCitiesUser>
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
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK_Game");

            entity.ToTable("City");

            entity.HasIndex(e => e.Longitude, "IX_Game_Players");

            entity.HasIndex(e => e.Latitude, "IX_Game_Price");

            entity.HasIndex(e => e.CountryId, "IX_Game_PublisherID");

            entity.HasIndex(e => e.Name, "IX_Game_Revenue");

            entity.HasIndex(e => e.Population, "IX_Game_Year");

            entity.Property(e => e.CityId).ValueGeneratedNever();
            entity.Property(e => e.Latitude).HasColumnType("numeric(18, 4)");
            entity.Property(e => e.Longitude).HasColumnType("numeric(18, 4)");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Country).WithMany(p => p.Cities)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Game_Publisher");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("PK_Publisher");

            entity.ToTable("Country");

            entity.HasIndex(e => e.Iso2, "IX_Publisher_PublisherName");

            entity.Property(e => e.Iso2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Iso3)
                .HasMaxLength(900)
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
