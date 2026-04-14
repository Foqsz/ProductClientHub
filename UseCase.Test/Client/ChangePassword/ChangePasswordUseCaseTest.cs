using CommonTestUtilities.Cryptografhy;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using ProductClientHub.Application.UseCases.Clients.ChangePassword;
using ProductClientHub.Domain.Extensions;
using ProductClientHub.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCase.Test.Client.ChangePassword;

public class ChangePasswordUseCaseTest
{
    [Fact]
    public async Task ChangePasswordUseCase_Success()
    {
        (var client, var password) = ClientBuilder.Build();

        var newPassword = RequestChangePasswordBuilder.Build(currentPassword: password, newPassword: "newPassword123");

        var useCase = CreateUseCase(client, clientNull: false);

        var response = useCase.Execute(client.Id, newPassword);

        await response.ShouldNotBeNull();
        response.IsCompletedSuccessfully.ShouldBeTrue();
        response.IsCompleted.ShouldBeTrue();
    }

    [Fact]
    public async Task ChangePasswordUseCase_ClientNull()
    {
        (var client, var password) = ClientBuilder.Build();

        var newPassword = RequestChangePasswordBuilder.Build(currentPassword: password, newPassword: "newPassword123");

        var useCase = CreateUseCase(client, clientNull: true);

        var exception = await Should.ThrowAsync<NotFoundException>(async () => await useCase.Execute(client.Id, newPassword));

        exception.Message.ShouldBe(ResourceMessagesExceptions.CLIENT_NOCONTENT);
    }

    [Fact]
    public async Task ChangePasswordUseCase_PasswordInvalid()
    {
        (var client, _) = ClientBuilder.Build();

        var newPassword = RequestChangePasswordBuilder.Build(currentPassword: "123123", newPassword: "newpassword123");

        var useCase = CreateUseCase(client, clientNull: false);

        var exception = await Should.ThrowAsync<ChangePasswordException>(async () => await useCase.Execute(client.Id, newPassword));

        exception.Message.ShouldBe(ResourceMessagesExceptions.LOGIN_INVALID);
    }

    private static ChangePasswordUseCase CreateUseCase(ProductClientHub.Domain.Entities.Client? client, bool clientNull)
    {
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var clientWriteOnlyRepository = ClientWriteOnlyRepositoryBuilder.Build();
        var clientReadOnlyRepository = new ClientReadOnlyRepositoryBuilder();
        var unitOfWork = UnitOfWorkBuilder.Build();

        if(client is not null && clientNull.IsFalse())
        {
            clientReadOnlyRepository.GetById(client);
            passwordEncripter.Encrypt(client!.Password);
        } 

        return new ChangePasswordUseCase(passwordEncripter, clientWriteOnlyRepository, unitOfWork, clientReadOnlyRepository.Build());
    }
}
