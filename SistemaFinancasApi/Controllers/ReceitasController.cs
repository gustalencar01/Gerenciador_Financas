using Microsoft.AspNetCore.Mvc;
using SistemaFinancasApi.Data; 
using Financas; 
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
        public IActionResult PostReceita([FromBody] ReceitaEntity novaReceita)
        {
            _context.Receitas.Add(novaReceita);
            _context.SaveChanges();
            return Ok(new { mensagem = "Receita salva com sucesso!" });
        }

        // PUT: api/Receitas/5
        [HttpPut("{id}")]
        public IActionResult PutReceita(int id, [FromBody] ReceitaEntity receitaAtualizada)
        {
            var receitaBanco = _context.Receitas.Find(id);

            if (receitaBanco == null)
            {
                return NotFound(new { mensagem = "Receita não encontrada para edição." });
            }

            // Atualiza os dados
            receitaBanco.Descricao = receitaAtualizada.Descricao;
            receitaBanco.Valor = receitaAtualizada.Valor;
            receitaBanco.Categoria = receitaAtualizada.Categoria;

            _context.SaveChanges();

            return Ok(new { mensagem = "Receita atualizada com sucesso!" });
        }

        // DELETE: api/Receita
        [HttpDelete("{id}")]
        public IActionResult DeleteReceita(int id)
        {
            var receita = _context.Receitas.Find(id);

            if (receita == null)
            {
                return NotFound(new { mensagem = "Receita não encontrada para exclusão." });
            }

            _context.Receitas.Remove(receita);
            _context.SaveChanges();

            return Ok(new { mensagem = "Receita removida do banco!" });
        }
    }
}