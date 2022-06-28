using TicketFrontend.Service;
using Auth0.AspNetCore.Authentication;
using TicketAPI.DAL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient<ITicketService, TicketService>();
builder.Services.AddHttpClient<IPropertyService, PropertyService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddRazorPages();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None;
});
builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = builder.Configuration["Auth0:Domain"];
    options.ClientId = builder.Configuration["Auth0:ClientID"];
    options.ClientSecret = builder.Configuration["Auth0:ClientSecret"];
    options.Scope = "openid profile email roles";
}).WithAccessToken(options =>
{
    options.Audience = builder.Configuration["Auth0:Audience"];
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();