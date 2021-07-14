using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using prs_app_dotnet.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<prs_app_dotnet.Models.User> Users { get; set; }
    public DbSet<prs_app_dotnet.Models.LineItem> LineItems { get; set; }
    public DbSet<prs_app_dotnet.Models.Product> Products { get; set; }
    public DbSet<prs_app_dotnet.Models.Vendor> Vendors { get; set; }
    public DbSet<prs_app_dotnet.Models.Request> Requests { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>(e => { e.HasIndex(p => p.Username).IsUnique(); });

        builder.Entity<Vendor>(e => { e.HasIndex(p => p.Code).IsUnique(); });

        builder.Entity<Product>().HasIndex(p => new { p.VendorId, p.PartNumber }).IsUnique();

        builder.Entity<LineItem>().HasIndex(p => new { p.RequestId, p.ProductId }).IsUnique();
    }

}

    
