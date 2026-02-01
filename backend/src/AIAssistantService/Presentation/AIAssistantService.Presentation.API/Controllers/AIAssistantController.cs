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
        private readonly IDialogHistoryService _dialogHistoryService;

        public AIAssistantController(ILLMClientService llmClientService, IDialogHistoryService dialogHistoryService)
        {
            _llmClientService = llmClientService;
            _dialogHistoryService = dialogHistoryService;
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

                if (!string.IsNullOrEmpty(request.SurveyId))
                {
                    var userMessage = new DialogMessageDto
                    {
                        Content = request.Prompt,
                        IsUserMessage = true,
                        Timestamp = DateTime.UtcNow
                    };
                    await _dialogHistoryService.SaveMessagesAsync(request.SurveyId, [ userMessage ], cancellationToken);
                }

                var result = await _llmClientService.AskLLMAsync(request.Prompt, request.CurrentSurveyJson, cancellationToken);

                if (!string.IsNullOrEmpty(request.SurveyId))
                {
                    var aiMessage = new DialogMessageDto
                    {
                        Content = result,
                        IsUserMessage = false,
                        Timestamp = DateTime.UtcNow
                    };
                    await _dialogHistoryService.SaveMessagesAsync(request.SurveyId, [ aiMessage ], cancellationToken);
                }

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

        [HttpPost("history/{surveyId}")]
        public async Task<ActionResult> SaveDialogHistory(string surveyId, [FromBody] List<DialogMessageDto> messages, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(surveyId))
                {
                    return BadRequest(new { message = "SurveyId is required." });
                }
                if (messages != null && messages.Count > 0)
                {
                    await _dialogHistoryService.SaveMessagesAsync(surveyId, messages, cancellationToken);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while saving dialog history", error = ex.Message });
            }
        }

        [HttpGet("history/{surveyId}")]
        public async Task<ActionResult<List<DialogMessageDto>>> GetDialogHistory(string surveyId, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(surveyId))
                {
                    return BadRequest(new { message = "SurveyId is required." });
                }

                var history = await _dialogHistoryService.GetDialogHistoryAsync(surveyId, cancellationToken);
                return Ok(history);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving dialog history", error = ex.Message });
            }
        }

        [HttpDelete("history/{surveyId}")]
        public async Task<ActionResult> ClearDialogHistory(string surveyId, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(surveyId))
                {
                    return BadRequest(new { message = "SurveyId is required." });
                }

                await _dialogHistoryService.ClearDialogHistoryAsync(surveyId, cancellationToken);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while clearing dialog history", error = ex.Message });
            }
        }

    }
}

