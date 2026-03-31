using Microsoft.AspNetCore.Mvc;
using SistemaFinancasApi.Data; 
using System.Linq;

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
        public IActionResult PostDespesa([FromBody] DespesaEntity novaDespesa, [FromServices] IFinanceiroService financeiroService)
        {
            try
            {
                // O Controller não faz nada além de passar a bola para a Service
                var resultado = financeiroService.AdicionarDespesaComValidacao(novaDespesa);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = "Erro ao processar: " + ex.Message });
            }
        }


        // DELETE: api/Despesas/5
        [HttpDelete("{id}")]
        public IActionResult DeleteDespesa(int id, [FromServices] IFinanceiroService financeiroService)
        {
            try
            {
                var resultado = financeiroService.ExcluirDespesa(id);

                if (resultado == "Despesa não encontrada.")
                {
                    return NotFound(new { mensagem = resultado });
                }

                return Ok(new { mensagem = resultado });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = "Erro ao excluir: " + ex.Message });
            }
        }

        // PUT: api/Despesas/5
        [HttpPut("{id}")]
        public IActionResult PutDespesa(int id, [FromBody] DespesaEntity despesaAtualizada, [FromServices] IFinanceiroService financeiroService)
        {
            try
            {
                var resultado = financeiroService.AtualizarDespesa(id, despesaAtualizada);

                if (resultado == "Despesa não encontrada.")
                {
                    return NotFound(new { mensagem = resultado });
                }

                return Ok(new { mensagem = resultado });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = "Erro ao atualizar despesa: " + ex.Message });
            }
        }
    }
}