using DataAccessObjects;
using Microsoft.EntityFrameworkCore;
using ProductManagementRazorPages.Hubs;
using Repositories;
using Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddDbContext<MyStoreContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MyStoreDB")));
builder.Services.AddScoped<CategoryDAO>();
builder.Services.AddScoped<ProductDAO>();
builder.Services.AddScoped<AccountDAO>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    DatabaseSeeder.Seed(scope.ServiceProvider.GetRequiredService<MyStoreContext>());
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.MapStaticAssets();
app.MapHub<SignalRServer>("/signalRServer");
app.MapRazorPages().WithStaticAssets();
app.Run();
