using Microsoft.AspNetCore.Mvc;
using SistemaFinancasApi.Data; 
using Financas; // Nome do seu namespace original

namespace SistemaFinancasApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DespesasController : ControllerBase
    {
        private readonly AppDbContext _context; // Referência ao banco

        // O .NET injeta o banco automaticamente aqui
        public DespesasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult PostDespesa([FromBody] DespesaEntity novaDespesa)
        {
            try
            {
                // 1. Adiciona o objeto na fila do Entity Framework
                _context.Despesas.Add(novaDespesa);

                // 2. ORDEM CRÍTICA: Salva as mudanças fisicamente no SQL Server
                _context.SaveChanges();

                return Ok(new { mensagem = "Agora sim! Salvo no banco de dados." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
        }
    }
}