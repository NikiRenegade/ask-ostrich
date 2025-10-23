using AIAssistantService.Domain.DTO;
using AIAssistantService.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace AIAssistantService.Presentation.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SurveyGeneratorController : ControllerBase
    {
        private readonly ISurveyGeneratorService _surveyGeneratorService;

        public SurveyGeneratorController(ISurveyGeneratorService surveyGeneratorService)
        {
            _surveyGeneratorService = surveyGeneratorService;
        }

        [HttpPost]
        public async Task<ActionResult<GeneratedSurveyDto>> GenerateSurvey([FromBody] GenerateSurveyRequestDto request, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (request is null || string.IsNullOrWhiteSpace(request.Prompt))
                {
                    return BadRequest(new { message = "Prompt is required." });
                }

                var result = await _surveyGeneratorService.GenerateSurveyAsync(request.Prompt, cancellationToken);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while generating the survey", error = ex.Message });
            }
        }

    }
}
