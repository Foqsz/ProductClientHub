using CommonTestUtilities.Cryptografhy;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using ProductClientHub.Application.UseCases.Users.Update;
using ProductClientHub.Domain.Extensions;
using ProductClientHub.Exceptions.ExceptionsBase;
using ProductClientHub.Infrastructure.Services;
using Shouldly;

namespace UseCase.Test.Client.Update;

public class UpdateClientUseCaseTest
{
    [Fact]
    public async Task UpdateClient_Sucess()
    {
        (var client, _) = ClientBuilder.Build();

        var clientRequest = RequestShortClientJsonBuilder.Build(client.Name, client.Email);

        var useCase = CreateUseCase(client, emailExistsTest: false, clientExist: true);

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

        var useCase = CreateUseCase(client, emailExistsTest: false, clientExist: false);

        var resultException = await Should.ThrowAsync<NotFoundException>(async () => await useCase.Execute(clientRequest));

        resultException.ShouldNotBeNull();
        resultException.ShouldSatisfyAllConditions(() => resultException.Message.ShouldBe(ResourceMessagesExceptions.CLIENT_NOCONTENT));
    }

    [Fact]
    public async Task UpdateClient_Error_EmailExist()
    {
        (var client, _) = ClientBuilder.Build();

        var clientRequest = RequestShortClientJsonBuilder.Build(client.Name, client.Email);

        var useCase = CreateUseCase(client, emailExistsTest: true, clientExist: true);

        var resultException = await Should.ThrowAsync<EmailAlreadyExistsException>(async () => await useCase.Execute(clientRequest));

        resultException.ShouldNotBeNull();
        resultException.ShouldSatisfyAllConditions(() => resultException.Message.ShouldBe(ResourceMessagesExceptions.EMAIL_INVALID));
    }

    private static UpdateClientUseCase CreateUseCase(ProductClientHub.Domain.Entities.Client? client, bool emailExistsTest, bool clientExist)
    {
        var clientWriteOnlyRepository = ClientWriteOnlyRepositoryBuilder.Build();
        var clientReadOnlyRepository = new ClientReadOnlyRepositoryBuilder();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(client!);

        if (client is not null && clientExist.IsTrue())
            clientReadOnlyRepository.GetById(client); 

        if(emailExistsTest.IsTrue())
            clientReadOnlyRepository.EmailAlreadyExists(client);

        return new UpdateClientUseCase(clientWriteOnlyRepository, clientReadOnlyRepository.Build(), unitOfWork, loggedUser);
    }
}
