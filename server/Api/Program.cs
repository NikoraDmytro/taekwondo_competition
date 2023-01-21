using DAL;
using BLL;
using Api.Helpers;
using BLLAbstractions;
using DALAbstractions;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

string connectionString = builder.Configuration
    .GetConnectionString("MySQLConnection");

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddSingleton<IUnitOfWork>(_ => new UnitOfWork(connectionString));
builder.Services.AddScoped<IClubsService, ClubsService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseDeveloperExceptionPage();

app.UseStaticFiles();
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseCors("CorsPolicy");

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();