namespace CoplandFileManager.Infrastructure.EntityFrameworkCore.DbContext.ModelBuilders;

using Domain.File;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public static class UserFilePermissionModelBuilder
{
    public static void Configure(this EntityTypeBuilder<UserFilePermission> builder)
    {
        builder.HasKey(uf => new { uf.UserId, uf.FileId });

            builder.HasOne(uf => uf.User)
            .WithMany(u => u.UserPermissions)
        .HasForeignKey(uf => uf.UserId);

            builder.HasOne(uf => uf.File)
            .WithMany(f => f.UserPermissions)
            .HasForeignKey(uf => uf.FileId);
    }
}
