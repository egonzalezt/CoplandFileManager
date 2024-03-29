﻿namespace CoplandFileManager.Infrastructure.EntityFrameworkCore.DbContext;

using Domain.User;
using Domain.File;
using Microsoft.EntityFrameworkCore;
using ModelBuilders;

public class CoplandFileManagerDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<File> Files { get; set; }
    public DbSet<UserFilePermission> UserFilePermissions { get; set; }


    public CoplandFileManagerDbContext(DbContextOptions<CoplandFileManagerDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().Configure();
        modelBuilder.Entity<File>().Configure();
        modelBuilder.Entity<UserFilePermission>().Configure();

    }
}
