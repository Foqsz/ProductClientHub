using Microsoft.Extensions.DependencyInjection;
using ProductClientHub.Application.Services.Mapping;
using ProductClientHub.Application.UseCases.Clients.ChangePassword;
using ProductClientHub.Application.UseCases.GetById;
using ProductClientHub.Application.UseCases.Products.Delete;
using ProductClientHub.Application.UseCases.Products.GetAll;
using ProductClientHub.Application.UseCases.Products.Register;
using ProductClientHub.Application.UseCases.Users.Delete;
using ProductClientHub.Application.UseCases.Users.GetAll;
using ProductClientHub.Application.UseCases.Users.GetById;
using ProductClientHub.Application.UseCases.Users.Register;
using ProductClientHub.Application.UseCases.Users.Update;

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
        services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();

        //products
        services.AddScoped<IRegisterProductUseCase, RegisterProductUseCase>();
        services.AddScoped<IGetAllProductsUseCase, GetAllProductsUseCase>();
        services.AddScoped<IGetProductByIdUseCase, GetProductByIdUseCase>();
        services.AddScoped<IDeleteProductUseCase, DeleteProductUseCase>();
    }
}
