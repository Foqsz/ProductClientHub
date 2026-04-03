using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using ProductClientHub.Application.UseCases.Users.GetAll;
using ProductClientHub.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCase.Test.Client.GetAll;

public class GetAllClientUseCaseTest
{
    [Fact]
    public async Task GetAllClientUseCaseTest_Sucess()
    {
        (var newUser, _) = ClientBuilder.Build();

        var useCase = CreateUseCase([newUser]);

        var result = await useCase.Execute();

        result.ShouldNotBeNull();
        result.Clients.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetAllClientUseCaseTest_EmptyList()
    {
        (var newUser, _) = ClientBuilder.Build();
        newUser = null;

        var useCase = CreateUseCase([newUser]);

        var result = await Should.ThrowAsync<NoContentException>(async () =>
        {
            await useCase.Execute();
        });

        result.Message.ShouldBe(ResourceMessagesExceptions.CLIENT_NOCONTENT);
    }

    private static GetAllClientsUseCase CreateUseCase(IList<ProductClientHub.Domain.Entities.Client?> clients)
    {
        var repository = new ClientReadOnlyRepositoryBuilder();

        if(clients is not null)
            repository.GetAll(clients.Where(c => c is not null).Cast<ProductClientHub.Domain.Entities.Client>().ToList());

        return new GetAllClientsUseCase(repository.Build());
    }
}
