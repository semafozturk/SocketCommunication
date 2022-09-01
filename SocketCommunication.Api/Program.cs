using Microsoft.EntityFrameworkCore;
using SocketCommunication.Api;
using SocketCommunication.Api.Infrastructure;
using SocketCommunication.Api.Infrastructure.Implementations;
using System.Reflection;
using Microsoft.Extensions.Logging.Log4Net.AspNetCore;
using log4net.Config;
using Salaros.Configuration;

var builder = WebApplication.CreateBuilder(args);
XmlConfigurator.Configure(new FileInfo("log4net.config"));
// Add services to the container.
string appConf = @"C:\Users\sema.ozturk\Desktop\Sockettt\SocketCommunication_Async_withGetInfo_ServerBasewLog_ReadFromIniFileinUserServ_GetById_12\SocketCommunication\Variables.ini";


builder.Services.AddControllers();
builder.Logging.AddLog4Net();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddDbContext<UserDbContext>(x =>
{
    var cfg = new ConfigParser(appConf);
    string conStr = cfg.GetValue("ReadDetails", "connectionString");
    x.UseSqlServer(conStr);
});
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
