using Core.DTOs.CategoryExpense;
using Core.Interfaces.Repository;
using Core.Interfaces.Service;
using Core.Request;

namespace Infrastructure.Services;

public class CategoryExpenseService : ICategoryExpenseService
{
    private readonly ICategoryExpenseRepository _categoryExpenseRepository;

    public CategoryExpenseService(ICategoryExpenseRepository categoryExpenseRepository)
    {
        _categoryExpenseRepository = categoryExpenseRepository;
    }

    #region Crud
    public Task<CreateCategoryExpenseResponseDto> CreateCategoryExpense(CreateCategoryExpenseRequest createCategoryExpenseRequest, CancellationToken cancellationToken)
    {
        var createdCategory = _categoryExpenseRepository.CreateCategoryExpense(createCategoryExpenseRequest, cancellationToken)
            ?? throw new NullReferenceException("No se pudo crear el usuario.");

        return createdCategory;
    }

    public Task<CreateCategoryExpenseResponseDto> DeleteCategoryExpense(int id, CancellationToken cancellationToken)
    {
        Validate(id);
        var deletedCategory = _categoryExpenseRepository.DeleteCategoryExpense(id, cancellationToken)
            ?? throw new NullReferenceException("No se pudo crear el usuario.");

        return deletedCategory;
    }

    public Task<CreateCategoryExpenseResponseDto> SearchCategoryExpense(int id, CancellationToken cancellationToken)
    {
        Validate(id);
        var searchedCategory = _categoryExpenseRepository.SearchCategoryExpense(id, cancellationToken)
            ?? throw new ArgumentNullException($"No se encontro la Categoria de Gastos con el Id : {id}");

        return searchedCategory;
    }

    public Task<CreateCategoryExpenseResponseDto> UpdateCategoryExpense(int id, UpdateCategoryExpenseDto updateCategoryExpenseDto, CancellationToken cancellationToken)
    {
        Validate(id);
        var updatedCategory = _categoryExpenseRepository.UpdateCategoryExpense(id, updateCategoryExpenseDto, cancellationToken)
            ?? throw new ArgumentNullException($"No se pudo actualiza la Categoria de Gastos con el id {id}");

        return updatedCategory;
    }

    public Task<List<CreateCategoryExpenseResponseDto>> GetAllCategoryExpense(ExpenseCategoryPaginationRequest expenseCategoryPaginationRequest, CancellationToken cancellationToken)
    {
        if (expenseCategoryPaginationRequest.Page <= 0 && expenseCategoryPaginationRequest.Page <= 0)
            throw new ArgumentOutOfRangeException("La pagina y el tamaño de la pagina deben ser mayores a 0");

        var allCategories = _categoryExpenseRepository.GetAllCategoryExpense(expenseCategoryPaginationRequest, cancellationToken)
            ?? throw new ArgumentNullException("No se encontraron categorias de gastos");

        return allCategories;
    }
    #endregion

    #region Private Methods
    private static void Validate(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
    }
    #endregion
}
