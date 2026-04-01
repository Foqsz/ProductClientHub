using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using ProductClientHub.Application.UseCases.Users.GetById;
using ProductClientHub.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCase.Test.Client.GetById;

public class GetByIdUseCaseTest
{
    [Fact]
    public async Task GetByIdUseCaseTest_Sucess()
    {
        var (newClient, _) = ClientBuilder.Build();

        var useCase = CreateUseCase(newClient);

        var result = await useCase.Execute(newClient.Id);

        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetByIdUseCaseTest_NotFound()
    {
        var (newClient, _) = ClientBuilder.Build();
        newClient.Id = Guid.NewGuid();

        var useCase = CreateUseCase(newClient);

        var result = await Should.ThrowAsync<NotFoundException>(async () =>
        {
            await useCase.Execute(newClient.Id);
        });

        result.Message.ShouldBe(ResourceMessagesExceptions.CLIENT_NOCONTENT);
    }

    private static GetClientByIdUseCase CreateUseCase(ProductClientHub.Domain.Entities.Client? client)
    {
        var repository = new ClientReadOnlyRepositoryBuilder().Build();

        if(client is not null)
            repository.GetById(client.Id);

        return new GetClientByIdUseCase(repository);
    }
}
