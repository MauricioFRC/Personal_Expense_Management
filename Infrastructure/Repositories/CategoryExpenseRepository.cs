using Core.DTOs.CategoryExpense;
using Core.Entities;
using Core.Interfaces.Repository;
using Core.Request;
using Infrastructure.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CategoryExpenseRepository : ICategoryExpenseRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryExpenseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CreateCategoryExpenseResponseDto> CreateCategoryExpense(CreateCategoryExpenseRequest createCategoryExpenseRequest, CancellationToken cancellationToken)
    {
        var createdUser = createCategoryExpenseRequest.Adapt<ExpenseCategory>();

        _context.ExpenseCategories.Add(createdUser);
        await _context.SaveChangesAsync(cancellationToken);

        return createCategoryExpenseRequest.Adapt<CreateCategoryExpenseResponseDto>();
    }

    public async Task<CreateCategoryExpenseResponseDto> DeleteCategoryExpense(int id, CancellationToken cancellationToken)
    {
        var deletedCategory = await _context.ExpenseCategories
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        _context.ExpenseCategories.Remove(deletedCategory!);
        await _context.SaveChangesAsync(cancellationToken);

        return deletedCategory.Adapt<CreateCategoryExpenseResponseDto>();
    }

    public async Task<CreateCategoryExpenseResponseDto> SearchCategoryExpense(int id, CancellationToken cancellationToken)
    {
        var searchedCategory = await _context.ExpenseCategories
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return searchedCategory.Adapt<CreateCategoryExpenseResponseDto>();
    }

    public async Task<CreateCategoryExpenseResponseDto> UpdateCategoryExpense(int id, UpdateCategoryExpenseDto updateCategoryExpenseDto, CancellationToken cancellationToken)
    {
        var searchedCategory = await _context.ExpenseCategories
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        updateCategoryExpenseDto.Adapt(searchedCategory);
        
        _context.ExpenseCategories.Update(searchedCategory!);
        await _context.SaveChangesAsync(cancellationToken);

        return searchedCategory.Adapt<CreateCategoryExpenseResponseDto>();
    }
}
