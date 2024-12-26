namespace Core.Request;

public class UserPaginationRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public bool IsDeleted { get; set; }
    public bool IsBlocked { get; set; }
}
