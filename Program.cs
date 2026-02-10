using Ecommerce_ASP.NET.Data;
using Ecommerce_ASP.NET.DTOs.AddOrderItem;
using Ecommerce_ASP.NET.DTOs.Address;
using Ecommerce_ASP.NET.DTOs.Cart;
using Ecommerce_ASP.NET.DTOs.Category;
using Ecommerce_ASP.NET.DTOs.Dashboard;
using Ecommerce_ASP.NET.DTOs.Discount;
using Ecommerce_ASP.NET.DTOs.Product;
using Ecommerce_ASP.NET.DTOs.Review;
using Ecommerce_ASP.NET.DTOs.UserDto;
using Ecommerce_ASP.NET.Helpers;
using Ecommerce_ASP.NET.Manager;
using Ecommerce_ASP.NET.Manager.Excel;
using Ecommerce_ASP.NET.Manager.Report;
using Ecommerce_ASP.NET.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("Connection"),
        new MySqlServerVersion(new Version(8, 0, 30))
    )
);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            ),
            ValidateLifetime = true
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Ecommerce API",
        Version = "v1",
        Description = "Ecommerce ASP.NET API"
    });

    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());


    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});


builder.Services.AddScoped<AuthManager>();
builder.Services.AddScoped<ProductManager>();
builder.Services.AddScoped<CategoryManager>();
builder.Services.AddScoped<CartManager>();
builder.Services.AddScoped<OrderManager>();
builder.Services.AddScoped<WishlistItemsManager>();
builder.Services.AddScoped<ReviewManager>();
builder.Services.AddScoped<DiscountManager>();
builder.Services.AddScoped<PaymentManager>();
builder.Services.AddScoped<UserManagmentManeger>();
builder.Services.AddScoped<NotificationManager>();
builder.Services.AddScoped<InventoryManagementManager>();
builder.Services.AddScoped<ReportManager>();
builder.Services.AddScoped<ProductPerformanceReportManager>();
builder.Services.AddScoped<ExportSalesReportToExcel>();
builder.Services.AddScoped<JwtHelper>();
builder.Services.AddScoped<UserManager>();
builder.Services.AddScoped<UpdateProfile>();
builder.Services.AddScoped<CategoryDto>();
builder.Services.AddScoped<AddProduct>();
builder.Services.AddScoped<CartDto>();
builder.Services.AddScoped<AddressDto>();
builder.Services.AddScoped<ReviewDto>();
builder.Services.AddScoped<DiscountUserDto>();
builder.Services.AddScoped<OrderItemsDto>();
builder.Services.AddScoped<AdminDashboardDto>();
builder.Services.AddScoped<BankApprove>();
builder.Services.AddScoped<PasswordHasher>();
builder.Services.AddScoped<OrderTrackings>();
builder.Services.AddScoped<TrackingHistories>();

var app = builder.Build();


// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();   
app.UseAuthorization();
app.MapControllers();

app.Run();

