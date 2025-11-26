using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Core.Misc;
using SurveyManageService.Domain.DTO;
using SurveyManageService.Domain.Interfaces.Services;

namespace SurveyManageService.Presentation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SurveyController : ControllerBase
{
    private readonly ISurveyService _surveyService;

    public SurveyController(ISurveyService surveyService)
    {
        _surveyService = surveyService;
    }

    [HttpGet]
    public async Task<ActionResult<IList<SurveyDto>>> GetAll(CancellationToken cancellationToken = default)
    {
        try
        {
            var surveys = await _surveyService.GetAllAsync(cancellationToken);
            return Ok(surveys);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving surveys", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SurveyDto>> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var survey = await _surveyService.GetByIdAsync(id, cancellationToken);
            if (survey == null)
            {
                return NotFound(new { message = $"Survey with ID {id} not found" });
            }
            return Ok(survey);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the survey", error = ex.Message });
        }
    }
    [HttpGet("existing/{userId}")]
    public async Task<ActionResult<IList<SurveyShortDto>>> GetExistingByUserId(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var surveys = await _surveyService.GetExistingByUserIdAsync(userId, cancellationToken);
            if (surveys.Count == 0)
            {
                return NotFound(new { message = $"Surveys with User ID {userId} not found" });
            }
            return Ok(surveys);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving surveys", error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<SurveyCreatedDto>> Create([FromBody] CreateSurveyDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _surveyService.AddAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the survey", error = ex.Message });
        }
    }

    [HttpPut()]
    public async Task<IActionResult> Update([FromBody] UpdateSurveyDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updated = await _surveyService.UpdateAsync(request, cancellationToken);
            if (!updated)
            {
                return NotFound(new { message = $"Survey with ID {request.Id} not found" });
            }
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the survey", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var deleted = await _surveyService.DeleteAsync(id, cancellationToken);
            if (!deleted)
            {
                return NotFound(new { message = $"Survey with ID {id} not found" });
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the survey", error = ex.Message });
        }
    }
}
