namespace Core.Interfaces.Auth;

public interface IJwtProvider
{
    public string GenerateToken(string id, string name);
}
