using Core.DTOs.User;
using Core.Entities;
using Core.Request;
using Mapster;

namespace Infrastructure.Mapping;

public class UserMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateUserRequest, User>()
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.Password, src => BCrypt.Net.BCrypt.HashPassword(src.Password))
            .Map(dest => dest.Created_At, src => DateTime.UtcNow)
            .Map(dest => dest.Updated_At, src => DateTime.UtcNow)
            .Map(dest => dest.IsDeleted, src => false)
            .Map(dest => dest.IsBlocked, src => false);

        config.NewConfig<UpdateUserDto, User>()
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.IsBlocked, src => src.IsBlocked)
            .Map(dest => dest.IsDeleted, src => src.IsDeleted)
            .Map(dest => dest.Updated_At, src => DateTime.UtcNow);

        config.NewConfig<User, CreateUserResponseDto>()
            .Map(dest => dest.UserId, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.IsDeleted, src => src.IsDeleted)
            .Map(dest => dest.IsBlocked, src => src.IsBlocked)
            .Map(dest => dest.Created_At, src => src.Created_At.ToShortDateString())
            .Map(dest => dest.Updated_At, src => src.Updated_At.ToShortDateString());
    }
}
