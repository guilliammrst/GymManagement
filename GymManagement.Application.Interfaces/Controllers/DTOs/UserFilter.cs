﻿using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Application.Interfaces.Controllers.DTOs
{
    public class UserFilter
    {
        [FromQuery(Name = "page-number")]
        public int? PageNumber { get; set; }

        [FromQuery(Name = "page-size")]
        public int? PageSize { get; set; }

        [FromQuery(Name = "email")]
        public string? Email { get; set; }
    }
}
