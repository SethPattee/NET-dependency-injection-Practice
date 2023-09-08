using NET_dependency_injection_Practice.Lib;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<Worker>();
builder.Services.AddScoped<IMessageWriter, LoggingMessageWriter>();
//builder.Services.AddSingleton<IMessageWriter, ConsoleMessageWriter>();
//builder.Services.TryAddSingleton<IMessageWriter, LoggingMessageWriter>();

using IHost host = builder.Build();

_ = host.Services.GetService<ExampleService>();

await host.RunAsync();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
