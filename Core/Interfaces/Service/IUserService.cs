using Core.DTOs.User;
using Core.Request;

namespace Core.Interfaces.Service;

public interface IUserService
{
    public Task<string> ChangeUserPassword(string email, ChangePasswordDto changePasswordDto, CancellationToken cancellationToken);
    public Task<string> CreateLoginToken(AuthenticateRequest authenticateRequest, CancellationToken cancellationToken);
    public Task<CreateUserResponseDto> CreateUser(CreateUserRequest createUserRequest, CancellationToken cancellationToken);
    public Task<CreateUserResponseDto> UpdateUser(string email, UpdateUserDto updateUserDto, CancellationToken cancellationToken);
    public Task<List<CreateUserResponseDto>> ListUsers(UserPaginationRequest userPaginationRequest, CancellationToken cancellationToken);
}
