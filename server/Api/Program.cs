using Api.ActionFilters;
using DAL;
using BLL;
using Api.Helpers;
using Api.Profiles;
using AutoMapper;
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

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile<ClubProfile>();
    mc.AddProfile<SportsmanProfile>();
    mc.AddProfile<CompetitorProfile>();
    mc.AddProfile<CompetitionProfile>();
});

IMapper mapper = mapperConfig.CreateMapper();

builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.AddScoped<IClubsService, ClubsService>();
builder.Services.AddScoped<ISportsmansService, SportsmansService>();
builder.Services.AddScoped<ICompetitorsService, CompetitorsService>();
builder.Services.AddScoped<ICompetitionsService, CompetitionsService>();
builder.Services.AddSingleton<IUnitOfWork>(_ => new UnitOfWork(connectionString));
builder.Services.AddSingleton(mapper);

builder.Services.AddControllers();

var app = builder.Build();

app.UseDeveloperExceptionPage();

app.UseStaticFiles();
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseCors("CorsPolicy");

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();