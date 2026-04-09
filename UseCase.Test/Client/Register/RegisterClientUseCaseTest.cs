using CommonTestUtilities.Cryptografhy;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Messaging;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using ProductClientHub.Application.UseCases.Users.Register;
using ProductClientHub.Domain.Extensions;
using ProductClientHub.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCase.Test.Client.Register;

public class RegisterClientUseCaseTest
{
    [Fact]
    public async Task RegisterClientUseCase_Sucess()
    {
        (var newClient, _) = ClientBuilder.Build();

        var client = RequestClientJsonBuilder.Build(
            newClient.Name,
            newClient.Email,
            newClient.Password
        ); 

        var useCase = CreateUseCase(newClient, emailExistsTest: false);

        var result = await useCase.Execute(client);

        result.ShouldNotBeNull(); 
        result.ShouldSatisfyAllConditions();
    }

    [Fact]
    public async Task RegisterClientUseCase_EmailAlreadyExists_Error()
    {
        (var newClient, _) = ClientBuilder.Build();

        var client = RequestClientJsonBuilder.Build(
            newClient.Name,
            newClient.Email,
            newClient.Password
        );

        var useCase = CreateUseCase(newClient, emailExistsTest: true);

        var resultErrorEmailAlreadyExists = await Should.ThrowAsync<EmailAlreadyExistsException>(async () => await useCase.Execute(client));

        resultErrorEmailAlreadyExists.Message.ShouldBe(ResourceMessagesExceptions.EMAIL_INVALID);
        resultErrorEmailAlreadyExists.GetErrors().ShouldContain(ResourceMessagesExceptions.EMAIL_INVALID);
        resultErrorEmailAlreadyExists.GetHttpStatusCode().ShouldBe(System.Net.HttpStatusCode.BadRequest);
    }

    private static RegisterClientUseCase CreateUseCase(ProductClientHub.Domain.Entities.Client? client, bool emailExistsTest)
    {
        var clientWriteOnlyRepository = ClientWriteOnlyRepositoryBuilder.Build();
        var clientReadOnlyRepository = new ClientReadOnlyRepositoryBuilder();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var passwordEncripterBuilder = PasswordEncripterBuilder.Build();
        var messagePublisher = MessagePublisherBuilder.Build();

        if(client is not null && emailExistsTest.IsTrue())
        {
            clientReadOnlyRepository.GetById(client);
            clientReadOnlyRepository.EmailAlreadyExists(client);
        }

        return new RegisterClientUseCase(
            clientWriteOnlyRepository,
            unitOfWork,
            clientReadOnlyRepository.Build(),
            passwordEncripterBuilder,
            messagePublisher);
    }
}
