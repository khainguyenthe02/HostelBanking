using HostelBanking.Repositories.Interfaces;
using HostelBanking.Repositories;
using HostelBanking.Services.Interfaces;
using HostelBanking.Services;
using HostelBanking.SqlServerDbHelper.Interfaces;
using HostelBanking.SqlServerDbHelper;
using Dapper;

var builder = WebApplication.CreateBuilder(args);
// Đọc cấu hình từ appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(option =>
{
	option.AddPolicy("policy", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyHeader().AllowAnyMethod());
});



builder.Services.AddSingleton<IServiceManager, ServiceManager>();
builder.Services.AddSingleton<IRepositoryManager, RepositoryManager>();

DefaultTypeMap.MatchNamesWithUnderscores = true;
var app = builder.Build();
app.UseCors("policy");
// Configure the HTTP request pipeline.


app.UseSwagger();
	app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
