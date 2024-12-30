using Core.DTOs.CategoryExpense;
using Core.Interfaces.Service;
using Core.Request;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class CategoryExpenseController : BaseApiController
{
    private readonly ICategoryExpenseService _categoryExpenseService;
    private readonly IValidator<CreateCategoryExpenseRequest> _createCategoryExpenseValidator;
    private readonly IValidator<UpdateCategoryExpenseDto> _updateCategoryExpenseValidator;

    public CategoryExpenseController(
        ICategoryExpenseService categoryExpenseService,
        IValidator<CreateCategoryExpenseRequest> createCategoryExpenseValidator,
        IValidator<UpdateCategoryExpenseDto> updateCategoryExpenseValidator
        )
    {
        _categoryExpenseService = categoryExpenseService;
        _createCategoryExpenseValidator = createCategoryExpenseValidator;
        _updateCategoryExpenseValidator = updateCategoryExpenseValidator;
    }

    [HttpPost("create-category-expense")]
    public async Task<IActionResult> CreateCategoryExpense([FromBody] CreateCategoryExpenseRequest createCategoryExpenseRequest, CancellationToken cancellationToken)
    {
        var result = await _createCategoryExpenseValidator.ValidateAsync(createCategoryExpenseRequest, cancellationToken);

        if (!result.IsValid) return BadRequest(result.Errors.Select(x => new { x.PropertyName, x.ErrorMessage }));

        return Ok(await _categoryExpenseService.CreateCategoryExpense(createCategoryExpenseRequest, cancellationToken));
    }

    [HttpGet("get-category-expense/{id}")]
    public async Task<IActionResult> GetCategoryExpenseById(int id, CancellationToken cancellationToken)
    {
        return Ok(await _categoryExpenseService.SearchCategoryExpense(id, cancellationToken));
    }

    [HttpPut("update-category-expense/{id}")]
    public async Task<IActionResult> UpdateCategoryExpense(int id, [FromBody] UpdateCategoryExpenseDto updateCategoryExpenseDto, CancellationToken cancellationToken)
    {
        var result = await _updateCategoryExpenseValidator.ValidateAsync(updateCategoryExpenseDto, cancellationToken);

        if (!result.IsValid) return BadRequest(result.Errors.Select(x => new { x.PropertyName, x.ErrorMessage }));

        return Ok(await _categoryExpenseService.UpdateCategoryExpense(id, updateCategoryExpenseDto, cancellationToken));
    }

    [HttpDelete("delete-category-expense/{id}")]
    public async Task<IActionResult> DelteCategoryExpense(int id, CancellationToken cancellationToken)
    {
        return Ok(await _categoryExpenseService.DeleteCategoryExpense(id, cancellationToken));
    }

    [HttpGet("get-all-expense")]
    public async Task<IActionResult> GetAllCategoryExpense([FromQuery] ExpenseCategoryPaginationRequest expenseCategoryPaginationRequest, CancellationToken cancellationToken)
    {
        return Ok(await _categoryExpenseService.GetAllCategoryExpense(expenseCategoryPaginationRequest, cancellationToken));
    }
}
