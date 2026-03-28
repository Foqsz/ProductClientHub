using Microsoft.AspNetCore.Mvc;
using ProductClientHub.Application.UseCases.DoLogin;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseTokenJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DoLogin([FromBody] RequestLoginJson request, [FromServices] IDoLoginUseCase useCase)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }
}
