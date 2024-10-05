using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TripEnjoy.Application.Data;
using TripEnjoy.Application.Interface;
using TripEnjoy.Application.Interface.EmailService;
using TripEnjoy.Application.Interface.Hotel;
using TripEnjoy.Application.Services;
using TripEnjoy.Application.Services.Email;
using TripEnjoy.Infrastructure.Entities;
using TripEnjoy.Infrastructure.Helper;
using TripEnjoy.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình CORS trước khi build ứng dụng
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
// thêm session vào ứng dụng
builder.Services.AddDistributedMemoryCache(); 
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian timeout
    options.Cookie.HttpOnly = true; // Bảo mật cookie
    options.Cookie.IsEssential = true; // Chấp nhận sử dụng cookie ngay cả khi không được phép
});
// cấu hình database connection
builder.Services.AddDbContext<ApplicationDbContext>(options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
} 
);
// cấu hình smtp cho email
builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("EmailSettings"));
// cấu hình auto mapper
builder.Services.AddAutoMapper(typeof(MappingProfile));
// Khai Báo Dependency Cho các tầng sử dụng
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IHotelService, HotelService>();

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddTransient<IEmailService, EmailService>();
// Add services to the container.
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => 
{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = false;

}).AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Phân quyền với Role
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicry" , policy => policy.RequireRole("Admin"));
    options.AddPolicy("StaffPolicy", policy => policy.RequireRole("Staff"));
    options.AddPolicy("Partner", policy => policy.RequireRole("Partner"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
});

builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("EmailSetttings"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Áp dụng chính sách CORS sau khi ứng dụng đã được build
app.UseCors("AllowAllOrigins");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();
app.MapControllers();

app.Run();
