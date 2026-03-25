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
        
        [HttpGet]
        public IActionResult GetDespesas()
        {
            return Ok(_context.Despesas.ToList());
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

        // PUT: api/Despesas/5
        [HttpPut("{id}")]
        public IActionResult PutDespesa(int id, [FromBody] DespesaEntity despesaAtualizada)
        {
            var despesaBanco = _context.Despesas.Find(id);

            if (despesaBanco == null)
            {
                return NotFound(new { mensagem = "Despesa não encontrada para edição." });
            }

            // Atualiza os dados
            despesaBanco.Descricao = despesaAtualizada.Descricao;
            despesaBanco.Valor = despesaAtualizada.Valor;
            despesaBanco.Categoria = despesaAtualizada.Categoria;

            _context.SaveChanges();

            return Ok(new { mensagem = "Despesa atualizada com sucesso!" });
        }

        // DELETE: api/Despesas/5
        [HttpDelete("{id}")]
        public IActionResult DeleteDespesa(int id)
        {
            var despesa = _context.Despesas.Find(id);

            if (despesa == null)
            {
                return NotFound(new { mensagem = "Despesa não encontrada para exclusão." });
            }

            _context.Despesas.Remove(despesa);
            _context.SaveChanges();

            return Ok(new { mensagem = "Despesa removida do banco!" });
        }
    }
}