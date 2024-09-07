using Microsoft.AspNetCore.Mvc;

namespace Expense.Tracker.Api.Controllers;


[Route("api/[Controller]")]
[ApiController]
public class UserAccountRegistrationController : ControllerBase
{   
    [HttpGet("user-registration")]
    public IActionResult Get()
    {
        return Ok(new {
            Message = "Registered"
        });
    }
}