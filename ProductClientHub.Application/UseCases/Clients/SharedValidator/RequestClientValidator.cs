using FluentValidation;
using ProductClientHub.Communication.Requests;

namespace ProductClientHub.Application.UseCases.Clients.SharedValidator;

public class RequestClientValidator : AbstractValidator<RequestClientJson>
{
    public RequestClientValidator()
    {
        RuleFor(client => client.Name)
            .NotEmpty()
            .WithMessage("O nome do cliente é obrigatório.")
            .MaximumLength(100)
            .WithMessage("O nome do cliente deve conter no máximo 100 caracteres.");

        RuleFor(client => client.Email)
            .NotEmpty()
            .WithMessage("O e-mail é obrigatório e não pode ser vazio.")
            .EmailAddress()
            .WithMessage("O e-mail deve ser um endereço de email válido.");
    }
}
