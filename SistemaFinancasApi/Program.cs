using Financas;
using Microsoft.EntityFrameworkCore;
using SistemaFinancasApi.Data;

var builder = WebApplication.CreateBuilder(args);

// --- SERVIÇOS ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // O Swagger básico não precisa de 'using' extra

builder.Services.AddSingleton<GerenciamentoDespesas>();
builder.Services.AddSingleton<GerenciamentoReceitas>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// --- PIPELINE ---

// Ativa o Swagger sempre
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sistema Financas V1");
    c.RoutePrefix = string.Empty; // Abre direto no localhost:5000
});

app.MapControllers();

app.Run();