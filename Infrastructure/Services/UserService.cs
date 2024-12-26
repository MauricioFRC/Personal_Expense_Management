using Core.DTOs.User;
using Core.Interfaces.Auth;
using Core.Interfaces.Repository;
using Core.Interfaces.Service;
using Core.Request;

namespace Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;

    public UserService(
        IUserRepository userRepository,
        IJwtProvider jwtProvider
        )
    {
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
    }

    public async Task<bool> IsDeletedOrBlocked(string email, CancellationToken cancellationToken)
    {
        ValidateEmail(email);
        var status = await _userRepository.GetUserByEmail(email, cancellationToken)
            ?? throw new InvalidOperationException($"No se encontró el email: {email}");

        return status.IsDeleted || status.IsBlocked;
    }

    public async Task<string> ChangeUserPassword(string email, ChangePasswordDto changePasswordDto, CancellationToken cancellationToken)
    {
        ValidateEmail(email);
        var user = await _userRepository.GetUserByEmail(email, cancellationToken)
            ?? throw new KeyNotFoundException("Usuario no encontrado.");

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(changePasswordDto.verifyPassword, user.Password);

        if (!isPasswordValid)
        {
            throw new UnauthorizedAccessException("La contraseña actual es incorrecta.");
        }

        var updateResult = await _userRepository.UpdateUserPassword(email, changePasswordDto.newPassword, cancellationToken);

        if (updateResult)
        {
            return "Contraseña cambiada con éxito";
        }
        else
        {
            throw new Exception("No se pudo actualizar la contraseña.");
        }
    }

    public async Task<string> CreateLoginToken(AuthenticateRequest authenticateRequest, CancellationToken cancellationToken)
    {
        ValidateEmail(authenticateRequest.Email);
        var user = await _userRepository.GetUserByEmail(authenticateRequest.Email, cancellationToken);

        var userStatus = await IsDeletedOrBlocked(authenticateRequest.Email, cancellationToken);
        var isPasswordValid = BCrypt.Net.BCrypt.Verify(authenticateRequest.Password, user.Password);

        if (!userStatus && isPasswordValid)
        {
            var token = _jwtProvider.GenerateToken(user.Id.ToString(), user.Name);
            return token;
        }
        else
        {
            throw new Exception($"No se pudo crear el token ya que el usuario {user.Name} esta inactivo o bloqueado!");
        }
    }

    public Task<CreateUserResponseDto> CreateUser(CreateUserRequest createUserRequest, CancellationToken cancellationToken)
    {
        var createdUser = _userRepository.CreateUser(createUserRequest, cancellationToken)
            ?? throw new NullReferenceException("No se pudo crear el usuario.");

        return createdUser;
    }

    public Task<CreateUserResponseDto> UpdateUser(string email, UpdateUserDto updateUserDto, CancellationToken cancellationToken)
    {
        ValidateEmail(email);
        var updatedUser = _userRepository.UpdateUser(email, updateUserDto, cancellationToken)
            ?? throw new NullReferenceException("No se encontró el usuario solicitado.");

        return updatedUser;
    }

    private static void ValidateEmail(string email)
    {
        if (string.IsNullOrEmpty(email)) throw new ArgumentNullException($"Verifique que el Email: {email} sea correcto");
    }

    public async Task<List<CreateUserResponseDto>> ListUsers(UserPaginationRequest userPaginationRequest, CancellationToken cancellationToken)
    {
        if (userPaginationRequest.Page <= 0 || userPaginationRequest.PageSize <= 0)
            throw new ArgumentOutOfRangeException($"La página y el tamaño de la página no pueden ser negativas ni cero");

        var userList = await _userRepository.GetUsers(userPaginationRequest, cancellationToken)
            ?? throw new NullReferenceException("No se encontraron usuarios");

        return userList;
    }
}
