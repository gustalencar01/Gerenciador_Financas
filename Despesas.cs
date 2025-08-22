using System;
using System.Collections.Generic;

namespace Financas
{
    public struct Despesa
    {
        public string Descricao { get; set; }
        public double Valor { get; set; }
    }

    public class GerenciamentoDespesas
    {
        private readonly Dictionary<string, List<Despesa>> categorias;

        public GerenciamentoDespesas()
        {
            categorias = new Dictionary<string, List<Despesa>>()
            {
                { "Alimentação", new List<Despesa>() },
                { "Transporte", new List<Despesa>() },
                { "Moradia", new List<Despesa>() },
                { "Lazer", new List<Despesa>() },
                { "Saúde", new List<Despesa>() },
                { "Educação", new List<Despesa>() }
            };
        }

        // Adiciona uma despesa a uma categoria existente
        public void AdicionarDespesa(string categoria, string descricao, double valor)
        {
            if (!categorias.ContainsKey(categoria))
            {
                throw new ArgumentException("Categoria inválida.");
            }

            categorias[categoria].Add(new Despesa { Descricao = descricao, Valor = valor });
        }

        // Retorna o total de uma categoria
        public double ObterTotalPorCategoria(string categoria)
        {
            if (!categorias.ContainsKey(categoria)) return 0;

            double total = 0;
            foreach (var despesa in categorias[categoria])
            {
                total += despesa.Valor;
            }
            return total;
        }

        // Lista todas as despesas de uma categoria
        public IEnumerable<Despesa> ListarDespesas(string categoria)
        {
            return categorias.ContainsKey(categoria) ? categorias[categoria] : new List<Despesa>();
        }

        // Retorna todas as categorias
        public IEnumerable<string> ListarCategorias()
        {
            return categorias.Keys;
        }
    }
}
