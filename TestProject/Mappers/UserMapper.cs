using Work.ApiModels;
using Work.Database;

namespace Work.Mappers;

public class UserMapper : IMapper<User, UserModelDto>
{
    public User ToModel(UserModelDto dto)
    {
        return new User
        {
            Id = dto.Id,
            Name = dto.Name,
            Birthday = dto.Birthday
        };

    }

    public UserModelDto ToDto(User model)
    {
        return new UserModelDto
        {
            Id = model.Id,
            Name = model.Name,
            Birthday = model.Birthday
        };
    }
}