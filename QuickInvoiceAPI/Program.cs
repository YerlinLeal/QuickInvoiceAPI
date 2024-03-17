using Microsoft.EntityFrameworkCore;
using QuickInvoiceAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<QuickInvoiceBdContext>
    (opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlStringConnection")));

builder.Services.AddControllers().AddJsonOptions
    (opt => opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

var rulesCors = "RulesCors";
builder.Services.AddCors
    (opt => {
        opt.AddPolicy(name: rulesCors, builder =>
        {
            builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(rulesCors);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
