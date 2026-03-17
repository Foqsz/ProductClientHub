using Microsoft.AspNetCore.Mvc;
using ProductClientHub.Application.UseCases.Users.Delete;
using ProductClientHub.Application.UseCases.Users.GetAll;
using ProductClientHub.Application.UseCases.Users.GetById;
using ProductClientHub.Application.UseCases.Users.Register;
using ProductClientHub.Application.UseCases.Users.Update;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientsController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseShortClientJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RequestClientJson request, [FromServices] IRegisterClientUseCase useCase)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpPut]
    [Route("{clientId:guid}")]
    [ProducesResponseType(typeof(ResponseClientUpdatedJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromRoute] Guid clientId, [FromBody] RequestClientJson request, [FromServices] IUpdateClientUseCase useCase)
    {
        var response = await useCase.Execute(clientId, request);

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseAllClientsJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll([FromServices] IGetAllClientsUseCase useCase)
    {
        var response = await useCase.Execute();
        return Ok(response);
    }

    [HttpGet]
    [Route("{clientId:guid}")]
    [ProducesResponseType(typeof(RequestClientJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid clientId, [FromServices] IGetClientByIdUseCase useCase)
    {
        var response = await useCase.Execute(clientId);

        return Ok(response);
    }

    [HttpDelete]
    [Route("${clientId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid clientId, [FromServices] IDeleteClientUseCase useCase)
    {
        await useCase.Execute(clientId);

        return NoContent();
    }
}
