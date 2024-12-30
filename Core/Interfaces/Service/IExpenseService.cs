﻿using Core.DTOs.CategoryExpense;
using Core.DTOs.Expense;
using Core.Request;

namespace Core.Interfaces.Service;

public interface IExpenseService
{
    public Task<ExpenseResponseDto> CreateExpense(CreateExpenseRequest createExpenseRequest, CancellationToken cancellationToken);
    public Task<ExpenseResponseDto> SearchExpense(int id, CancellationToken cancellationToken);
    public Task<byte[]> GenerateExpenseQr(int id, CancellationToken cancellationToken);
    public Task<byte[]> GenerateExpenseChartImg(int userId, CancellationToken cancellationToken);
    public Task<ExpenseResponseDto> DeleteExpense(int id, CancellationToken cancellationToken);
    public Task<ExpenseResponseDto> UpdateExpense(int id, UpdateExpenseDto updateExpenseDto, CancellationToken cancellationToken);
    public Task<List<ExpenseResponseDto>> GetAllExpense(PaginationRequest paginationRequest, CancellationToken cancellationToken);
    // public Task<byte[]> GetPdfReport(int month, int year, CancellationToken cancellationToken);
    public Task<byte[]> GenerateExpensePdfReport(int range, CancellationToken cancellationToken);
}
