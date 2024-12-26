using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> entity)
    {
        entity.HasKey(x => x.Id);

        entity.HasOne(x => x.User)
              .WithMany(x => x.Expenses)
              .HasForeignKey(x => x.UserId);

        entity.HasOne(x => x.ExpenseCategory)
              .WithMany(x => x.Expenses)
              .HasForeignKey(x => x.ExpenseCategoryId);
    }
}
