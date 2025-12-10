using Microsoft.AspNetCore.Mvc;
using SurveyResponseService.Domain.DTOs.SurveyResults;
using SurveyResponseService.Domain.Interfaces.Services;

namespace SurveyResponseService.Presentation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SurveyResultController : ControllerBase
    {
        private readonly ISurveyResultService _surveyResultService;

        public SurveyResultController(ISurveyResultService surveyResultService)
        {
            _surveyResultService = surveyResultService;
        }

        [HttpGet]
        public async Task<ActionResult<IList<SurveyResultDto>>> GetAll(CancellationToken cancellationToken = default)
        {
            try
            {
                var results = await _surveyResultService.GetAllAsync(cancellationToken);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving survey results", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SurveyResultDto>> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _surveyResultService.GetByIdAsync(id, cancellationToken);
                if (result == null)
                {
                    return NotFound(new { message = $"Survey result with ID {id} not found" });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the survey result", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<SurveyResultCreatedDto>> Create([FromBody] CreateSurveyResultDto request, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _surveyResultService.AddAsync(request, cancellationToken);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the survey result", error = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateSurveyResultDto request, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updated = await _surveyResultService.UpdateAsync(request, cancellationToken);
                if (!updated)
                {
                    return NotFound(new { message = $"Survey result with ID {request.Id} not found" });
                }
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the survey result", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var deleted = await _surveyResultService.DeleteAsync(id, cancellationToken);
                if (!deleted)
                {
                    return NotFound(new { message = $"Survey result with ID {id} not found" });
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the survey result", error = ex.Message });
            }
        }

        [HttpGet("user-passed-surveys/{userId}")]
        public async Task<ActionResult<IList<PassedSurveyDto>>> GetPassedSurveysByUserId(Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                var passedSurveys = await _surveyResultService.GetPassedSurveysByUserIdAsync(userId, cancellationToken);
                return Ok(passedSurveys);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving passed surveys", error = ex.Message });
            }
        }

        [HttpGet("survey/{surveyId}/user/{userId}")]
        public async Task<ActionResult<PassedSurveyDto>> GetLatestBySurveyIdAndUserId(Guid surveyId, Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _surveyResultService.GetLatestBySurveyIdAndUserIdAsync(surveyId, userId, cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the survey result", error = ex.Message });
            }
        }
    }
}