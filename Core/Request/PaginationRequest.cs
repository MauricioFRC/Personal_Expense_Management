namespace Core.Request;

public class PaginationRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string DescriptionText { get; set; } = string.Empty;
    public int ExpenseCategoryId { get; set; }
}
