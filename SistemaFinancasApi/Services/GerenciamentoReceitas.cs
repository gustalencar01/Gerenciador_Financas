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
        private readonly Dictionary<string, List<Receita>> _categorias;

        public GerenciamentoReceitas()
        {
            _categorias = new Dictionary<string, List<Receita>>(StringComparer.OrdinalIgnoreCase)
            {
                { "Salário", new List<Receita>() },
                { "Investimentos", new List<Receita>() },
                { "Freelance", new List<Receita>() }
            };
        }

        public void AdicionarCategoria(string categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria))
                throw new ArgumentException("Nome da categoria não pode ser vazio.");
            if (_categorias.ContainsKey(categoria))
                throw new ArgumentException("Categoria já existe.");
            _categorias.Add(categoria, new List<Receita>());
        }
        public void AdicionarReceita(string categoria, string descricao, double valor)
        {
            if (string.IsNullOrWhiteSpace(categoria) || !_categorias.ContainsKey(categoria))
                throw new ArgumentException("Categoria inválida.");

            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("Descrição não pode ser vazia.");

            if (valor < 0)
                throw new ArgumentException("O valor da receita não pode ser negativo.");

            _categorias[categoria].Add(new Receita { Descricao = descricao, Valor = valor });
        }

        public void RemoverReceita(string categoria, string descricao)
        {
            if (string.IsNullOrWhiteSpace(categoria) || !_categorias.ContainsKey(categoria))
                throw new ArgumentException("Categoria inválida.");

            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException("Descrição não pode ser vazia.");

            var receitas = _categorias[categoria];
            var index = receitas.FindIndex(r => r.Descricao == descricao);

            if (index < 0)
                throw new ArgumentException("Receita não encontrada.");

            receitas.RemoveAt(index);
        }

        public double ObterTotalPorCategoria(string categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria) || !_categorias.ContainsKey(categoria))
                throw new ArgumentException("Categoria inválida.");

            return _categorias[categoria].Sum(r => r.Valor);
        }

        public IEnumerable<Receita> ListarReceitas(string categoria)
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
