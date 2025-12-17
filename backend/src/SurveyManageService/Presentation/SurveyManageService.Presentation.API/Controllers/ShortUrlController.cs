using Microsoft.AspNetCore.Mvc;
using SurveyManageService.Domain.DTO;
using SurveyManageService.Domain.Interfaces.Services;

namespace SurveyManageService.Presentation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShortUrlController : ControllerBase
{
    private readonly IShortUrlService _shortUrlService;
    private readonly ISurveyService _surveyService;

    public ShortUrlController(
        IShortUrlService shortUrlService,
        ISurveyService surveyService)
    {
        _shortUrlService = shortUrlService;
        _surveyService = surveyService;
    }

    [HttpGet("id/{id}")]
    public async Task<ActionResult<SurveyDto>> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var survey = await _shortUrlService.GetByIdAsync(id, cancellationToken);
            if (survey == null)
            {
                return NotFound(new { message = $"Short URL with ID {id} not found" });
            }
            return Ok(survey);
        } catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the short URL", error = ex.Message });
        }
    }

    [HttpGet("{shortCode}")]
    public async Task<IActionResult> RedirectToOrigin(string shortCode, CancellationToken cancellationToken = default)
    {
        try
        {
            var url = await _shortUrlService.GetByCodeAsync(shortCode, cancellationToken);

            if (url == null)
                return NotFound(new { error = "Short URL not found" });

            return Redirect(url.OriginUrl);
        } 
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while redirect", error = ex.Message });
        }
    }
}
