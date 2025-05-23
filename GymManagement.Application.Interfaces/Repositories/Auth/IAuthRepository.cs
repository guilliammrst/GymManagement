﻿using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.Results;

namespace GymManagement.Application.Interfaces.Repositories.Auth
{
    public interface IAuthRepository
    {
        Task<ModelActionResult<string>> GetUserPasswordByEmailAsync(string? email);
        Task<ModelActionResult<Role>> GetUserRoleByEmailAsync(string? email);
    }
}