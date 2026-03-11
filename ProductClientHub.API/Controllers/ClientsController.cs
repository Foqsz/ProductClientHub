using Microsoft.AspNetCore.Mvc;
using ProductClientHub.Application.UseCases.Clients.Register;
using ProductClientHub.Application.UseCases.GetAll;
using ProductClientHub.Application.UseCases.Update;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientsController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseClientJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RequestClientJson request, [FromServices] IRegisterClientUseCase useCase)
    {
        var response = await useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpPut]
    [Route("{clientId:guid}")]
    [ProducesResponseType(typeof(ResponseClientUpdatedJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById([FromRoute] Guid id)
    {
        return Ok();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Delete()
    {
        return Ok();
    }
}
