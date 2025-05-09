using System.Text.Json.Serialization;
using Ecommerce.BackendAPI.Data;
using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.BackendAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Ecommerce.BackendAPI.Services;
using Ecommerce.BackendAPI.Interfaces.Helper;
using Ecommerce.BackendAPI.Helper;
using Ecommerce.BackendAPI.FiltersAction;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Add custom filters and services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<VerifyToken>();
builder.Services.AddScoped<CheckUserExists>();
builder.Services.AddScoped<CategoryAndParentAndClassificationFilter>();

// Product filters
builder.Services.AddScoped<UpdateAndDeleteProductFilter>();

// Cart filters
builder.Services.AddScoped<AddToCartFilter>();
builder.Services.AddScoped<RemoveFromCartFilter>();

// Order filters
builder.Services.AddScoped<GetOrderFilter>();
builder.Services.AddScoped<GetAllOrdersFilter>();
builder.Services.AddScoped<CreateOrderFilter>();

// User filter
builder.Services.AddScoped<VerifyAdmin>();
builder.Services.AddScoped<VerifyCustomer>();

// Review filters
builder.Services.AddScoped<VerifyWhenCreateReview>();
builder.Services.AddScoped<VerifyWhenUpdateAndDeleteReview>();

// Add custom repositories
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IDependMethod, DependMethod>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IVariantRepository, VariantRepository>();
builder.Services.AddScoped<IParentCategoryRepo, ParentCategoryRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IClassificationRepository, ClassificationRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddSwaggerGen(c =>
// {
//     c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
//     c.OperationFilter<FileUploadOperationFilter>();
// });
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("cookieAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Cookie,
        Name = "access_token", // Name of the cookie
        Description = "JWT Authorization cookie"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "cookieAuth"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.WithOrigins
            (
                "http://localhost:5173",
                "http://localhost:5122"
            ) // Your React app's origin
               .AllowAnyMethod() // Allow GET, POST, etc.
               .AllowAnyHeader() // Allow headers like Authorization
               .AllowCredentials(); // If using cookies or auth headers
        });
});

builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");

app.UseWhen(
    context => 
        (
            context.Request.Path.StartsWithSegments("/api/Product") || 
            context.Request.Path.StartsWithSegments("/api/Variant") ||
            context.Request.Path.StartsWithSegments("/api/File")
        )
        && context.Request.Method == "POST", 
    appBuilder =>
    {
        appBuilder.UploadProducts();
    }
);

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
