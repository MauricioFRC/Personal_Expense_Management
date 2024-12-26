﻿namespace Core.Entities;

public class ExpenseCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public List<Expense> Expenses { get; set; } = [];
}
