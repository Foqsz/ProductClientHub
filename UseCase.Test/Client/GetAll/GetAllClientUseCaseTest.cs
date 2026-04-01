using CommonTestUtilities.Entities;
using CommonTestUtilities.Requests;
using ProductClientHub.Application.UseCases.Users.GetAll;

namespace UseCase.Test.Client.GetAll;

public class GetAllClientUseCaseTest
{
    [Fact]
    public async Task GetAllClientUseCaseTest_Sucess()
    {
        (var newUser, var password) = ClientBuilder.Build();

        var request = RequestClientJsonBuilder.Build(newUser.Name, newUser.Email, password);
    }

    private async GetAllClientsUseCase GetUseCase()
    {
        var repository = ClientRepositoryBuilder.Build(); 

        return useCase;
    }
}
