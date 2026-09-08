using Microsoft.EntityFrameworkCore;
using CloudCart.Api.Persistence;
using CloudCart.Api.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure EF Core - prefer PostgreSQL when a connection string is provided; fall back to InMemory for local development.
var defaultConn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (!string.IsNullOrEmpty(defaultConn))
    {
        options.UseNpgsql(defaultConn);
    }
    else
    {
        options.UseInMemoryDatabase("CloudCart");
    }
});

// Register repositories (Repository pattern)
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Register services
builder.Services.AddScoped<CloudCart.Api.Services.Interfaces.IProductService, CloudCart.Api.Services.ProductService>();
builder.Services.AddScoped<CloudCart.Api.Services.Interfaces.IOrderService, CloudCart.Api.Services.OrderService>();
builder.Services.AddScoped<CloudCart.Api.Services.Interfaces.IUserService, CloudCart.Api.Services.UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


// If running with PostgreSQL configuration, ensure the database is created on startup (useful for integration tests).
if (!string.IsNullOrEmpty(defaultConn))
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        // Ensure the database is created. For real production scenarios, prefer migrations instead.
        db.Database.EnsureCreated();
    }
}

app.Run();
