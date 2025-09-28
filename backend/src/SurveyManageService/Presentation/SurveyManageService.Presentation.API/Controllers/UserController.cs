using Microsoft.AspNetCore.Mvc;
using SurveyManageService.Domain.DTO;
using SurveyManageService.Domain.Interfaces.Services;

namespace SurveyManageService.Presentation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IList<UserDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var users = await _userService.GetAllAsync(cancellationToken);
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving users", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _userService.GetByIdAsync(id, cancellationToken);
            if (user == null)
            {
                return NotFound(new { message = $"User with ID {id} not found" });
            }
            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the user", error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<ActionResult<UserCreatedDto>> CreateAsync([FromBody] CreateUserDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.AddAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = result.Id }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the user", error = ex.Message });
        }
    }

    [HttpPut()]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateUserDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _userService.UpdateAsync(request, cancellationToken);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the user", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            await _userService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the user", error = ex.Message });
        }
    }
}
