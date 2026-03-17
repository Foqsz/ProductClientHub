using Microsoft.Extensions.DependencyInjection;
using ProductClientHub.Application.Services.Mapping;
using ProductClientHub.Application.UseCases.Users.Delete;
using ProductClientHub.Application.UseCases.Users.GetAll;
using ProductClientHub.Application.UseCases.Users.GetById;
using ProductClientHub.Application.UseCases.Users.Register;
using ProductClientHub.Application.UseCases.Users.Update;
using ProductClientHub.Application.UseCases.GetById;
using ProductClientHub.Application.UseCases.Products.GetAll;
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
        services.AddScoped<IGetClientByIdUseCase, GetClientByIdUseCase>();
        services.AddScoped<IDeleteClientUseCase, DeleteClientUseCase>();

        //products
        services.AddScoped<IRegisterProductUseCase, RegisterProductUseCase>();
        services.AddScoped<IGetAllProductsUseCase, GetAllProductsUseCase>();
        services.AddScoped<IGetProductByIdUseCase, GetProductByIdUseCase>();
    }
}
