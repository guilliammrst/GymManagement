﻿using GymManagement.Shared.Core.Enums;

namespace GymManagement.Application.Interfaces.Services.Auth
{
    public class RegisterDto
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public Gender? Gender { get; set; }
    }
}
