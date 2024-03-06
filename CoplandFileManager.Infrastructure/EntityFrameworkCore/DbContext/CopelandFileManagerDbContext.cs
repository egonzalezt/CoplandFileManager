namespace CoplandFileManager.Infrastructure.EntityFrameworkCore.DbContext;

using CoplandFileManager.Domain.User;
using CoplandFileManager.Domain.File;
using Microsoft.EntityFrameworkCore;
using ModelBuilders;

public class CoplandFileManagerDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<File> Files { get; set; }

    public CoplandFileManagerDbContext(DbContextOptions<CoplandFileManagerDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().Configure();
        modelBuilder.Entity<File>().Configure();

    }
}
