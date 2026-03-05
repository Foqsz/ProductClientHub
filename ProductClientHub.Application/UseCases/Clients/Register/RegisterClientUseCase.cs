using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;
using ProductClientHub.Domain.Extensions;
using ProductClientHub.Exceptions.ExceptionsBase;

namespace ProductClientHub.Application.UseCases.Clients.Register;

public class RegisterClientUseCase : IRegisterClientUseCase
{
    public async Task<ResponseClientJson> Execute(RequestClientJson request)
    {
        var validator = new RegisterClientValidator();

        var validationResult = validator.Validate(request);

        if (validationResult.IsValid.IsFalse())
        {
            var errors = validationResult.Errors.Select(failure => failure.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errors);
        }

        return new ResponseClientJson
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        };
    }
}
