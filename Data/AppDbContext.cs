using Microsoft.EntityFrameworkCore;
using DemoDockerAPI.Models;

namespace DemoDockerAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
}