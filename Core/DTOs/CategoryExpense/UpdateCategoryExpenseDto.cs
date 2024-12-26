namespace Core.DTOs.CategoryExpense;

public class UpdateCategoryExpenseDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public int UserId { get; set; }
}
