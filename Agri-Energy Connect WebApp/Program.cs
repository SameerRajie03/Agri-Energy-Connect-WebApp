using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Agri_Energy_Connect_WebApp.Data;
using Agri_Energy_Connect_WebApp.Models;
using Agri_Energy_Connect_WebApp.Workers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Agri_Energy_Connect_WebAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Agri_Energy_Connect_WebAppContext") 
    ?? throw new InvalidOperationException("Connection string 'Agri_Energy_Connect_WebAppContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

SeedDatabase(app);

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
    pattern: "{controller=LoginView}/{action=Login}/{id?}");
//pattern: "{controller=Farmers}/{action=Index}/{id?}");

app.Run();

void SeedDatabase(IHost app)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        var logger = services.GetRequiredService<ILogger<Program>>();

        try
        {
            DbData.InitializeCategories(services);
            DbData.InitializeEmployees(services);
            DbData.InitializeFarmers(services);
            DbData.InitializeProducts(services);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred seeding the DB.");
        }
    }
}