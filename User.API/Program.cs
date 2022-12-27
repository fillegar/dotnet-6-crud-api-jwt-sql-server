using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using User.API.Handler;
using User.API.Middlewares;
using User.Repository;
using User.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<UserInfoDbContext>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("DB_UserApplication")), ServiceLifetime.Transient);
}
else
{
    builder.Services.AddDbContext<UserInfoDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("DB_UserApplication")), ServiceLifetime.Transient);
}


builder.Services.AddTransient<UserInfoService>();
builder.Services.AddTransient<UserInfoRepository>();
builder.Services.AddTransient<TokenUtilsHandler>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthorization();


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();
