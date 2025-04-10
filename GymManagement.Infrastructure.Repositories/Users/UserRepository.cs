﻿using Dapper;
using GymManagement.Shared.Core.Configurations;
using GymManagement.Shared.Core.Enums;
using GymManagement.Shared.Core.KeyVaultService;
using GymManagement.Infrastructure.Repositories.Converters;
using GymManagement.Infrastructure.Repositories.DbContexts;
using GymManagement.Shared.Core.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;
using GymManagement.Application.Interfaces.Repositories.Users;

namespace GymManagement.Infrastructure.Repositories.Users
{
    public class UserRepository(GymDbContext _context, IKeyVaultService _keyVaultService, IOptions<KeyVaultOptions> options) : IUserRepository
    {
        private readonly KeyVaultOptions _keyVaultSettings = options.Value;

        public async Task<ModelActionResult<UserDetailsDao>> GetUserByIdAsync(int id)
        {
            await using var connection = new NpgsqlConnection(_keyVaultService.GetValue(_keyVaultSettings.GymDb));

            const string query = "SELECT * FROM users WHERE id = @Id";
            var user = await connection.QueryFirstOrDefaultAsync<UserDetailsDao>(query, new { Id = id });
            if (user == null)
                return ModelActionResult<UserDetailsDao>.Fail(GymFaultType.UserNotFound, "User not found.");
            return ModelActionResult<UserDetailsDao>.Ok(user);
        }

        public async Task<ModelActionResult<List<UserDao>>> GetUsersAsync()
        {
            await using var connection = new NpgsqlConnection(_keyVaultService.GetValue(_keyVaultSettings.GymDb));

            const string query = "SELECT * FROM users";
            var users = await connection.QueryAsync<UserDao>(query);
            return ModelActionResult<List<UserDao>>.Ok([.. users]);
        }

        public async Task<ModelActionResult<UserDao>> CreateUserAsync(UserCreateDao userCreate)
        {
            try
            {
                if (_context.Users.Any(u => u.Email == userCreate.Email))
                    return ModelActionResult<UserDao>.Fail(GymFaultType.UserAlreadyExists, "User creation failed: user already exists.");

                var userModel = userCreate.ToModel();

                await _context.Users.AddAsync(userModel);
                var result = await _context.SaveChangesAsync();
                if (result == 0)
                    return ModelActionResult<UserDao>.Fail(GymFaultType.UserCreationFailed, "User creation failed: no rows affected.");

                return ModelActionResult<UserDao>.Ok(userModel.ToDao());
            }
            catch (DbUpdateException ex)
            {
                return ModelActionResult<UserDao>.Fail(GymFaultType.DatabaseUnavailable, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
            catch (Exception ex)
            {
                return ModelActionResult<UserDao>.Fail(GymFaultType.UserCreationFailed, ex.Message);
            }
        }
    }
}
