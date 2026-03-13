namespace ProductClientHub.Application.UseCases.Clients.Delete;

public interface IDeleteClientUseCase
{
    Task Execute(Guid clientId);
}
