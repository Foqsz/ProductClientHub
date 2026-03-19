using FluentValidation;
using ProductClientHub.Communication.Requests;

namespace ProductClientHub.Application.UseCases.Users.SharedValidator;

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

        RuleFor(client => client.Password)
            .NotEmpty()
            .WithMessage("A senha é obrigatória e não pode ser vazia.")
            .MinimumLength(6)
            .WithMessage("A senha deve conter no mínimo 6 caracteres.")
            .MaximumLength(100)
            .WithMessage("A senha deve conter no máximo 100 caracteres.");
    }
}
