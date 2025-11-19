using AIAssistantService.Domain.DTO;
using AIAssistantService.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace AIAssistantService.Presentation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AIAssistantController : ControllerBase
    {
        private readonly ILLMClientService _llmClientService;

        public AIAssistantController(ILLMClientService llmClientService)
        {
            _llmClientService = llmClientService;
        }

        [HttpPost("generate")]
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

                var result = await _llmClientService.GenerateSurveyAsync(request.Prompt, request.CurrentSurveyJson, cancellationToken);

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

        [HttpPost("ask")]
        public async Task<ActionResult<string>> AskLLM([FromBody] GenerateSurveyRequestDto request, CancellationToken cancellationToken = default)
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

                var result = await _llmClientService.AskLLMAsync(request.Prompt, request.CurrentSurveyJson, cancellationToken);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while asking the LLM", error = ex.Message });
            }
        }

    }
}

