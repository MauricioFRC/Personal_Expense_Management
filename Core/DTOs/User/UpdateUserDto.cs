namespace Core.DTOs.User;

public class UpdateUserDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;
    public bool IsBlocked { get; set; } = false;
}
