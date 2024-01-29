var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("StudentAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetSection("ApiUrls:Student").Value);
});

builder.Services.AddHttpClient("MenuAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetSection("ApiUrls:Menu").Value);
});

builder.Services.AddHttpClient("AuthAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetSection("ApiUrls:Auth").Value);
});

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(40);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

app.UseCookiePolicy();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
