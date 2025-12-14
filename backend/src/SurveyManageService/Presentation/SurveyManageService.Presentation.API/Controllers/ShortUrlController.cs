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

    [HttpPost]
    public async Task<ActionResult<ShortUrlCreatedDto>> CreateShortUrl([FromBody] CreateShortUrlDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var survey = await _surveyService.GetByIdAsync(request.SurveyId, cancellationToken);
            if (survey == null)
            {
                return BadRequest(ModelState);
            }

            var shortUrl = await _shortUrlService.AddAsync(request, cancellationToken);

            var surveyUpdateDto = new UpdateSurveyDto()
            {
                Id = survey.Id,
                Description = survey.Description,
                Title = survey.Title,
                AuthorGuid = survey.Author.Id,
                IsPublished = survey.IsPublished,
                ShortUrlId = shortUrl.Id,
                Questions = survey.Questions
            };

            var updateSurveyResult = await _surveyService.UpdateAsync(surveyUpdateDto, cancellationToken);
            if (!updateSurveyResult)
            {
                await _shortUrlService.DeleteAsync(shortUrl.Id, cancellationToken);
                throw new Exception("Update survey short url error!");
            }

            return CreatedAtAction(nameof(GetById), new { id = shortUrl.Id }, shortUrl);
        } 
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        } 
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the short url", error = ex.Message });
        }
    }
}
