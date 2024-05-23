using BlazorApp1.Components;
using BlazorApp1.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddHttpContextAccessor();
//## for BLAZOR COOKIE Auth
/// ref → https://blazorhelpwebsite.com/ViewBlogPost/36
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Strict;
    options.CheckConsentNeeded = context => true;
    options.ConsentCookie.Name = "GDPRConsent";
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(cfg =>
    {
        cfg.LoginPath = "/Accout/Login"; // default: /Accout/Login
        cfg.Cookie.Name = ".N8BlazorServerAuth.Cookies"; //default:.AspNetCore.Cookies
    });
builder.Services.AddCascadingAuthenticationState();

//§§ 註冊：客製服務
builder.Services.AddSingleton<AccountService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
//# for BLAZOR COOKIE Auth
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
