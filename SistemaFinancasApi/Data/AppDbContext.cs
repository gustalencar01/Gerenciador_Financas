using Microsoft.EntityFrameworkCore;
using Financas;

namespace SistemaFinancasApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Isso criará tabelas no banco de dados para Despesas e Receitas
        public DbSet<DespesaEntity> Despesas { get; set; }
        // Se quiser receitas depois, adicione o DbSet de receitas aqui
        public DbSet<ReceitaEntity> Receitas { get; set; }
    }

    // Criamos uma versão "Entity" da sua struct para o banco de dados
    public class DespesaEntity
    {
        public int Id { get; set; } // O SQL precisa de uma chave primária
        public string Descricao { get; set; }
        public double Valor { get; set; }
        public string Categoria { get; set; }
    }

    public class ReceitaEntity
    {
        public int Id { get; set; } // O SQL precisa de uma chave primária
        public string Descricao { get; set; }
        public double Valor { get; set; }
        public string Categoria { get; set; }
    }
}