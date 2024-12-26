using Core.DTOs.User;
using Core.Entities;
using Core.Interfaces.Repository;
using Core.Request;
using Infrastructure.Context;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserByEmail(string email, CancellationToken cancellationToken)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> UpdateUserPassword(string email, string newPassword, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        user!.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);

        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<CreateUserResponseDto> CreateUser(CreateUserRequest createUserRequest, CancellationToken cancellationToken)
    {
        var createdUser = createUserRequest.Adapt<User>();

        _context.Users.Add(createdUser);
        await _context.SaveChangesAsync(cancellationToken);

        return createdUser.Adapt<CreateUserResponseDto>();
    }

    public async Task<CreateUserResponseDto> UpdateUser(string email, UpdateUserDto updateUserDto, CancellationToken cancellationToken)
    {
        var updatedUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

        updateUserDto.Adapt(updatedUser);

        _context.Users.Update(updatedUser!);
        await _context.SaveChangesAsync(cancellationToken);

        return updatedUser.Adapt<CreateUserResponseDto>();
    }

    public async Task<List<CreateUserResponseDto>> GetUsers(UserPaginationRequest userPaginationRequest, CancellationToken cancellationToken)
    {
        var userList = await _context.Users
            .Where(x => x.IsBlocked == userPaginationRequest.IsBlocked)
            .Where(x => x.IsDeleted == userPaginationRequest.IsDeleted)
            .OrderBy(x => x.Id)
            .Skip((userPaginationRequest.Page -1) * userPaginationRequest.PageSize)
            .Take(userPaginationRequest.PageSize)
            .ToListAsync(cancellationToken);

        return userList.Adapt<List<CreateUserResponseDto>>();
    }
}