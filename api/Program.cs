using api.Data;
using api.Interfaces;
using api.Repositories;
using api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Repositories
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
#endregion

#region Services
builder.Services.AddScoped<IBrandVariationService, BrandVariationService>();
builder.Services.AddScoped<IBrandQueryService, BrandQueryService>();
#endregion

#region Controllers
builder.Services.AddControllers();

#endregion
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();

