using Microsoft.OpenApi;
using ProductClientHub.API.Filters;
using ProductClientHub.API.Middlewares;
using ProductClientHub.Application;
using ProductClientHub.Infrastructure;
using ProductClientHub.Infrastructure.Extensions;
using ProductClientHub.Infrastructure.Migrations;

const string AUTHENTICATION_TYPE = "Bearer";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(AUTHENTICATION_TYPE, new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme
                      Enter 'Bearer' [space] and then your token in the text input below
                      Example: 'Bearer 123456abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = AUTHENTICATION_TYPE
    });
});

//builder.Services.AddOpenApi("v1", o =>
//{
//    o.AddDocumentTransformer((document, context, CancellationToken) =>
//    {
//        document.Info = new()
//        {
//            Title = "Product Client Hub API",
//            Version = "v1",
//            Description = "API for managing product clients and their interactions.",
//            Contact = new()
//            {
//                Name = "Product Client Hub Team",
//                Email = "teste@gmail.com"
//            }
//        };
//
//        document.Servers =
//        [
//            new()
//            {
//                Url = "https://localhost:5001",
//                Description = "Local Development Server"
//            },
//        ];
//
//        document.ExternalDocs = new()
//        {
//            Description = "API Documentation",
//            Url = new Uri("https://localhost:5001/swagger/index.html")
//        };
//
//        return Task.CompletedTask;
//    });
//});

//Add global exception filter
builder.Services.AddMvc(option => option.Filters.Add(typeof(ExceptionFilter)));

//Inject dependencies
builder.Services.AddAplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Host.AddLogger(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<LocalizationMiddleware>();

app.UseHttpsRedirection();

//app.MapOpenApi("/doc/{documentName}.json");

app.UseAuthorization();

app.MapControllers();

MigrateDatabase();

app.Run();

void MigrateDatabase()
{
    var connectionString = builder.Configuration.ConnectionString();

    var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

    DataBaseMigration.Migrate(serviceScope.ServiceProvider, connectionString);
}