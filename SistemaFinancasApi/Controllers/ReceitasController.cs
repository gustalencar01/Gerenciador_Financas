using Microsoft.AspNetCore.Mvc;
using SistemaFinancasApi.Data; 
using Financas; // O namespace onde estão suas structs e classes

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
    }
}