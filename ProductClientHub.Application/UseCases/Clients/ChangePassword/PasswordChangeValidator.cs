using FluentValidation;
using ProductClientHub.Communication.Requests;

namespace ProductClientHub.Application.UseCases.Clients.ChangePassword;

public class PasswordChangeValidator : AbstractValidator<RequestChangePassword>
{
    public PasswordChangeValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty()
            .WithMessage("Senha atual é necessário.");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage("Nova senha é necessária.")
            .MinimumLength(6)
            .WithMessage("A nova senha deve conter no mínimo 6 caracteres.")
            .MaximumLength(100)
            .WithMessage("A nova senha deve conter no máximo 100 caracteres.");
    }
}