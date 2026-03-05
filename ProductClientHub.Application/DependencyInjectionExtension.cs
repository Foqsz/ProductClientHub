using Microsoft.Extensions.DependencyInjection;
using ProductClientHub.Application.UseCases.Clients.Register;

namespace ProductClientHub.Application;

public static class DependencyInjectionExtension
{
    public static void AddAplication(this IServiceCollection services)
    {
        AddUseCases(services);
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterClientUseCase, RegisterClientUseCase>();
    }
}
