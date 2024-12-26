namespace Core.Request;

public class CreateExpenseRequest
{
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public int UserId { get; set; }
    public int ExpenseCategoryId { get; set; }
}
