using Microsoft.AspNetCore.Mvc;
using ProductClientHub.Application.UseCases.Products.Register;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    [HttpPost]
    [Route("${clientId:guid}")]
    [ProducesResponseType(typeof(ResponseProductJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Register([FromBody] RequestProductJson request, Guid clientId, [FromServices] IRegisterProductUseCase useCase)
    {
        var response = await useCase.Execute(clientId, request);

        return Created(string.Empty, response);
    }
}
