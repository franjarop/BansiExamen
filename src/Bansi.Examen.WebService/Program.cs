using System.Reflection;
using Bansi.Examen.Application.Interfaces;
using Bansi.Examen.Application.Services;
using Bansi.Examen.Infrastructure.Persistence;
using Bansi.Examen.Infrastructure.Repositories;
using Bansi.Examen.WebService.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
});

builder.Services.AddDbContext<ExamenDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BdiExamen")));

builder.Services.AddScoped<IExamenGateway, EfExamenRepository>();
builder.Services.AddScoped<ExamenService>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
