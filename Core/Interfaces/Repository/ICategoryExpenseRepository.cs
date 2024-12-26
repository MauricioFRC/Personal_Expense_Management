using Core.DTOs.CategoryExpense;
using Core.Request;

namespace Core.Interfaces.Repository;

public interface ICategoryExpenseRepository
{
    public Task<CreateCategoryExpenseResponseDto> CreateCategoryExpense(CreateCategoryExpenseRequest createCategoryExpenseRequest, CancellationToken cancellationToken);
    public Task<CreateCategoryExpenseResponseDto> SearchCategoryExpense(int id, CancellationToken cancellationToken);
    public Task<CreateCategoryExpenseResponseDto> DeleteCategoryExpense(int id, CancellationToken cancellationToken);
    public Task<CreateCategoryExpenseResponseDto> UpdateCategoryExpense(int id, UpdateCategoryExpenseDto updateCategoryExpenseDto, CancellationToken cancellationToken);
}
