using ProductClientHub.API.Filters;
using ProductClientHub.Application;
using ProductClientHub.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

//Add global exception filter
builder.Services.AddMvc(option => option.Filters.Add(typeof(ExceptionFilter)));

//Inject dependencies
builder.Services.AddAplication();
builder.Services.AddInfrastructure(builder.Configuration);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
