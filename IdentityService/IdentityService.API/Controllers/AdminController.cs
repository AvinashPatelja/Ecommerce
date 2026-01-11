using IdentityService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public AdminController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost("approve/{userId}")]
    public async Task<IActionResult> ApproveUser(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
            return NotFound();

        user.IsApproved = true;
        await _userRepository.UpdateAsync(user);

        return Ok("User approved successfully");
    }
}
