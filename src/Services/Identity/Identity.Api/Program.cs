using Identity.Application;
using Identity.Infrastructure;
using Microsoft.FeatureManagement;
using Shared.Events.EventBus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEventBus(builder.Configuration);
builder.Services.AddFeatureManagement();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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