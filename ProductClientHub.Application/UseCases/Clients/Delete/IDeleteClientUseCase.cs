namespace ProductClientHub.Application.UseCases.Users.Delete;

public interface IDeleteClientUseCase
{
    Task Execute(Guid clientId);
}
