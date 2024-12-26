namespace Core.DTOs.Expense;

public class ExpenseResponseDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CategoryExpenseId { get; set; }
    public decimal Amount { get; set; }
    public string Date { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public bool IsBlocked { get; set; }
    public string ExpenseCategoryName { get; set; } = string.Empty;
}
