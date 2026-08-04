using Bansi.Examen.AccesoDatos;
using Bansi.Examen.Infrastructure.Gateways;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
