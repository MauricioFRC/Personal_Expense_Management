namespace Core.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;
    public bool IsBlocked { get; set; } = false;
    public DateTime Created_At { get; set; }
    public DateTime Updated_At { get; set; }

    public List<ExpenseCategory> ExpenseCategories { get; set; } = [];
    public List<Expense> Expenses { get; set; } = [];
}
