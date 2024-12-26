using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class ExpenseCategoryConfiguration : IEntityTypeConfiguration<ExpenseCategory>
{
    public void Configure(EntityTypeBuilder<ExpenseCategory> entity)
    {
        entity.HasKey(x => x.Id);

        entity.HasOne(x => x.User)
              .WithMany(x => x.ExpenseCategories)
              .HasForeignKey(x => x.UserId);

        entity.HasMany(x => x.Expenses)
              .WithOne(x => x.ExpenseCategory)
              .HasForeignKey(x => x.ExpenseCategoryId);
    }
}
