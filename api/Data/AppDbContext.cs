using Microsoft.EntityFrameworkCore;
using api.Models;

namespace api.Data;

public class AppDbContext : DbContext
{
    public DbSet<Brand>           Brands           { get; set; }
    public DbSet<Class>           Classes          { get; set; }
    public DbSet<Sector>          Sectors          { get; set; }
    public DbSet<SectorKeyword>   SectorKeywords   { get; set; }
    public DbSet<BrandVariation>  BrandVariations  { get; set; }

    public DbSet<BrandClass>      BrandClasses     { get; set; } 
    public DbSet<BrandSector>     BrandSectors     { get; set; } 

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Class>()
            .Property(c => c.Id)
            .ValueGeneratedNever();
        
        modelBuilder.Entity<BrandClass>()
            .HasKey(bc => new { bc.BrandId, bc.ClassId });

        modelBuilder.Entity<BrandClass>()
            .HasOne(bc => bc.Brand)
            .WithMany(b => b.BrandClasses)
            .HasForeignKey(bc => bc.BrandId);

        modelBuilder.Entity<BrandClass>()
            .HasOne(bc => bc.Class)
            .WithMany(c => c.BrandClasses)
            .HasForeignKey(bc => bc.ClassId);
        
        modelBuilder.Entity<BrandSector>()
            .HasKey(bs => new { bs.BrandId, bs.SectorId });

        modelBuilder.Entity<BrandSector>()
            .HasOne(bs => bs.Brand)
            .WithMany(b => b.BrandSectors)
            .HasForeignKey(bs => bs.BrandId);

        modelBuilder.Entity<BrandSector>()
            .HasOne(bs => bs.Sector)
            .WithMany(s => s.BrandSectors)
            .HasForeignKey(bs => bs.SectorId);
    }
}