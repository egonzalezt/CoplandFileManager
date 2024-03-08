namespace CoplandFileManager.Infrastructure.EntityFrameworkCore.DbContext.ModelBuilders;

using CoplandFileManager.Domain.File;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal static class FileModelBuilder
{
    public static void Configure(this EntityTypeBuilder<File> builder)
    {
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(150);
        builder.Property(p => p.Format)
            .IsRequired()
            .HasMaxLength(30);
        builder.Property(p => p.ObjectRoute)
            .IsRequired()
            .HasMaxLength(256);
        builder.HasIndex(p => p.ObjectRoute).IsUnique();
        builder.HasIndex(p => p.Id).IsUnique();
    }
}
