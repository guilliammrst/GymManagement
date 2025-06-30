using GymManagement.Shared.Core.BaseObjects;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.PasswordExt;
using GymManagement.Shared.Core.Regexs;
using GymManagement.Shared.Core.Results;

namespace GymManagement.Domain
{
    public class User : BaseObject
    {
        private User(string name, string surname, DateTime birthdate, string password, Role role, string email, string phoneNumber, Gender gender, Country country, string city, string street, string postalCode, string number) : base(0, DateTime.UtcNow)
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
            Memberships = [];
        }

        private User(int id, DateTime createdAt, DateTime? updatedAt, string name, string surname, DateTime birthdate, string password, Role role, string email, string phoneNumber, Gender gender, Country country, string city, string street, string postalCode, string number, List<Membership>? memberships) : base(id, createdAt, updatedAt)
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
            Memberships = memberships ?? [];
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
        public List<Membership> Memberships { get; private set; }

        public static ModelActionResult<User> Create(string? name, string? surname, DateTime? birthdate, string? password, Role? role, string? email, string? phoneNumber, Gender? gender, Country? country, string? city, string? street, string? postalCode, string? number)
        {
            if (string.IsNullOrWhiteSpace(name))
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, "User creation failed: field Name is required.");

            if (string.IsNullOrWhiteSpace(surname))
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, "User creation failed: field Surname is required.");

            if (!birthdate.HasValue)
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, "User creation failed: field Birthdate is required.");

            birthdate = birthdate.Value.ToUniversalTime();

            if (birthdate.Value > DateTime.UtcNow)
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, "User creation failed: field Birthdate is in future.");

            if (string.IsNullOrWhiteSpace(password))
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, "User creation failed: field Password is required.");

            if (!password.IsValidPassword())
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, $"User creation failed: field Password wrong format. Expected: at least 8 characters, at least 1 uppercase letter, at least 1 lowercase letter, at least 1 number, at least 1 special character in : '{PasswordExt.SPECIAL_CHARS}'.");

            if (!role.HasValue)
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, "User creation failed: field Role is required.");

            if (!Enum.IsDefined(role.Value))
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, "User creation failed: field Role does not contain a valid value (expected: 0 - None, 1 - Member, 2 - Coach, 3 - Staff, 4 - Manager).");

            if (string.IsNullOrWhiteSpace(email))
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, "User creation failed: field Email is required.");

            if (!UserRegexs.ValidEmail().IsMatch(email))
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, "User creation failed: field Email wrong format (expected: xx@xx.xx).");

            if (string.IsNullOrWhiteSpace(phoneNumber))
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, "User creation failed: field PhoneNumber is required.");

            if (!UserRegexs.ValidPhoneNumber().IsMatch(phoneNumber))
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, "User creation failed: field PhoneNumber wrong format (expected: 0123456789).");

            if (!gender.HasValue)
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, "User creation failed: field Gender is required.");

            if (!Enum.IsDefined(gender.Value))
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, "User creation failed: field Gender does not contain a valid value (expected: 0 - None, 1 - Male, 2 - Female, 3 - Other).");

            if (!country.HasValue)
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, "User creation failed: field Country is required.");

            if (!Enum.IsDefined(country.Value))
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, "User creation failed: field Country does not contain a valid value.");

            if (string.IsNullOrWhiteSpace(city))
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, "User creation failed: field City is required.");

            if (string.IsNullOrWhiteSpace(street))
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, "User creation failed: field Street is required.");

            if (string.IsNullOrWhiteSpace(postalCode))
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, "User creation failed: field PostalCode is required.");

            if (string.IsNullOrWhiteSpace(number))
                return ModelActionResult<User>.Fail(GymFaultType.BadParameter, "User creation failed: field Number is required.");

            var hashedPassword = password.Hash();

            return ModelActionResult<User>.Ok(new User(name, surname, birthdate.Value, hashedPassword, role.Value, email, phoneNumber, gender.Value, country.Value, city, street, postalCode, number));
        }

        public static ModelActionResult<User> Build(int id, DateTime createdAt, DateTime? updatedAt, string name, string surname, DateTime birthdate, string password, Role role, string email, string phoneNumber, Gender gender, Country country, string city, string street, string postalCode, string number, List<Membership>? memberships = null)
        {
            return ModelActionResult<User>.Ok(new User(id, createdAt, updatedAt, name, surname, birthdate, password, role, email, phoneNumber, gender, country, city, street, postalCode, number, memberships));
        }

        public ModelActionResult Update(string? name, string? surname, DateTime? birthdate, string? password, Role? role, string? email, string? phoneNumber, Gender? gender, Country? country, string? city, string? street, string? postalCode, string? number)
        {
            if (!string.IsNullOrWhiteSpace(name))
                Name = name;

            if (!string.IsNullOrWhiteSpace(surname))
                Surname = surname;

            if (birthdate.HasValue)
            {
                birthdate = birthdate.Value.ToUniversalTime();

                if (birthdate > DateTime.UtcNow)
                    return ModelActionResult.Fail(GymFaultType.BadParameter, "User update failed: field Birthdate is in future.");

                Birthdate = birthdate.Value;
            }

            if (!string.IsNullOrWhiteSpace(password))
            {
                if (!password.IsValidPassword())
                    return ModelActionResult.Fail(GymFaultType.BadParameter, $"User update failed: field Password wrong format. Expected: at least 8 characters, at least 1 uppercase letter, at least 1 lowercase letter, at least 1 number, at least 1 special character in : '{PasswordExt.SPECIAL_CHARS}'.");

                var hashedPassword = password.Hash();
                Password = hashedPassword;
            }

            if (role.HasValue)
            {
                if (!Enum.IsDefined(role.Value))
                    return ModelActionResult.Fail(GymFaultType.BadParameter, "User update failed: field Role does not contain a valid value (expected: 0 - None, 1 - Member, 2 - Coach, 3 - Staff, 4 - Manager).");

                Role = role.Value;
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

            if (gender.HasValue)
            {
                if (!Enum.IsDefined(gender.Value))
                    return ModelActionResult.Fail(GymFaultType.BadParameter, "User update failed: field Gender does not contain a valid value (expected: 0 - None, 1 - Male, 2 - Female, 3 - Other).");

                Gender = gender.Value;
            }

            if (country.HasValue)
            {
                if (!Enum.IsDefined(country.Value))
                    return ModelActionResult.Fail(GymFaultType.BadParameter, "User update failed: field Country does not contain a valid value.");

                Country = country.Value;
            }

            if (!string.IsNullOrWhiteSpace(city))
                City = city;

            if (!string.IsNullOrWhiteSpace(street))
                Street = street;

            if (!string.IsNullOrWhiteSpace(postalCode))
                PostalCode = postalCode;

            if (!string.IsNullOrWhiteSpace(number))
                Number = number;

            return ModelActionResult.Ok;
        }

        public ModelActionResult AddNewMembership(Membership membership)
        {
            if (Memberships.Any(m => m.RenewWhenExpiry))
                return ModelActionResult.Fail(GymFaultType.UserAlreadyHaveAnActiveMembership, "User add new membership failed: user already has an active membership with renew when expiry option enabled, user have to disable RenewWhenExpiry before start new membership.");

            if (Memberships.Any(m => !m.IsExpired && m.EndDate > membership.StartDate && m.IsActive))
                return ModelActionResult.Fail(GymFaultType.UserAlreadyHaveAnActiveMembership, "User add new membership failed: user will already have an active membership on the chosen start date.");

            Memberships.Add(membership);
            return ModelActionResult.Ok;
        }

        public ModelActionResult<string> GenerateQrCode(string token)
        {
            if (Memberships.Count == 0)
                return ModelActionResult<string>.Fail(GymFaultType.UserHasNoMembership, "User generate QR code failed: user has no membership.");
            
            var activeMembership = Memberships.FirstOrDefault(m => m.IsActive && !m.IsExpired);
            if (activeMembership == null)
                return ModelActionResult<string>.Fail(GymFaultType.UserHasNoActiveMembership, "User generate QR code failed: user has no active membership.");

            var qrCodeResult = QrCode.Create(Id, token);
            if (!qrCodeResult.Success)
                return ModelActionResult<string>.Fail(qrCodeResult);

            var qrCodeGenerationResult = qrCodeResult.Results.Generate();
            if (!qrCodeGenerationResult.Success)
                return ModelActionResult<string>.Fail(qrCodeGenerationResult);

            var qrCode = qrCodeGenerationResult.Results;

            return ModelActionResult<string>.Ok(qrCode);
        }
    }
}
