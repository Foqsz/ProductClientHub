using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using ProductClientHub.Application.UseCases.Users.Delete;
using Shouldly;

namespace UseCase.Test.Client.Delete;

public class DeleteClientUseCaseTest
{
    [Fact]
    public async Task DeleteClientUseCase_Sucess()
    {
        var (client, _) = ClientBuilder.Build();

        var useCase = CreateUseCase(client);

        var result = useCase.Execute(client.Id);

        await result.ShouldNotBeNull();
    }

    private static DeleteClientUseCase CreateUseCase(ProductClientHub.Domain.Entities.Client? client)
    {
        var deleteWriteOnlyRepository = ClientDeleteAccountOnlyRepositoryBuilder.Build();
        var repositoryReadOnly = new ClientReadOnlyRepositoryBuilder();
        var unitOfWork = UnitOfWorkBuilder.Build();

        if(client is not null)
        {
            repositoryReadOnly.GetById(client);
            deleteWriteOnlyRepository.Delete(client.Id);
        }

        return new DeleteClientUseCase(deleteWriteOnlyRepository, unitOfWork, repositoryReadOnly.Build());
    }
}
