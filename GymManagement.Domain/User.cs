﻿using GymManagement.Shared.Core.BaseObjects;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.PasswordExt;
using GymManagement.Shared.Core.Regexs;
using GymManagement.Shared.Core.Results;

namespace GymManagement.Domain
{
    public class User : BaseObject
    {
        private User (string name, string surname, DateTime birthdate, string password, Role role, string email, string phoneNumber, Gender gender) : base(0, DateTime.UtcNow)
        {
            Name = name;
            Surname = surname;
            Birthdate = birthdate;
            Password = password;
            Role = role;
            Email = email;
            PhoneNumber = phoneNumber;
            Gender = gender;
        }

        public string Name { get; }
        public string Surname { get; }
        public DateTime Birthdate { get; }
        public string Password { get; }
        public Role Role { get; }
        public string Email { get; }
        public string PhoneNumber { get; }
        public Gender Gender { get; }

        public static ModelActionResult<User> Create(string? name, string? surname, DateTime? birthdate, string? password, Role? role, string? email, string? phoneNumber, Gender? gender)
        {
            if (string.IsNullOrWhiteSpace(name))
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, "User creation failed: field Name is required.");

            if (string.IsNullOrWhiteSpace(surname))
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, "User creation failed: field Surname is required.");

            if (birthdate == null)
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, "User creation failed: field Birthdate is required.");

            if (string.IsNullOrWhiteSpace(password))
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, "User creation failed: field Password is required.");

            if (!password.IsValidPassword())
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, $"User creation failed: field Password wrong format. Expected: at least 8 characters, at least 1 uppercase letter, at least 1 lowercase letter, at least 1 number, at least 1 special character in : '{PasswordExt.SPECIAL_CHARS}'.");

            if (role == null)
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, "User creation failed: field Role is required.");

            if (!Enum.IsDefined((Role)role))
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, "User creation failed: field Role does not contain a valid value (expected: 0 - None, 1 - Member, 2 - Coach, 3 - Staff, 4 - Manager).");

            if (string.IsNullOrWhiteSpace(email))
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, "User creation failed: field Email is required.");

            if (!UserRegexs.ValidEmail().IsMatch(email))
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, "User creation failed: field Email wrong format (expected: xx@xx.xx).");

            if (string.IsNullOrWhiteSpace(phoneNumber))
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, "User creation failed: field PhoneNumber is required.");

            if (!UserRegexs.ValidPhoneNumber().IsMatch(phoneNumber))
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, "User creation failed: field PhoneNumber wrong format (expected: 0123456789).");

            if (gender == null)
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, "User creation failed: field Gender is required.");

            if (!Enum.IsDefined((Gender)gender))
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, "User creation failed: field Gender does not contain a valid value (expected: 0 - None, 1 - Male, 2 - Female, 3 - Other).");

            var hashedPassword = password.Hash();

            return ModelActionResult<User>.Ok(new User(name, surname, (DateTime)birthdate, hashedPassword, (Role)role, email, phoneNumber, (Gender)gender));
        }
    }
}
