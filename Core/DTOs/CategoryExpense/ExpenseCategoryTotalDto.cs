namespace Core.DTOs.CategoryExpense;

public class ExpenseCategoryTotalDto
{
    public int UserId { get; set; }
    public string Category { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}
