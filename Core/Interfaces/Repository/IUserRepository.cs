using Core.DTOs.User;
using Core.Entities;
using Core.Request;

namespace Core.Interfaces.Repository;

public interface IUserRepository
{
    public Task<User> GetUserByEmail(string email, CancellationToken cancellationToken);
    public Task<bool> UpdateUserPassword(string email, string newPassword, CancellationToken cancellationToken);
    public Task<CreateUserResponseDto> CreateUser(CreateUserRequest createUserRequest, CancellationToken cancellationToken);
    public Task<CreateUserResponseDto> UpdateUser(string email, UpdateUserDto updateUserDto, CancellationToken cancellationToken);
    public Task<List<CreateUserResponseDto>> GetUsers(UserPaginationRequest userPaginationRequest, CancellationToken cancellationToken);
}