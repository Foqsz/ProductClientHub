using CommonTestUtilities.Entities;
using CommonTestUtilities.loggedClient;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using ProductClientHub.Application.UseCases.Users.Update;
using ProductClientHub.Domain.Extensions;
using ProductClientHub.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCase.Test.Client.Update;

public class UpdateClientUseCaseTest
{
    [Fact]
    public async Task UpdateClient_Sucess()
    {
        (var client, _) = ClientBuilder.Build();

        var clientRequest = RequestShortClientJsonBuilder.Build(client.Name, client.Email);

        var useCase = CreateUseCase(client, emailExistClient: null, clientExist: true);

        var result = await useCase.Execute(clientRequest);

        result.ShouldNotBeNull();
        result.ShouldSatisfyAllConditions(
            () => result.Name.ShouldBe(clientRequest.Name),
            () => result.Email.ShouldBe(clientRequest.Email)
        );
    }

    [Fact]
    public async Task UpdateClient_Error_ClientNotExists()
    {
        (var client, _) = ClientBuilder.Build();

        var clientRequest = RequestShortClientJsonBuilder.Build(client.Name, client.Email);

        var useCase = CreateUseCase(client, emailExistClient: null, clientExist: false);

        var resultException = await Should.ThrowAsync<NotFoundException>(async () => await useCase.Execute(clientRequest));

        resultException.ShouldNotBeNull();
        resultException.ShouldSatisfyAllConditions(() => resultException.Message.ShouldBe(ResourceMessagesExceptions.CLIENT_NOCONTENT));
    }

    [Fact]
    public async Task UpdateClient_Error_EmailExist()
    {
        (var client, _) = ClientBuilder.Build();
        (var anotherClient, _) = ClientBuilder.Build();

        var clientRequest = RequestShortClientJsonBuilder.Build(client.Name, client.Email);

        var useCase = CreateUseCase(client, emailExistClient: anotherClient, clientExist: true);

        var resultException = await Should.ThrowAsync<EmailAlreadyExistsException>(async () => await useCase.Execute(clientRequest));

        resultException.ShouldNotBeNull();
        resultException.ShouldSatisfyAllConditions(() => resultException.Message.ShouldBe(ResourceMessagesExceptions.EMAIL_INVALID));
    }

    private static UpdateClientUseCase CreateUseCase(
        ProductClientHub.Domain.Entities.Client? client,
        ProductClientHub.Domain.Entities.Client? emailExistClient = null,  
        bool clientExist = false)
    {
        var clientWriteOnlyRepository = ClientWriteOnlyRepositoryBuilder.Build();
        var clientReadOnlyRepository = new ClientReadOnlyRepositoryBuilder();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedClient = LoggedClientBuilder.Build(client!);

        if (client is not null && clientExist.IsTrue())
            clientReadOnlyRepository.GetById(client);

        if (emailExistClient is not null)
            clientReadOnlyRepository.EmailAlreadyExists(client, emailExistClient); 

        return new UpdateClientUseCase(clientWriteOnlyRepository, clientReadOnlyRepository.Build(), unitOfWork, loggedClient);
    }
}
