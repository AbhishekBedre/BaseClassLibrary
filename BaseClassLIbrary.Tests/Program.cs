using BaseClassLibrary;
using BaseClassLibrary.Interface;
using BaseClassLibrary.Repository;
using BaseClassLIbrary.Tests;
using BaseClassLIbrary.Tests.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddBaseLibraryServices();

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(@"Data Source=DESKTOP-P77TN5G;Initial Catalog=Test;Integrated Security=True"));

builder.Services.AddScoped<ICompanyMasterRepository, CompanyMasterRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
