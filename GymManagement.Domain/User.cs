using GymManagement.Shared.Core.BaseObjects;
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

        private User(int id, DateTime createdAt, DateTime updatedAt, string name, string surname, DateTime birthdate, string password, Role role, string email, string phoneNumber, Gender gender, Country country, string city, string street, string postalCode, string number) : base(id, createdAt, updatedAt)
        {
            Name = name;
            Surname = surname;
            Birthdate = birthdate;
            Password = password;
            Role = role;
            Email = email;
            PhoneNumber = phoneNumber;
            Gender = gender;
            Country = country;
            City = city;
            Street = street;
            PostalCode = postalCode;
            Number = number;
        }

        public string Name { get; private set; }
        public string Surname { get; private set; }
        public DateTime Birthdate { get; private set; }
        public string Password { get; private set; }
        public Role Role { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public Gender Gender { get; private set; }
        public Country Country { get; private set; }
        public string City { get; private set; }
        public string Street { get; private set; }
        public string PostalCode { get; private set; }
        public string Number { get; private set; }

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

        public static ModelActionResult<User> Build(int id, DateTime createdAt, DateTime updatedAt, string name, string surname, DateTime birthdate, string password, Role role, string email, string phoneNumber, Gender gender, Country country, string city, string street, string postalCode, string number)
        {
            return ModelActionResult<User>.Ok(new User(id, createdAt, updatedAt, name, surname, birthdate, password, role, email, phoneNumber, gender, country, city, street, postalCode, number));
        }

        public ModelActionResult Update(string? name, string? surname, DateTime? birthdate, string? password, Role? role, string? email, string? phoneNumber, Gender? gender, Country? country, string? city, string? street, string? postalCode, string? number)
        {

            if (!string.IsNullOrWhiteSpace(name))
                Name = name;

            if (!string.IsNullOrWhiteSpace(surname))
                Surname = surname;

            if (birthdate != null)
                Birthdate = (DateTime)birthdate;

            if (!string.IsNullOrWhiteSpace(password))
            {
                if (!password.IsValidPassword())
                    return ModelActionResult.Fail(GymFaultType.BadParameter, $"User update failed: field Password wrong format. Expected: at least 8 characters, at least 1 uppercase letter, at least 1 lowercase letter, at least 1 number, at least 1 special character in : '{PasswordExt.SPECIAL_CHARS}'.");
                
                var hashedPassword = password.Hash();
                Password = hashedPassword;
            }        

            if (role != null)
            {
                if (!Enum.IsDefined((Role)role))
                    return ModelActionResult.Fail(GymFaultType.BadParameter, "User update failed: field Role does not contain a valid value (expected: 0 - None, 1 - Member, 2 - Coach, 3 - Staff, 4 - Manager).");
                Role = (Role)role;
            }


            if (!string.IsNullOrWhiteSpace(email))
            {
                if (!UserRegexs.ValidEmail().IsMatch(email))
                    return ModelActionResult.Fail(GymFaultType.BadParameter, "User update failed: field Email wrong format (expected: xx@xx.xx).");
                Email = email;
            }      

            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                if (!UserRegexs.ValidPhoneNumber().IsMatch(phoneNumber))
                    return ModelActionResult.Fail(GymFaultType.BadParameter, "User update failed: field PhoneNumber wrong format (expected: 0123456789).");
                PhoneNumber = phoneNumber;
            }

            if (gender != null)
            {
                if (!Enum.IsDefined((Gender)gender))
                    return ModelActionResult.Fail(GymFaultType.BadParameter, "User update failed: field Gender does not contain a valid value (expected: 0 - None, 1 - Male, 2 - Female, 3 - Other).");
                Gender = (Gender)gender;

            }

            return ModelActionResult.Ok;
        }
    }
}
