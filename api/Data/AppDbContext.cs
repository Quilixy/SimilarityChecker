using System.Drawing;
using Microsoft.EntityFrameworkCore;
using api.Models;

namespace api.Data;

public class AppDbContext : DbContext
{
    public DbSet<Brand>          Brands          { get; set; }
    public DbSet<Class>          Classes         { get; set; }
    public DbSet<Sector>         Sectors         { get; set; }
    public DbSet<SectorKeyword>  SectorKeywords  { get; set; }
    public DbSet<BrandVariation> BrandVariations { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
}