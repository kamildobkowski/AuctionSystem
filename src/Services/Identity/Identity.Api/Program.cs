using Identity.Application;
using Identity.Infrastructure;
using Microsoft.FeatureManagement;
using Shared.Base.Microservice;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFeatureManagement();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.UseMicroservice();

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