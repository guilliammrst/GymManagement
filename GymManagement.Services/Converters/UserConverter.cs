﻿using GymManagement.Domain;
using GymManagement.Repositories.Users.Interfaces;
using GymManagement.Services.Users.Interfaces;

namespace GymManagement.Services.Converters
{
    public static class UserConverter
    {
        public static UserCreateDao ToCreateDao(this User user)
        {
            return new UserCreateDao
            {
                Name = user.Name,
                Surname = user.Surname,
                Birthdate = user.Birthdate,
                Password = user.Password,
                Role = user.Role,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender
            };
        }

        public static UserDto ToDto(this UserDao user)
        {
            return new UserDto
            {
                Id = user.Id,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Name = user.Name,
                Surname = user.Surname,
                Birthdate = user.Birthdate,
                Password = user.Password,
                Role = user.Role,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender
            };
        }

        public static List<UserDto> ToDto(this List<UserDao> users)
        {
            return users.Select(ToDto).ToList();
        }
    }
}
