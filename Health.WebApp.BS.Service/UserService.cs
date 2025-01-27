using AutoMapper;
using HealthManager.WebApp.BS.Contracts;
using HealthManager.WebApp.BS.Service.Contracts;
using HealthManager.WebApp.BS.Shared.Constants;
using HealthManager.WebApp.BS.Shared.RequestFeatures;
using UserModel = HealthManager.WebApp.BS.Entities.Models.User;
using HealthManager.WebApp.BS.Shared.Exceptions.User;
using HealthManager.WebApp.BS.Shared.Exceptions.Auth0;
using PasswordGenerator;
using HealthManager.WebApp.BS.Shared.DataTransferObjects.User;
using HealthManager.WebApp.BS.Shared.DataTransferObjects.Authentfication;

namespace HealthManager.WebApp.BS.Service
{
    public sealed class UserService : IUserService
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public UserService(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<UserDto> users, MetaData metaData)> GetAllUsersAsync(UserParameters userParameters, bool trackChanges)
        {
            PagedList<Entities.Models.User> usersWithMetaData;

            if(string.IsNullOrWhiteSpace(userParameters.SubscriptionId))
            {
                 usersWithMetaData = await _repository.User.GetAllUsersAsync(userParameters, trackChanges);
            }
            else
            {
                usersWithMetaData = await _repository.User.GetAllUsersForSubscriptionAsync(userParameters, trackChanges);
            }

            var usersDto = _mapper.Map<IEnumerable<UserDto>>(usersWithMetaData);

            return (users: usersDto, metaData: usersWithMetaData.MetaData);
        }

        public async Task<UserDto> GetUserAsync(int userId, bool trackChanges)
        {
            var user = await _repository.User.GetUserAsync(userId, trackChanges);

            if (user is null)
            {
                throw new UserNotFoundException(userId);
            }

            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }

        public async Task<UserDto> GetUserByUsernameAsync(string username, bool trackChanges)
        {
            var user = await _repository.User.GetUserByUsernameAsync(username, trackChanges);

            if (user is null)
            {
                throw new UserNotFoundException(username);
            }

            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task<UserDto> UpdateUserAsync(int userId, UserForUpdateDto userForUpdate, bool trackChanges)
        {
            var userEntity = await _repository.User.GetUserAsync(userId, trackChanges);

            if (userEntity is null)
            {
                throw new UserNotFoundException(userId);
            }

            _mapper.Map(userForUpdate, userEntity);

             await _repository.SaveAsync();

            var userDto = _mapper.Map<UserDto>(userEntity);

            return userDto;

        }

        public async Task<(UserForUpdateDto userToPatch, HealthManager.WebApp.BS.Entities.Models.User userEntity)> GetUserForPatchAsync(int userId, bool trackChanges)
        {
            var userEntity = await _repository.User.GetUserEnableDisableAsync(userId, trackChanges);

            if (userEntity is null)
            {
                throw new UserNotFoundException(userId);
            }

            var userToPatch = _mapper.Map<UserForUpdateDto>(userEntity);

            return (userToPatch, userEntity);
        }

        public async Task SaveChangesForPatchAsync(UserForUpdateDto userToPatch, HealthManager.WebApp.BS.Entities.Models.User userEntity)
        {
            _mapper.Map(userToPatch, userEntity);

            await _repository.SaveAsync();
        }

        public async Task<UserDto> AddUserAsync(InvitedUserDto user)
        {

            var userModel = _mapper.Map<UserModel>(user);
            if(await _repository.User.GetUserByUsernameAsync(user.Username, false) != null)
            {
                throw new UserAlreadyExistsException(user.Username);
            }          

            userModel.Status = await _repository.UserStatus.GetUserStatusByName(UserStatuses.Created);

            IPassword pwdGenerator = new Password().IncludeLowercase().IncludeUppercase().IncludeSpecial().LengthRequired(15);

           // User createdUser = await _auth0Service.CreateAuth0User(user, userModel.IUserCategory, pwdGenerator.Next());

            await _repository.User.AddUserAsync(userModel);

            try
            { 
                await _repository.SaveAsync(); 

            }catch
            {
               // _auth0Service.DeleteAuth0User(userModel.SAuthZeroUserId);
                throw;
            }
          

            return _mapper.Map<UserDto>(await _repository.User.GetUserByUsernameAsync(user.Username, false));
        }

        public async Task DeleteUser(string username)
        {
            await _repository.User.DeleteUserAsync(username);
            UserModel user = await _repository.User.GetUserByUsernameAsync(username, false);
            await _repository.SaveAsync();

            try
            {
               // await _auth0Service.DeleteAuth0User(user.SAuthZeroUserId);
            }
            catch
            {
                await ReactivateDeletedUser(username);
                throw;
            }
        }

        public async Task ReactivateDeletedUser(string username)
        {
            try
            {
                await _repository.User.ReactivateDeleteUserAsync(username);
                await _repository.SaveAsync();

            }
            catch
            {
                //TODO: LOG error here later
            }
        }

        public async Task ChangePassword(string username, ChangePasswordDto model)
        {
            UserModel user = await _repository.User.GetUserByUsernameAsync(username, false);

            if (user is null)
            {
                throw new UserNotFoundException(username);
            }

            if(model.NewPassword != model.NewPasswordRepeat)
            {
                throw new PasswordsDontMatchException();
            }

            //await _auth0Service.ChangeUserPassword(username, model.NewPassword, user.SAuthZeroUserId);

        }

        public async Task<int> GetNumberOfUsersBySubscriptionIdAsync(string subscriptionId, bool trackChanges)
        {
            int usersNumber = await _repository.User.GetUsersNumberBySubscriptionIdAsync(subscriptionId, trackChanges);

            return usersNumber;
        }

    }
}
