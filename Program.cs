using System;
using System.Collections.Generic;
using System.Linq;

namespace Financas
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bem-vindo ao sistema de finanças pessoais!");
            var despesas = new GerenciamentoDespesas();
            var receitas = new GerenciamentoReceitas();
            Boolean control = true;

            while (control)
            {
                Console.WriteLine();
                Console.WriteLine("Por favor, escolha uma opção:");
                Console.WriteLine("1. Gerenciar despesas");
                Console.WriteLine("2. Gerenciar receitas");
                Console.WriteLine("3. Para sair do sistema");
                Console.Write("Opção: ");
                var opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        MenuDespesas(despesas);
                        break;
                    case "2":
                        MenuReceitas(receitas);
                        break;
                    case "3":
                        Console.WriteLine("Obrigado por usar o sistema de finanças pessoais. Até a próxima!");
                        control = false;
                        break;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            }
        }

        static void MenuDespesas(GerenciamentoDespesas despesas)
        {
            while (true)
            {
                Console.WriteLine("\n--- Menu de Gerenciamento de Despesas ---");
                Console.WriteLine("1. Adicionar nova despesa");
                Console.WriteLine("2. Obter valor total de uma categoria");
                Console.WriteLine("3. Listar despesas de uma categoria");
                Console.WriteLine("4. Remover despesa de uma categoria");
                Console.WriteLine("5. Sair");
                Console.Write("Escolha uma opção: ");
                var opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        AdicionarDespesa(despesas);
                        break;
                    case "2":
                        ObterTotalDespesas(despesas);
                        break;
                    case "3":
                        ListarDespesas(despesas);
                        break;
                    case "4":
                        RemoverDespesa(despesas);
                        break;
                    case "5":
                        Console.WriteLine("Saindo do gerenciamento de despesas.");
                        return;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            }
        }

        static void MenuReceitas(GerenciamentoReceitas receitas)
        {
            while (true)
            {
                Console.WriteLine("\n--- Menu de Gerenciamento de Receitas ---");
                Console.WriteLine("1. Adicionar nova receita");
                Console.WriteLine("2. Listar receitas de uma categoria");
                Console.WriteLine("3. Listar categorias de receita");
                Console.WriteLine("4. Remover receita por categoria");
                Console.WriteLine("5. Sair");
                Console.Write("Escolha uma opção: ");
                var opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        AdicionarReceita(receitas);
                        break;
                    case "2":
                        ListarReceitas(receitas);
                        break;
                    case "3":
                        ListarCategoriasReceita(receitas);
                        break;
                    case "4":
                        RemoverReceita(receitas);
                        break;
                    case "5":
                        Console.WriteLine("Saindo do gerenciamento de receitas.");
                        return;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            }
        }

        static void AdicionarDespesa(GerenciamentoDespesas despesas)
        {
            String res;
            var categorias = new List<string>(despesas.ListarCategorias());

            if (categorias.Count == 0)
            {
                Console.WriteLine("Nenhuma categoria encontrada. Crie uma despesa para começar.");
                return;
            }

            for (int i = 0; i < categorias.Count; i++)
                Console.WriteLine($"{i + 1}. {categorias[i]}");

            Console.WriteLine("Deseja adicionar categoria personalizada?");
            res = Console.ReadLine();
            if (res == "s")
            {
                    Console.Write("Digite o nome da nova categoria: ");
                    var novaCategoria = Console.ReadLine();
                    try
                    {
                        despesas.AdicionarCategoria(novaCategoria);
                        Console.WriteLine($"Categoria '{novaCategoria}' adicionada com sucesso!");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
            }

            Console.WriteLine();
            Console.Write("Escolha o número da categoria: ");
            if (!int.TryParse(Console.ReadLine(), out int escolha) || escolha < 1 || escolha > categorias.Count)
            {
                Console.WriteLine("Opção inválida.");
                return;
            }

            var categoriaEscolhida = categorias[escolha - 1];
            Console.WriteLine($"Você escolheu: {categoriaEscolhida}");

            Console.Write("Digite a descrição da despesa: ");
            var descricao = Console.ReadLine();

            Console.Write("Digite o valor da despesa: ");
            if (!double.TryParse(Console.ReadLine(), out double valor))
            {
                Console.WriteLine("Valor inválido.");
                return;
            }

            try
            {
                despesas.AdicionarDespesa(categoriaEscolhida, descricao, valor);
                Console.WriteLine($"Despesa adicionada com sucesso na categoria {categoriaEscolhida}!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void ObterTotalDespesas(GerenciamentoDespesas despesas)
        {
            Console.Write("Digite o nome da categoria para obter o total: ");
            var categoria = Console.ReadLine();
            try
            {
                Console.WriteLine($"Total em {categoria}: {despesas.ObterTotalPorCategoria(categoria):C}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void ListarDespesas(GerenciamentoDespesas despesas)
        {
            Console.Write("Digite o nome da categoria para listar as despesas: ");
            var categoria = Console.ReadLine();
            try
            {
                var lista = despesas.ListarDespesas(categoria);
                if (lista.Any())
                {
                    Console.WriteLine($"Despesas na categoria {categoria}:");
                    foreach (var d in lista)
                        Console.WriteLine($"Descrição: {d.Descricao}, Valor: {d.Valor:C}");
                }
                else
                {
                    Console.WriteLine($"Nenhuma despesa encontrada para a categoria {categoria}.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void RemoverDespesa(GerenciamentoDespesas despesas)
        {
            Console.Write("Digite o nome da categoria da despesa que deseja remover: ");
            var categoria = Console.ReadLine();
            Console.Write("Digite a descrição da despesa que deseja remover: ");
            var descricao = Console.ReadLine();
            try
            {
                despesas.RemoverDespesa(categoria, descricao);
                Console.WriteLine($"Despesa '{descricao}' removida da categoria '{categoria}' com sucesso!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void AdicionarReceita(GerenciamentoReceitas receitas)
        {
            String res;
            var categ = new List<string>(receitas.ListarCategorias());
            for (int i = 0; i < categ.Count; i++)
                Console.WriteLine($"{i + 1}. {categ[i]}");

            Console.WriteLine("Deseja adicionar categoria personalizada?");
            res = Console.ReadLine();
            if (res == "s")
            {
                    Console.Write("Digite o nome da nova categoria: ");
                    var novaCategoria = Console.ReadLine();
                    try
                    {
                        receitas.AdicionarCategoria(novaCategoria);
                        Console.WriteLine($"Categoria '{novaCategoria}' adicionada com sucesso!");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
            
            }
            Console.WriteLine();


            Console.Write("Escolha uma categoria: ");
            var categoria = Console.ReadLine();

            Console.Write("Digite a descrição da receita: ");
            var descricao = Console.ReadLine();

            Console.Write("Digite o valor da receita: ");
            if (!double.TryParse(Console.ReadLine(), out double valor))
            {
                Console.WriteLine("Valor inválido.");
                return;
            }

            try
            {
                receitas.AdicionarReceita(categoria, descricao, valor);
                Console.WriteLine($"Receita adicionada com sucesso na categoria {categoria}!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void ListarReceitas(GerenciamentoReceitas receitas)
        {
            Console.Write("Digite o nome da categoria para listar as receitas: ");
            var categoria = Console.ReadLine();
            try
            {
                var lista = receitas.ListarReceitas(categoria);
                if (lista.Any())
                {
                    Console.WriteLine($"Receitas na categoria {categoria}:");
                    foreach (var r in lista)
                        Console.WriteLine($"Descrição: {r.Descricao}, Valor: {r.Valor:C}");
                }
                else
                {
                    Console.WriteLine($"Nenhuma receita encontrada para a categoria {categoria}.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void ListarCategoriasReceita(GerenciamentoReceitas receitas)
        {
            try
            {
                var categorias = new List<string>(receitas.ListarCategorias());
                if (categorias.Any())
                {
                    Console.WriteLine("Categorias de receita existentes:");
                    foreach (var c in categorias)
                        Console.WriteLine($"- {c}");
                }
                else
                {
                    Console.WriteLine("Nenhuma categoria de receita encontrada.");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void RemoverReceita(GerenciamentoReceitas receitas)
        {
            Console.Write("Digite o nome da categoria da receita que deseja remover: ");
            var categoria = Console.ReadLine();
            Console.Write("Digite a descrição da receita que deseja remover: ");
            var descricao = Console.ReadLine();
            try
            {
                receitas.RemoverReceita(categoria, descricao);
                Console.WriteLine($"Receita '{descricao}' removida da categoria '{categoria}' com sucesso!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
