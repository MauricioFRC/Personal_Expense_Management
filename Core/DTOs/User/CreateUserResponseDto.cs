namespace Core.DTOs.User;

public class CreateUserResponseDto
{
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    // public string Password { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;
    public bool IsBlocked { get; set; } = false;
    public string Created_At { get; set; } = string.Empty;
    public string Updated_At { get; set; } = string.Empty;
}
