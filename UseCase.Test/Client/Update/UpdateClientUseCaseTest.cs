using CommonTestUtilities.Cryptografhy;
using CommonTestUtilities.Repositories;
using ProductClientHub.Application.UseCases.Users.Update;
using ProductClientHub.Domain.Extensions;

namespace UseCase.Test.Client.Update;

public class UpdateClientUseCaseTest
{

    private static UpdateClientUseCase CreateUseCase(ProductClientHub.Domain.Entities.Client? client, bool emailExistsTest)
    {
        var clientWriteOnlyRepository = ClientWriteOnlyRepositoryBuilder.Build();
        var clientReadOnlyRepository = new ClientReadOnlyRepositoryBuilder();
        var unitOfWork = UnitOfWorkBuilder.Build();

        if(client is not null && emailExistsTest.IsTrue())
        {
            clientReadOnlyRepository.EmailAlreadyExists(client);
        }
        return new UpdateClientUseCase(clientWriteOnlyRepository, clientReadOnlyRepository.Build(), unitOfWork);
    }
}
