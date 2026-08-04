using Bansi.Examen.AccesoDatos;
using Bansi.Examen.Infrastructure.Gateways;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

builder.Services.AddHttpClient(nameof(HttpExamenGateway), client =>
{
    client.BaseAddress = new Uri(builder.Configuration["WebServiceBaseUrl"]!);
});

builder.Services.AddScoped<IClsExamenFactory>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("BdiExamen")!;
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    return new ClsExamenFactory(connectionString, httpClientFactory);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Examen");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Examen}/{action=Index}/{id?}");

app.Run();
