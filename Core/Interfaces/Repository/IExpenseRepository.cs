using Core.DTOs.CategoryExpense;
using Core.DTOs.Expense;
using Core.Request;

namespace Core.Interfaces.Repository;

public interface IExpenseRepository
{
    public Task<ExpenseResponseDto> CreateExpense(CreateExpenseRequest createExpenseRequest, CancellationToken cancellationToken);
    public Task<ExpenseResponseDto> SearchExpense(int id, CancellationToken cancellationToken);
    public Task<ExpenseResponseDto> DeleteExpense(int id, CancellationToken cancellationToken);
    public Task<List<ExpenseResponseDto>> GetAllExpense(PaginationRequest paginationRequest, CancellationToken cancellationToken);
    public Task<ExpenseResponseDto> UpdateExpense(int id, UpdateExpenseDto updateExpenseDto, CancellationToken cancellationToken);
    public Task<List<ChartResponseDto>> ChartExpenseData(CancellationToken cancellationToken);
    public Task<List<ExpenseCategoryTotalDto>> GetProductRange(int range, CancellationToken cancellationToken);
}
