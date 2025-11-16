using Files.Core;
using Files.Features.Images.Common;
using Shared.Base.Microservice;
using Shared.Cache.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.UseMicroservice();
builder.Services.AddCache(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddImages();
builder.Services.AddCore(builder.Configuration);

var app = builder.Build();
await app.PrepareMinio();
app.PopulateRecurringJobs();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();