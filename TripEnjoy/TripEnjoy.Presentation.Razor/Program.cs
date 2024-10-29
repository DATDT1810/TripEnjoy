using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using TripEnjoy.Presentation.Razor.Helper;
using TripEnjoy.Presentation.Razor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()  // Cho phép tất cả các nguồn
                   .AllowAnyMethod()  // Cho phép tất cả các phương thức (GET, POST, v.v.)
                   .AllowAnyHeader(); // Cho phép tất cả các tiêu đề
        });
});

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<TokenServices>();
builder.Services.AddTransient<TokenHandler>();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient("DefaultClient").AddHttpMessageHandler<TokenHandler>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;

}).AddCookie().AddGoogle(options =>
{
    options.ClientId = builder.Configuration.GetSection("GoogleAuthSetting").GetValue<string>("ClientId");
    options.ClientSecret = builder.Configuration.GetSection("GoogleAuthSetting").GetValue<string>("ClientSecret");
});

var app = builder.Build();
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Thêm dòng này để kích hoạt Authentication
app.UseAuthorization();

app.UseEndpoints(endpoints =>  // Thêm Endpoint Mapping cho Razor Pages
{
    endpoints.MapRazorPages();
});

app.Run();
