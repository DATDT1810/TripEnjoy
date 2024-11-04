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

// Cấu hình cho Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian hết hạn Session
    options.Cookie.HttpOnly = true; // Đảm bảo chỉ có thể truy cập Session cookie qua HTTP
    options.Cookie.IsEssential = true; // Cần thiết cho hoạt động của ứng dụng
});

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

app.UseAuthentication();
app.UseAuthorization();

// Kích hoạt Session Middleware
app.UseSession();  // Phải được thêm trước app.UseEndpoints

app.UseEndpoints(endpoints =>  // Thêm Endpoint Mapping cho Razor Pages
{
    endpoints.MapRazorPages();
});

app.Run();
