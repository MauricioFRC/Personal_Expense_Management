using Core.DTOs.Expense;
using Core.Interfaces.Service;
using Core.Request;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class ExpenseController : BaseApiController
{
    private readonly IExpenseService _expenseService;
    private readonly IValidator<CreateExpenseRequest> _createExpenseValidator;
    private readonly IValidator<UpdateExpenseDto> _updateExpenseValidator;

    public ExpenseController(
        IExpenseService expenseService, 
        IValidator<CreateExpenseRequest> createExpenseValidator, 
        IValidator<UpdateExpenseDto> updateExpenseValidator
        )
    {
        _expenseService = expenseService;
        _createExpenseValidator = createExpenseValidator;
        _updateExpenseValidator = updateExpenseValidator;
    }

    [HttpPost("create-expense")]
    public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseRequest createExpenseRequest, CancellationToken cancellationToken)
    {
        var result = await _createExpenseValidator.ValidateAsync(createExpenseRequest, cancellationToken);
        if (!result.IsValid) return BadRequest(result.Errors.Select(x => new { x.PropertyName, x.ErrorMessage }));

        return Ok(await _expenseService.CreateExpense(createExpenseRequest, cancellationToken));
    }

    [HttpGet("get-expense/{id}")]
    public async Task<IActionResult> GetExpense(int id, CancellationToken cancellationToken)
    {
        return Ok(await _expenseService.SearchExpense(id, cancellationToken));
    }

    [HttpGet("generate-expense-qr/{id}")]
    public async Task<IActionResult> GenerateExpenseQr(int id, CancellationToken cancellationToken)
    {
        return File(await _expenseService.GenerateExpenseQr(id, cancellationToken), "image/png");
    }

    [HttpGet("get-all-expenses")]
    public async Task<IActionResult> GetAllExpense(PaginationRequest paginationRequest, CancellationToken cancellationToken)
    {
        return Ok(await _expenseService.GetAllExpense(paginationRequest, cancellationToken));
    }

    [HttpPut("update-expense/{id}")]
    public async Task<IActionResult> UpdateExpense(int id, [FromBody] UpdateExpenseDto updateExpenseDto, CancellationToken cancellationToken)
    {
        var result = await _updateExpenseValidator.ValidateAsync(updateExpenseDto, cancellationToken);
        if (!result.IsValid) return BadRequest(result.Errors.Select(x => new { x.ErrorMessage, x.PropertyName }));

        return Ok(await _expenseService.UpdateExpense(id, updateExpenseDto, cancellationToken));
    }

    [HttpDelete("delete-expense/{id}")]
    public async Task<IActionResult> DeleteExpense(int id, CancellationToken cancellationToken)
    {
        return Ok(await _expenseService.DeleteExpense(id, cancellationToken));
    }

    public IEnumerable<string> TiposGraficos = ["Pay", "Pay2", "Pay3"];

    [HttpGet("generate-chart-report")]
    public async Task<IActionResult> GeneratePdfChartReport(CancellationToken cancellationToken, [FromRoute] string fileName = "grafico_reporte")
    {
        var fileChartPdf = await _expenseService.GenerateExpenseChart(cancellationToken);
        return File(fileChartPdf, "application/pdf", fileName + "_generated_at=" + DateTime.UtcNow.ToShortDateString());
    }

    [HttpGet("get-pdf-expense-report/{range}")]
    public async Task<IActionResult> GetPdfReport(int range, CancellationToken cancellationToken, [FromQuery] string fileName = "reporte_de_gastos")
    {
        var file = await _expenseService.GenerateExpensePdfReport(range, cancellationToken);
        return File(file, "application/pdf", fileName + $"_cant={range}" + "_fecha=" + DateTime.UtcNow.ToShortDateString());
    }

    [HttpGet("get-excel-expense-report/{range}")]
    public async Task<IActionResult> GetExcelReport(int range, CancellationToken cancellationToken, [FromQuery] string fileName = "reporte_de_gastos")
    {
        var file = await _expenseService.GenerateExpenseExcelReport(range, cancellationToken);
        return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName + $"_cant={range}" + "_fecha=" + DateTime.UtcNow.ToShortDateString());
    }
}
