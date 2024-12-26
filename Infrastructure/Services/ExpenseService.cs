using System.Text;
using Core.DTOs.Expense;
using Core.Interfaces.Repository;
using Core.Interfaces.Service;
using Core.Request;
using QRCoder;

namespace Infrastructure.Services;

public class ExpenseService : IExpenseService
{
    private readonly IExpenseRepository _expenseRepository;

    public ExpenseService(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }

    public async Task<ExpenseResponseDto> CreateExpense(CreateExpenseRequest createExpenseRequest, CancellationToken cancellationToken)
    {
        var createdExpense = await _expenseRepository.CreateExpense(createExpenseRequest, cancellationToken)
            ?? throw new NullReferenceException("No se pudo crear el gasto");

        return createdExpense;
    }

    public async Task<ExpenseResponseDto> DeleteExpense(int id, CancellationToken cancellationToken)
    {
        ValidateId(id);
        var deletedExpense = await _expenseRepository.DeleteExpense(id, cancellationToken)
            ?? throw new NullReferenceException("No se pudo eliminar el gasto");

        return deletedExpense;
    }

    public async Task<List<ExpenseResponseDto>> GetAllExpense(PaginationRequest paginationRequest, CancellationToken cancellationToken)
    {
        ValidateId(paginationRequest.ExpenseCategoryId);
        if (paginationRequest.PageSize <= 0 && paginationRequest.Page <= 0) 
            throw new ArgumentOutOfRangeException("El tamaño de la pagina y el numero de paginas no pueden ser valores negativos");

        var expenseList = await _expenseRepository.GetAllExpense(paginationRequest, cancellationToken)
            ?? throw new NullReferenceException("No se pudo obtener la lista de los gastos");

        return expenseList;
    }

    public async Task<ExpenseResponseDto> SearchExpense(int id, CancellationToken cancellationToken)
    {
        ValidateId(id);
        var searchedExpense = await _expenseRepository.SearchExpense(id, cancellationToken)
            ?? throw new ArgumentNullException($"No se encontro el gasto con el id: {id}");

        return searchedExpense;
    }

    public async Task<byte[]> GenerateExpenseQr(int id, CancellationToken cancellationToken)
    {
        ValidateId(id);
        var generateExpenseQr = await _expenseRepository.SearchExpense(id, cancellationToken)
            ?? throw new ArgumentNullException($"No se encontro el gasto con el id: {id}");

        var expenseDataFormatted = new StringBuilder();
        expenseDataFormatted.AppendLine($"Nombre: {generateExpenseQr.UserName}");
        expenseDataFormatted.AppendLine($"Monto: {generateExpenseQr.Amount:F2}");
        expenseDataFormatted.AppendLine($"Fecha: {generateExpenseQr.Date}");
        expenseDataFormatted.AppendLine($"Descripción: {generateExpenseQr.Description}");

        var data = expenseDataFormatted.ToString();

        // var expenseData = JsonSerializer.Serialize(expenseDataFormatted);

        using (QRCodeGenerator qrCodeGenerator = new())
        using (QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q))
        using (PngByteQRCode qrCode = new(qrCodeData))
        {
            byte[] qrCodeImage = qrCode.GetGraphic(5);

            return qrCodeImage;
        }
    }

    public async Task<ExpenseResponseDto> UpdateExpense(int id, UpdateExpenseDto updateExpenseDto, CancellationToken cancellationToken)
    {
        ValidateId(id);
        var updatedExpense = await _expenseRepository.UpdateExpense(id, updateExpenseDto, cancellationToken)
            ?? throw new NullReferenceException($"No se pudo actualizar el gasto con el id: {id}");

        return updatedExpense;
    }

    private static void ValidateId(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(id);
    }
}
