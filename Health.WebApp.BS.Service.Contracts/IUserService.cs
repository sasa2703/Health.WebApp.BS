using HealthManager.WebApp.BS.Entities.Models;
using HealthManager.WebApp.BS.Shared.DataTransferObjects.Authentfication;
using HealthManager.WebApp.BS.Shared.DataTransferObjects.User;
using HealthManager.WebApp.BS.Shared.RequestFeatures;

namespace HealthManager.WebApp.BS.Service.Contracts
{
    public interface IUserService
    {
        Task<(IEnumerable<UserDto> users, MetaData metaData)> GetAllUsersAsync(UserParameters userParameters, bool trackChanges);
        Task<UserDto> GetUserAsync(int userId, bool trackChanges);
        Task<UserDto> AddUserAsync(InvitedUserDto user);
        Task<UserDto> GetUserByUsernameAsync(string username, bool trackChanges);
        Task<UserDto> UpdateUserAsync(int userId, UserForUpdateDto userForUpdate, bool trackChanges);
        Task<(UserForUpdateDto userToPatch, User userEntity)> GetUserForPatchAsync(int userId, bool trackChanges);
        Task SaveChangesForPatchAsync(UserForUpdateDto userToPatch, User userEntity);
        Task ChangePassword(string username, ChangePasswordDto model);
        Task DeleteUser(string username);
    }
}
