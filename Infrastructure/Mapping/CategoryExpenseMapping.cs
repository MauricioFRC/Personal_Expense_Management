using Core.DTOs.CategoryExpense;
using Core.Entities;
using Core.Request;
using Mapster;

namespace Infrastructure.Mapping;

public class CategoryExpenseMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateCategoryExpenseRequest, ExpenseCategory>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description);

        config.NewConfig<UpdateCategoryExpenseDto, ExpenseCategory>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Description, src => src.Description);

        config.NewConfig<ExpenseCategory, CreateCategoryExpenseResponseDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.UserName, src => src.User.Name);
    }
}
