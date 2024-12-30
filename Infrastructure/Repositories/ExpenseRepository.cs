using System.Text;
using System.Text.RegularExpressions;
using Core.DTOs.CategoryExpense;
using Core.DTOs.Expense;
using Core.Entities;
using Core.Interfaces.Repository;
using Core.Request;
using Infrastructure.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly ApplicationDbContext _context;

    public ExpenseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ExpenseResponseDto> CreateExpense(CreateExpenseRequest createExpenseRequest, CancellationToken cancellationToken)
    {
        var createExpense = createExpenseRequest.Adapt<Expense>();

        _context.Expenses.Add(createExpense);
        await _context.SaveChangesAsync(cancellationToken);

        return createExpense.Adapt<ExpenseResponseDto>();
    }

    public async Task<ExpenseResponseDto> DeleteExpense(int id, CancellationToken cancellationToken)
    {
        var deletedExpense = await _context.Expenses
            .Include(x => x.User)
            .Include(x => x.ExpenseCategory)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        _context.Expenses.Remove(deletedExpense!);
        await _context.SaveChangesAsync(cancellationToken);

        return deletedExpense.Adapt<ExpenseResponseDto>();
    }

    public async Task<List<ExpenseResponseDto>> GetAllExpense(PaginationRequest paginationRequest, CancellationToken cancellationToken)
    {
        var expenseList = await _context.Expenses
            .Include(x => x.User)
            .Include(x => x.ExpenseCategory)
            .Where(x => x.ExpenseCategoryId == paginationRequest.ExpenseCategoryId)
            .Where(x => x.Description.ToLower().Contains(RemoveAccents(paginationRequest.DescriptionText)))
            .OrderBy(x => x.Amount)
            .ThenBy(x => x.Date)
            .Skip((paginationRequest.Page - 1) * paginationRequest.PageSize)
            .Take(paginationRequest.PageSize)
            .ToListAsync(cancellationToken);

        return expenseList.Adapt<List<ExpenseResponseDto>>();
    }

    public async Task<ExpenseResponseDto> SearchExpense(int id, CancellationToken cancellationToken)
    {
        var searchedExpense = await _context.Expenses
            .Include(x => x.User)
            .Include(x => x.ExpenseCategory)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return searchedExpense.Adapt<ExpenseResponseDto>();
    }

    public async Task<ExpenseResponseDto> UpdateExpense(int id, UpdateExpenseDto updateExpenseDto, CancellationToken cancellationToken)
    {
        var updatedExpense = await _context.Expenses
            .Include(x => x.User)
            .Include(x => x.ExpenseCategory)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        updateExpenseDto.Adapt(updatedExpense);

        _context.Expenses.Update(updatedExpense!);
        await _context.SaveChangesAsync(cancellationToken);

        return updatedExpense.Adapt<ExpenseResponseDto>();
    }

    public async Task<List<ChartResponseDto>> ChartExpenseData(CancellationToken cancellationToken)
    {
        var expenseCategorySummaries = await _context.Expenses
            .Include(e => e.ExpenseCategory)
            //.Where(e => e.Date >= DateTime.Now.AddMonths(-6))
            .GroupBy(e => e.ExpenseCategory.Name)
            .Select(g => new ChartResponseDto
            {
                Category = g.Key,
                TotalAmount = g.Sum(e => e.Amount)
            })
            .ToListAsync(cancellationToken);

        return expenseCategorySummaries;
    }

    public async Task<List<ExpenseCategoryTotalDto>> GetProductRange(int range, CancellationToken cancellationToken)
    {
        var productRange = await _context.Expenses
            .Include(x => x.User)
            .Include(x => x.ExpenseCategory)
            .Select(x => x).Take(range)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

        return productRange.Adapt<List<ExpenseCategoryTotalDto>>();
    }

    private static string RemoveAccents(string text)
    {
        string normalizedText = text.ToLower().Normalize(NormalizationForm.FormD);
        Regex reg = new Regex("[^a-zA-Z0-9 ]");

        string textWithOutAccents = reg.Replace(normalizedText, "");

        return textWithOutAccents;
    }
}
