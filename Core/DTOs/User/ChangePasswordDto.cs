namespace Core.DTOs.User;

public class ChangePasswordDto
{
    public string verifyPassword { get; set; } = string.Empty;
    public string newPassword { get; set; } = string.Empty;
}
