using DirectorioAPI.Data;
using DirectorioAPI.Repositories;
using DirectorioAPI.Services;
using DirectorioCore.Interfaces;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<DirectorioDbContext>(options =>
    options.UseSqlite("Data Source=directorio.db"));

builder.Services.AddScoped<IPersonaRepository, PersonaRepository>();
builder.Services.AddScoped<IFacturaRepository, FacturaRepository>();
builder.Services.AddScoped<DirectorioService>();
builder.Services.AddScoped<VentasService>();



var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DirectorioDbContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();
