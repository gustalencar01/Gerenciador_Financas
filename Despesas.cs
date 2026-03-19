using System;
using System.Collections.Generic;
using System.Linq;

namespace Financas
{
    public struct Despesa
    {
        public string Descricao { get; set; }
        public double Valor { get; set; }
    }

    public class GerenciamentoDespesas
    {
        private readonly Dictionary<string, List<Despesa>> _categorias;

        public GerenciamentoDespesas()
        {
            _categorias = new Dictionary<string, List<Despesa>>(StringComparer.OrdinalIgnoreCase)
            {
                { "Alimentação", new List<Despesa>() },
                { "Transporte", new List<Despesa>() },
                { "Moradia", new List<Despesa>() },
                { "Lazer", new List<Despesa>() },
                { "Saúde", new List<Despesa>() },
                { "Educação", new List<Despesa>() }
            };
        }

        public void AdicionarDespesa(string categoria, string descricao, double valor)
        {
            if (string.IsNullOrWhiteSpace(categoria) || !_categorias.ContainsKey(categoria))
                throw new ArgumentException("Categoria inválida.");

            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("Descrição não pode ser vazia.");

            if (valor < 0)
                throw new ArgumentException("O valor da despesa não pode ser negativo.");

            _categorias[categoria].Add(new Despesa { Descricao = descricao, Valor = valor });
        }

        public void RemoverDespesa(string categoria, string descricao)
        {
            if (string.IsNullOrWhiteSpace(categoria) || !_categorias.ContainsKey(categoria))
                throw new ArgumentException("Categoria inválida.");

            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("Descrição não pode ser vazia.");

            var despesas = _categorias[categoria];
            var index = despesas.FindIndex(d => d.Descricao == descricao);

            if (index < 0)
                throw new ArgumentException("Despesa não encontrada.");

            despesas.RemoveAt(index);
        }

        public double ObterTotalPorCategoria(string categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria) || !_categorias.ContainsKey(categoria))
                throw new ArgumentException("Categoria inválida.");

            return _categorias[categoria].Sum(d => d.Valor);
        }

        public IEnumerable<Despesa> ListarDespesas(string categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria) || !_categorias.ContainsKey(categoria))
                throw new ArgumentException("Categoria inválida.");

            return _categorias[categoria];
        }

        public IEnumerable<string> ListarCategorias()
        {
            return _categorias.Keys;
        }
    }
}
