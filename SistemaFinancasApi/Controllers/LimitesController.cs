using Microsoft.AspNetCore.Mvc;
using SistemaFinancasApi.Data; 
using System.Linq;

namespace SistemaFinancasApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LimitesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LimitesController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/Limites
        [HttpPost]
        public IActionResult PostLimite([FromBody] LimiteEntity novoLimite, [FromServices] IFinanceiroService financeiroService)
        {
            try
            {
                // O Controller não toma nenhuma decisão lógica
                var resultado = financeiroService.SalvarOuAtualizarLimite(novoLimite);

                return Ok(new { mensagem = resultado });
            }
            catch (Exception ex)
            {
                // Caso algo dê errado no Banco ou na Service, o Controller captura aqui
                return BadRequest(new { erro = "Erro interno ao processar limite: " + ex.Message });
            }
        }

        // GET: api/Limites
        [HttpGet]
        public IActionResult GetLimites()
        {
            var lista = _context.Limites.ToList();
            return Ok(lista);
        }

        // DELETE: api/Limites/5
        [HttpDelete("{id}")]
        public IActionResult DeleteLimite(int id, [FromServices] IFinanceiroService financeiroService)
        {
            try
            {
                var resultado = financeiroService.ExcluirLimite(id);

                if (resultado == "Limite não encontrado.")
                {
                    return NotFound(new { mensagem = resultado });
                }

                return Ok(new { mensagem = resultado });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = "Erro ao excluir limite: " + ex.Message });
            }
        }
    }
}