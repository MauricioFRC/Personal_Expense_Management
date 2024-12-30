using Core.DTOs.User;
using Core.Interfaces.Service;
using Core.Request;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class UserController : BaseApiController
{
    private readonly IUserService _userService;
    private readonly IValidator<CreateUserRequest> _createUserRequestValidator;
    private readonly IValidator<UpdateUserDto> _updateUserValidator;
    
    public UserController(
        IUserService userService,
        IValidator<CreateUserRequest> validator,
        IValidator<UpdateUserDto> updateUserValidator
        )
    {
        _userService = userService;
        _createUserRequestValidator = validator;
        _updateUserValidator = updateUserValidator;
    }

    [HttpPost("create-user")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest createUserRequest, CancellationToken cancellationToken)
    {
        var result = await _createUserRequestValidator.ValidateAsync(createUserRequest, cancellationToken);

        if (!result.IsValid) return BadRequest(result.Errors.Select(x => new { x.PropertyName, x.ErrorMessage }));

        return Ok(await _userService.CreateUser(createUserRequest, cancellationToken));
    }

    [HttpPut("update-user-password")]
    public async Task<IActionResult> UpdateUserPassword([FromQuery] string email, [FromBody] ChangePasswordDto changePasswordDto, CancellationToken cancellationToken)
    {
        return Ok(await _userService.ChangeUserPassword(email, changePasswordDto, cancellationToken));
    }

    [HttpPut("update-user")]
    public async Task<IActionResult> UpdateUser([FromQuery] string email, [FromBody] UpdateUserDto updateUserDto, CancellationToken cancellationToken)
    {
        var result = await _updateUserValidator.ValidateAsync(updateUserDto, cancellationToken);

        if (!result.IsValid) return BadRequest(result.Errors.Select(x => new { x.PropertyName, x.ErrorMessage }));

        return Ok(await _userService.UpdateUser(email, updateUserDto, cancellationToken));
    }

    [HttpPost("login")]
    public async Task<IActionResult> GenerateToken([FromBody] AuthenticateRequest authenticateRequest, CancellationToken cancellationToken)
    {
        return Ok(await _userService.CreateLoginToken(authenticateRequest, cancellationToken));
    }

    [HttpGet("get-all-users")]
    public async Task<IActionResult> GetAllUsers([FromQuery] UserPaginationRequest userPaginationRequest, CancellationToken cancellationToken)
    {
        return Ok(await _userService.ListUsers(userPaginationRequest, cancellationToken));
    }
}