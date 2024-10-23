using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TripEnjoy.Application.Data;
using TripEnjoy.Application.Interface;
using TripEnjoy.Application.Interface.Booking_Room;
using TripEnjoy.Application.Interface.Category;
using TripEnjoy.Application.Interface.Comment;
using TripEnjoy.Application.Interface.EmailService;
using TripEnjoy.Application.Interface.Hotel;
using TripEnjoy.Application.Interface.ImageCloud;
using TripEnjoy.Application.Interface.Payment;
using TripEnjoy.Application.Interface.Rate;
using TripEnjoy.Application.Interface.Room;
using TripEnjoy.Application.Interface.RoomImage;
using TripEnjoy.Application.Interface.RoomType;
using TripEnjoy.Application.Interface.User;
using TripEnjoy.Application.Services;
using TripEnjoy.Application.Services.Booking;
using TripEnjoy.Application.Services.Category;
using TripEnjoy.Application.Services.Comment;
using TripEnjoy.Application.Services.Email;
using TripEnjoy.Application.Services.ImageCloud;
using TripEnjoy.Application.Services.Payment;
using TripEnjoy.Application.Services.Rate;
using TripEnjoy.Application.Services.Room;
using TripEnjoy.Application.Services.RoomImage;
using TripEnjoy.Application.Services.RoomType;
using TripEnjoy.Application.Services.User;
using TripEnjoy.Infrastructure.Entities;
using TripEnjoy.Infrastructure.Helper;
using TripEnjoy.Infrastructure.Repositories;
using TripEnjoy.Infrastructure.Service;
using TripEnjoy.Presentation.Web.Middleware;


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
});
// Cấu hình  cloudinary cho ứng dụng
builder.Services.Configure<CloudinarySetting>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<ImageManagementServices>();
// cấu hình smtp cho email
builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("EmailSettings"));
// cấu hình auto mapper
builder.Services.AddAutoMapper(typeof(MappingProfile));
//Khai Báo Dependency Cho các tầng sử dụng
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IHotelService, HotelService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IUserServices, UserService>();

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddSingleton<IVnPayServices, VnPayService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IRoomService, RoomService>();

builder.Services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
builder.Services.AddScoped<IRoomTypeService, RoomTypeService>();

builder.Services.AddScoped<IRoomImageRepository, RoomImageRepository>();
builder.Services.AddScoped<IRoomImageService, RoomImageService>();

builder.Services.AddScoped<IRateRepository, RateRepository>();
builder.Services.AddScoped<IRateService, RateService>();

builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();
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

app.UseAuthorization();

app.UseSession();
app.MapControllers();

app.Run();
