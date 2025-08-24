using System;
using System.Collections.Generic;
using System.Linq;

namespace Financas
{
    public struct Receita
    {
        public string Descricao { get; set; }
        public double Valor { get; set; }
    }

    public class GerenciamentoReceitas
    {
        private readonly Dictionary<string, List<Receita>> categorias;

        public GerenciamentoReceitas()
        {
            categorias = new Dictionary<string, List<Receita>>()
            {
                { "Salário", new List<Receita>() },
                { "Investimentos", new List<Receita>() },
                { "Freelance", new List<Receita>() },
                { "Outras", new List<Receita>() }
            };
        }

        // Adiciona uma receita a uma categoria existente
        public void AdicionarReceita(string categoria, string descricao, double valor)
        {
            if (!categorias.ContainsKey(categoria))
            {
                throw new ArgumentException("Categoria inválida.");
            }

            categorias[categoria].Add(new Receita { Descricao = descricao, Valor = valor });
        }

        // Remove uma receita de uma categoria
        public void RemoverReceita(string categoria, string descricao)
        {
            if (!categorias.ContainsKey(categoria))
            {
                throw new ArgumentException("Categoria inválida.");
            }
            var receitas = categorias[categoria];
            var receita = receitas.FirstOrDefault(r => r.Descricao == descricao);
            if (receita.Equals(default(Receita)))
            {
                throw new ArgumentException("Receita não encontrada.");
            }
            receitas.Remove(receita);
        }

        // Retorna o total de uma categoria
        public double ObterTotalPorCategoria(string categoria)
        {
            if (!categorias.ContainsKey(categoria)) return 0;

            double total = 0;
            foreach (var receita in categorias[categoria])
            {
                total += receita.Valor;
            }
            return total;
        }

        // Lista todas as receitas de uma categoria
        public IEnumerable<Receita> ListarReceitas(string categoria)
        {
            return categorias.ContainsKey(categoria) ? categorias[categoria] : new List<Receita>();
        }

        // Retorna todas as categorias
        public IEnumerable<string> ListarCategorias()
        {
            return categorias.Keys;
        }
    }
}