using Cats.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Cats.Api.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Cat> Cats { get; set; }
}

