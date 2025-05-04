var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(o => o.AddPolicy("AllowAll", p =>
	p.AllowAnyOrigin()
		.AllowAnyHeader()
		.AllowAnyMethod()
));

builder.Services.AddReverseProxy()
	.LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseCors("AllowAll");
app.MapReverseProxy();
app.Run();