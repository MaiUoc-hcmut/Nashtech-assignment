using System.Text.Json.Serialization;
using Ecommerce.BackendAPI.Data;
using Ecommerce.BackendAPI.Interfaces;
using Ecommerce.BackendAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Ecommerce.BackendAPI.Services;
using Ecommerce.BackendAPI.Interfaces.Helper;
using Ecommerce.BackendAPI.Helper;
using Ecommerce.BackendAPI.FiltersAction;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<VerifyToken>();
builder.Services.AddScoped<CheckUserExists>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IDependMethod, DependMethod>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IParentCategoryRepo, ParentCategoryRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
// builder.Services.AddScoped<ICartRepository, CartRepository>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
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

app.UseWhen(
    context => 
        (
            // context.Request.Path.StartsWithSegments("/api/Product") || 
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
