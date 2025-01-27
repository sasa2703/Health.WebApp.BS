using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using HealthManager.WebApp.BS.Service.Contracts;
using HealthManager.WebApp.BS.Shared.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using HealthManager.WebApp.BS.Authorization.Interfaces;
using HealthManager.WebApp.BS.Shared.DataTransferObjects.Product;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace HealthManager.WebApp.BS.Presentation.Controllers
{   
    [Authorize]
    [Route("api/trial")]
    [ApiController]
    [SwaggerTag("Manages clinical trial metadata")]
    public class TrialController : ControllerBase
    {
        private readonly ITrialService _service;
        private readonly IAccessRightsResolver _auth;

        public TrialController(ITrialService service, IAccessRightsResolver auth)
        {
            _service = service;
            _auth = auth;
        }

        [HttpGet("public")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Retrieves all public available clinical trials.")]
        public async Task<IActionResult> GetPublicTrials()
        {
            return Ok(await _service.GetPubliclyAvailableTrials(false));
        }

       
        [HttpGet]
        [Authorize(Policy = "InternalOrEndUser")]
        [SwaggerOperation(Summary = "Retrieves all internal or end user clinical trials.")]
        public async Task<IActionResult> GetTrials([FromQuery] TrialParameters TrialParameters)
        {
            var pagedResult = await _service.GetAllTrialsAsync(TrialParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.trial);
        }


        [HttpGet("{id:int}")]
        [Authorize(Policy = "InternalOrEndUser")]
        [SwaggerOperation(Summary = "Retrieves internal or end user clinical trials by TrialId.")]
        public async Task<IActionResult> GetTrial(string trialId)
        {
            var trial = await _service.GetTrialAsync(trialId, false);

            return Ok(trial);
        }


        [HttpPost]
        [Authorize(Policy = "InternalUser")]
        [SwaggerOperation(Summary = "Adds a new clinical trial.")]
        public async Task<IActionResult> AddTrial(IFormFile file)
        {           
           
            var (isSuccess, errorMessage, createdTrial) = await _service.CreateTrialAsync(file);

            if (!isSuccess)
                return BadRequest(errorMessage);
    

            return CreatedAtRoute("GetTrial", new
            {
                id = createdTrial.TrialId
            }, createdTrial);
        }


        [HttpDelete("{id}")]
        [Authorize(Policy = "InternalUser")]
        [SwaggerOperation(Summary = "Delete clinical trial.")]
        public async Task<IActionResult> DeleteTrial(string trialId)
        {
            await _service.DeleteTrialAsync(trialId);

            return NoContent();
        }


        [HttpPut("{trialId}")]
        [Authorize(Policy = "InternalUser")]
        [SwaggerOperation(Summary = "Update clinical trial.")]
        public async Task<IActionResult> UpdateTrial(string trialId, IFormFile file)
        {          
          
            var (isSuccess, errorMessage, editedTrial) = await _service.EditTrialAsync(trialId,file);

            if (!isSuccess)
                return BadRequest(errorMessage);


            return Ok(editedTrial);
        }     

    }
}
