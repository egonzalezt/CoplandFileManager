namespace CoplandFileManager.Infrastructure.EntityFrameworkCore.DbContext.ModelBuilders;

using Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal static class UserModelBuilder
{
    public static void Configure(this EntityTypeBuilder<User> builder)
    {
        builder.Property(p => p.IsActive)
            .HasDefaultValue(true);
        builder.HasIndex(p => p.Id).IsUnique();
    }
}
