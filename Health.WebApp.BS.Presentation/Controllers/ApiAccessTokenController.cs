using HealthManager.WebApp.BS.Authorization.Interfaces;
using HealthManager.WebApp.BS.Service.Contracts;
using HealthManager.WebApp.BS.Shared.DataTransferObjects.ApiAccessToken;
using HealthManager.WebApp.BS.Shared.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;


namespace HealthManager.WebApp.BS.Presentation.Controllers
{
    [Route("api/api-access-tokens")]
    [ApiController]
    [Authorize]
    [SwaggerTag("Manages access tokens")]
    public class ApiAccessTokenController: ControllerBase
    {
        private readonly IApiAccessTokenService _service;
        private readonly IAccessRightsResolver _auth;


        public ApiAccessTokenController(IApiAccessTokenService apiAccessToken,IAccessRightsResolver accessRightsResolver)
        {
            _service = apiAccessToken;
            _auth = accessRightsResolver;
        }

        [HttpGet]
        [Authorize(Policy = "InternalOrEndUser")]
        [SwaggerOperation(Summary = "Retrieves API access tokens.")]
        public async Task<IActionResult> GetApiAccessTokenPaged([FromQuery] ApiAccessTokenParameters apiAccessTokenRequestParameters)
        {

            var pagedResult = await _service.GetApiAccessTokensAsync(apiAccessTokenRequestParameters, trackChanges: false, User);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.apiAccessTokenDto);
        }


        [HttpPost]
        [Authorize(Policy = "InternalOrEndUser")]
        [SwaggerOperation(Summary = "Create API access token.")]
        public async Task<IActionResult> CreateApiAccessToken([FromBody] ApiAccessTokenForCreationDto apiAccessTokenForCreation)
        {
            if (apiAccessTokenForCreation is null)
            {
                return BadRequest("ApiAccessTokenForCreationDto object is null");
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }


            var createdApiAccessToken = await _service.CreateApiAccessTokenAsync(apiAccessTokenForCreation);

            return CreatedAtRoute("GetApiAccessToken", new
            {
                apiAccessTokenId = createdApiAccessToken.ApiAccessTokenId
            }, createdApiAccessToken);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "InternalUser")]
        [SwaggerOperation(Summary = "Delete API access token by ID.")]
        public async Task<IActionResult> DeleteApiAccessToken(int id)
        {         
            await _service.DeleteApiAccessTokenAsync(id, trackChanges: false);

            return NoContent();
        }


        [HttpGet("{apiAccessTokenId}", Name = "GetApiAccessToken")]
        [Authorize(Policy = "InternalOrEndUser")]
        [SwaggerOperation(Summary = "Retrieves api access token by apiAccessTokenId.")]
        public async Task<IActionResult> GetApiAccessToken(int apiAccessTokenId)
        {
            var result = await _service.GetApiAccessTokenByIdAsync(apiAccessTokenId, trackChanges: false);

            return Ok(result);
        }

    }
}
