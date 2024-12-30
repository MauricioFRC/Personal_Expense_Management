using Core.DTOs.CategoryExpense;
using Core.DTOs.Expense;
using Core.Entities;
using Core.Request;
using Mapster;

namespace Infrastructure.Mapping;

public class ExpenseMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateExpenseRequest, Expense>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.ExpenseCategoryId, src => src.ExpenseCategoryId)
            .Map(dest => dest.Amount, src => src.Amount)
            .Map(dest => dest.Date, src => src.Date)
            .Map(dest => dest.Description, src => src.Description);

        config.NewConfig<UpdateExpenseDto, Expense>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.ExpenseCategoryId, src => src.ExpenseCategoryId)
            .Map(dest => dest.Amount, src => src.Amount)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Date, src => src.Date);

        config.NewConfig<Expense, ExpenseResponseDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.UserName, src => src.User.Name)
            .Map(dest => dest.CategoryExpenseId, src => src.ExpenseCategoryId)
            .Map(dest => dest.ExpenseCategoryName, src => src.ExpenseCategory.Name)
            .Map(dest => dest.Amount, src => src.Amount)
            .Map(dest => dest.IsBlocked, src => src.User.IsBlocked)
            .Map(dest => dest.IsDeleted, src => src.User.IsDeleted)
            .Map(dest => dest.Date, src => src.Date.ToShortDateString())
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Date, src => src.Date);

        config.NewConfig<Expense, ExpenseCategoryTotalDto>()
            .Map(dest => dest.UserId, src => src.Id)
            .Map(dest => dest.Amount, src => src.Amount)
            .Map(dest => dest.UserName, src => src.User.Name)
            .Map(dest => dest.Category, src => src.ExpenseCategory.Name);
    }
}
