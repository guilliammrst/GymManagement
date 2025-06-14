using GymManagement.Shared.Core.Enums;
using GymManagement.Infrastructure.Repositories.Converters;
using GymManagement.Infrastructure.Repositories.DbContexts;
using GymManagement.Shared.Core.Results;
using Microsoft.EntityFrameworkCore;
using GymManagement.Application.Interfaces.Repositories.Users;
using System.Linq;

namespace GymManagement.Infrastructure.Repositories.Users
{
    public class UserQueryRepository(GymDbContext _context) : IUserQueryRepository
    {
        public async Task<ModelActionResult<UserDetailsDao>> GetUserByIdAsync(int id)
        {
            var userModel = await _context.Users.Include(u => u.Address)
                                                .Include(u => u.Memberships)
                                                .Include(u => u.Attendances)
                                                .FirstOrDefaultAsync(u => u.Id == id);

            if (userModel == null)
                return ModelActionResult<UserDetailsDao>.Fail(GymFaultType.UserNotFound, "Get user failed: user not found.");

            var user = userModel.ToDetailsDao();
            return ModelActionResult<UserDetailsDao>.Ok(user);
        }

        public async Task<ModelActionResult<UserDetailsDao>> GetUserByEmailAsync(string email)
        {
            var userModel = await _context.Users.Include(u => u.Address)
                                                .Include(u => u.Memberships)
                                                .Include(u => u.Attendances)
                                                .FirstOrDefaultAsync(u => u.Email == email);

            if (userModel == null)
                return ModelActionResult<UserDetailsDao>.Fail(GymFaultType.UserNotFound, "Get user failed: user not found.");

            var user = userModel.ToDetailsDao();
            return ModelActionResult<UserDetailsDao>.Ok(user);
        }

        public async Task<ModelActionResult<List<UserDao>>> GetUsersAsync(UserRepositoryFilter filter)
        {
            var pageNumber = filter.PageNumber ?? 1;
            var pageSize = filter.PageSize ?? 20;

            var userModels = await _context.Users.Include(u => u.Address)
                .Where(u => string.IsNullOrWhiteSpace(filter.Email) || u.Email.Contains(filter.Email))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return ModelActionResult<List<UserDao>>.Ok(userModels.ToDao());
        }
    }
}
