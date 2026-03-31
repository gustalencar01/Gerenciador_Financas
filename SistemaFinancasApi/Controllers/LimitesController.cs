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

        [HttpPost]
        public IActionResult PostLimite([FromBody] LimiteEntity novoLimite, [FromServices] IFinanceiroService financeiroService)
        {
            try
            {
                var resultado = financeiroService.SalvarOuAtualizarLimite(novoLimite);

                return Ok(new { mensagem = resultado });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = "Erro interno ao processar limite: " + ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetLimites()
        {
            var lista = _context.Limites.ToList();
            return Ok(lista);
        }

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