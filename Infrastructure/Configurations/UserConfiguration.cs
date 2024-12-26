using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.HasKey(x => x.Id);

        entity.HasIndex(x => x.Email)
              .IsUnique();

        entity.Property(x => x.Password)
              .IsRequired();

        entity.HasMany(x => x.ExpenseCategories)
              .WithOne(x => x.User)
              .HasForeignKey(x => x.UserId);

        entity.HasMany(x => x.Expenses)
              .WithOne(x => x.User)
              .HasForeignKey(x => x.UserId);
    }
}
