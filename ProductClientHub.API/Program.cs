using Microsoft.OpenApi;
using ProductClientHub.API.Filters;
using ProductClientHub.API.Middlewares;
using ProductClientHub.API.Token;
using ProductClientHub.Application;
using ProductClientHub.Domain.Security.Tokens;
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
        Name = "Authorization",
        Description = $"JWT Authorization header using the Bearer scheme.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = AUTHENTICATION_TYPE,
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference(AUTHENTICATION_TYPE, document)] = []
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
builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        policy => policy
            .WithOrigins("http://127.0.0.1:5500")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<LocalizationMiddleware>();

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

//app.MapOpenApi("/doc/{documentName}.json");

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsEnvironment("Testing").Equals(false))
    MigrateDatabase();

app.Run();

void MigrateDatabase()
{
    var connectionString = builder.Configuration.ConnectionString();

    var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

    DataBaseMigration.Migrate(serviceScope.ServiceProvider, connectionString);
}