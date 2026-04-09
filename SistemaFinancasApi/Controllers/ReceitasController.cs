using Microsoft.AspNetCore.Mvc;
using SistemaFinancasApi.Data; 
using System.Linq;

namespace SistemaFinancasApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceitasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReceitasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetReceitas()
        {
            return Ok(_context.Receitas.ToList());
        }

        [HttpPost]
        public IActionResult PostReceita([FromBody] ReceitaEntity novaReceita, [FromServices] IFinanceiroService financeiroService)
        {
            try
            {
                var resultado = financeiroService.AdicionarReceita(novaReceita);
                return Ok(new { mensagem = resultado });
            }
            catch(Exception ex)
            {
                return BadRequest(new { erro = "Erro ao processar: " + ex.Message });
            }

        }

        [HttpPut("{id}")]
        public IActionResult PutReceita(int id, [FromBody] ReceitaEntity receitaAtualizada, [FromServices] IFinanceiroService financeiroService)
        {
            try
            {
                var resultado = financeiroService.AtualizarReceita(id, receitaAtualizada);

                if (resultado == "Receita não encontrada.")
                {
                    return NotFound(new { mensagem = resultado });
                }

                if (resultado.StartsWith("Erro"))
                {
                    return BadRequest(new { mensagem = resultado });
                }

                return Ok(new { mensagem = resultado });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = "Falha ao processar atualização: " + ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReceita(int id, [FromServices] IFinanceiroService financeiroService)
        {
            try
            {
                var resultado = financeiroService.ExcluirReceita(id);

                if (resultado == "Receita não encontrada.")
                {
                    return NotFound(new { mensagem = resultado });
                }

                if (resultado.StartsWith("Erro"))
                {
                    return BadRequest(new { mensagem = resultado });
                }

                return Ok(new { mensagem = resultado });
            }
            catch (Exception ex)
            {
                return BadRequest(new { erro = "Falha na comunicação: " + ex.Message });
            }
        }
    }
}