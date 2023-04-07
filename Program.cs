global using HealthCheck;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHealthChecks()
    .AddCheck("ICMP_01", new ICMPHealthCheck("www.facebook.com", 100))
	.AddCheck("ICMP_02", new ICMPHealthCheck("www.google.com", 100))
	.AddCheck("ICMP_03", new ICMPHealthCheck($"www.{Guid.NewGuid():N}.com", 100));

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseHealthChecks(new PathString("/api/health"), new CustomHealthCheckOptions());

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
