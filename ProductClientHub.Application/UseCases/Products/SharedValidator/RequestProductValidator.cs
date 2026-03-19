using FluentValidation;
using ProductClientHub.Communication.Requests;

namespace ProductClientHub.Application.UseCases.Products.SharedValidator;

public class RequestProductValidator : AbstractValidator<RequestProductJson>
{
    public RequestProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("O Nome do produto é necessário.")
            .MaximumLength(100)
            .WithMessage("O Nome do produto deve conter no máximo 100 caracteres.");

        RuleFor(x => x.Brand)
            .NotEmpty()
            .WithMessage("A Marca do produto é necessária.")
            .MaximumLength(50)
            .WithMessage("A Marca do produto deve conter no máximo 50 caracteres.");

        RuleFor(x => x.Price)
            .NotEmpty()
            .WithMessage("O Preço do produto é necessário.")
            .GreaterThan(0)
            .WithMessage("O Preço do produto deve ser maior que zero.");
    }
}
