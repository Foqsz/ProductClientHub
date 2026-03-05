using Microsoft.AspNetCore.Mvc;
using ProductClientHub.Application.UseCases.Clients.Register;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;
using ProductClientHub.Exceptions.ExceptionsBase;

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
        try
        {
            var response = await useCase.Execute(request);

            return Created(string.Empty, response);
        }
        catch (ProductClientHubException ex)
        {
            var errors = ex.GetErrors();
            return BadRequest(new ResponseErrorMessagesJson(errors));
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorMessagesJson("Erro Desconhecido"));
        }
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Update()
    {
        return Ok();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        return Ok();
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
