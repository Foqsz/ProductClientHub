using Microsoft.Extensions.DependencyInjection;
using ProductClientHub.Application.Services.Mapping;
using ProductClientHub.Application.UseCases.Clients.GetAll;
using ProductClientHub.Application.UseCases.Clients.Register;
using ProductClientHub.Application.UseCases.Clients.Update;
using ProductClientHub.Application.UseCases.Products.Register;

namespace ProductClientHub.Application;

public static class DependencyInjectionExtension
{
    public static void AddAplication(this IServiceCollection services)
    {
        AddUseCases(services);
        AddMapping(services);
    }

    private static void AddMapping(IServiceCollection services)
    {
        MapConfigurations.Configure();
    }

    private static void AddUseCases(IServiceCollection services)
    {
        //clients
        services.AddScoped<IRegisterClientUseCase, RegisterClientUseCase>();
        services.AddScoped<IGetAllClientsUseCase, GetAllClientsUseCase>();
        services.AddScoped<IUpdateClientUseCase, UpdateClientUseCase>();

        //products
        services.AddScoped<IRegisterProductUseCase, RegisterProductUseCase>();
    }
}
