using Microsoft.EntityFrameworkCore;

namespace SistemaFinancasApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<DespesaEntity> Despesas { get; set; }
        public DbSet<ReceitaEntity> Receitas { get; set; }
        public DbSet<LimiteEntity> Limites { get; set; } 
    }

    public class DespesaEntity
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public double Valor { get; set; }
        public string Categoria { get; set; }
        public DateTime Data { get; set; } 
        public bool Pago { get; set; } 
    }

    public class ReceitaEntity
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public double Valor { get; set; }
        public string Categoria { get; set; }
        public DateTime Data { get; set; } 
    }

    public class LimiteEntity
    {
        public int Id { get; set; }
        public string Categoria { get; set; }
        public double ValorLimite { get; set; }
    }
}