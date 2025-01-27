using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using HealthManager.WebApp.BS.Service.Contracts;
using HealthManager.WebApp.BS.Shared.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using HealthManager.WebApp.BS.Shared.Constants;
using HealthManager.WebApp.BS.Authorization.IdentityTools;
using HealthManager.WebApp.BS.Authorization.Interfaces;
using HealthManager.WebApp.BS.Shared.DataTransferObjects.User;
using HealthManager.WebApp.BS.Shared.DataTransferObjects.Authentfication;
using Swashbuckle.AspNetCore.Annotations;

namespace HealthManager.WebApp.BS.Presentation.Controllers
{
    [Route("api/users")]
    [ApiController]
    [SwaggerTag("Manages users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IAccessRightsResolver _auth;

        public UsersController(IUserService service, IAccessRightsResolver auth)
        {
            _service = service;
            _auth = auth;
        }

        [HttpGet]
        [Authorize(Policy = "InternalUser")]
        [SwaggerOperation(Summary = "Retrieves user by UserParameters.")]
        public async Task<IActionResult> GetUsers([FromQuery] UserParameters userParameters)
        {
            var pagedResult = await _service.GetAllUsersAsync(userParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.users);
        }
         


        [HttpPatch("{username}/change-password")]
        [Authorize(Policy = "InternalOrEndUser")]
        [SwaggerOperation(Summary = "Reset user password.")]
        public async Task<IActionResult> ResetUserPassword(string username, [FromBody] ChangePasswordDto model)
        {
            _auth.CheckPrincipalsUsername(User, username);
            await _service.ChangePassword(username, model);

            return NoContent();
        }


        [HttpPost]
        [Authorize(Policy = "InternalUser")]
        [SwaggerOperation(Summary = "Add User.")]
        public async Task<IActionResult> AddUser([FromBody] InvitedUserDto user)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }
            var createdUser = await _service.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUserByUsername), new {username = createdUser.Username}, createdUser);
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = "InternalOrEndUser")]
        [SwaggerOperation(Summary = "Retrieves user by id.")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _service.GetUserAsync(id, trackChanges: false);

            return Ok(user);
        }

        [HttpGet("{username}")]
        [Authorize(Policy = "InternalOrEndUser")]
        [SwaggerOperation(Summary = "Retrieves user by username.")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var user = await _service.GetUserByUsernameAsync(username, trackChanges: false);

            return Ok(user);
        }


        [HttpGet("me")]
        [Authorize(Policy = "InternalOrEndUser")]
        [SwaggerOperation(Summary = "Retrieves data for logged in user.")]
        public async Task<IActionResult> GetLoggedInUserData()
        {
            var user = await _service.GetUserByUsernameAsync(ClaimsParser.ParseClaim(User, TokenClaims.Username), trackChanges: false);

            return Ok(user);
        }

        [HttpDelete("{username}")]
        [Authorize(Policy = "InternalUser")]
        [SwaggerOperation(Summary = "Delete user by username.")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            await _auth.CheckPrincipalsRightsOnDelete(User, username);
            await _service.DeleteUser(username);

            return NoContent();
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = "InternalOrEndUser")]
        [SwaggerOperation(Summary = "Update user by id.")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserForUpdateDto user)
        {
            if (user is null)
            {
                return BadRequest("UserForUpdateDto object is null");
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState); 
            }

           UserDto userDto =  await _service.UpdateUserAsync(id, user, trackChanges: true);

            return Ok(userDto);
        }

        [HttpPatch("{id:int}")]
        [Authorize(Policy = "InternalUser")]
        [SwaggerOperation(Summary = "Disable user bu id.")]
        public async Task<IActionResult> DisableUser(int id, [FromBody] JsonPatchDocument<UserForUpdateDto> patchDoc)
        {
            if (patchDoc is null)
            {
                return BadRequest("patchDoc object sent from client is null.");
            }

            var result = await _service.GetUserForPatchAsync(id, trackChanges: true);

            patchDoc.ApplyTo(result.userToPatch, ModelState);

            TryValidateModel(result.userToPatch);

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            await _service.SaveChangesForPatchAsync(result.userToPatch, result.userEntity);

            return NoContent();
        }
    }
}
