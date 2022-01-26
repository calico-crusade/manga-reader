using CardboardBox.Http;
using MangaReader.Core.Providers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
	   .AddCardboardHttp()
	   .AddTransient<IMangakakalot, Mangakakalot>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors(t =>
{
	t.AllowAnyHeader()
	 .AllowAnyMethod()
	 .AllowAnyOrigin();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
