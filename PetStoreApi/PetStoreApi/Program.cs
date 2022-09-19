using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PetStoreApi.Helpers;
using PetStoreApi.Services.Repositories;
using PetStoreApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(option => {
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
        builder.AllowAnyOrigin()
       .AllowAnyMethod()
       .AllowAnyHeader());
});
builder.Services.AddScoped<ModelValidationAttribute>();
builder.Logging.AddJsonConsole();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

#region Repositories
builder.Services.AddScoped<IBreedRepository, BreedRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IOriginRepository, OriginRepository>();
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
