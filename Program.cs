using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Financas
{
    public class Program
    {
        static void Main(string[] args)
        {
            string resposta;
            bool respBoll = true;
            System.Console.WriteLine("Bem-vindo ao sistema de finanças pessoais!");

            GerenciamentoDespesas despesas = new GerenciamentoDespesas();
            GerenciamentoReceitas receitas = new GerenciamentoReceitas();



            while (true)
            {       
                    System.Console.WriteLine("Você deseja continuar a gerenciar suas finanças? (s/n)");
                    resposta = Console.ReadLine().ToLower();                

                    if (resposta != "s")
                    {
                        respBoll = false;
                        System.Console.WriteLine("Saindo do sistema de finanças pessoais.");
                        return;
                    }
                
                System.Console.WriteLine();
                System.Console.WriteLine("Por favor, escolha uma opção:");
                System.Console.WriteLine("1. Gerenciar despesas");
                System.Console.WriteLine("2. Gerenciar receitas");
                
                string opcao = Console.ReadLine();
                switch (opcao)
                {
                    case "1":
                        while (true)
                        {
                            Console.WriteLine("--- Menu de Gerenciamento de Despesas ---");
                            Console.WriteLine("1. Adicionar nova despesa");
                            Console.WriteLine("2. Obter valor total de uma categoria");
                            Console.WriteLine("3. Listar despesas de uma categoria");
                            Console.WriteLine("4. Sair");
                            Console.Write("Escolha uma opção: ");

                            string opcaoDespesas = Console.ReadLine();

                            switch (opcaoDespesas)
                            {
                                case "1":
                                    // Lógica para adicionar despesa
                                    var categorias = new List<string>(despesas.ListarCategorias());
                                    if (categorias.Count == 0)
                                    {
                                        Console.WriteLine("Nenhuma categoria encontrada. Crie uma despesa para começar.");
                                        break;
                                    }

                                    for (int i = 0; i < categorias.Count; i++)
                                        Console.WriteLine($"{i + 1}. {categorias[i]}");

                                    Console.WriteLine("Escolha o número da categoria:");
                                    if (!int.TryParse(Console.ReadLine(), out int escolha) || escolha < 1 || escolha > categorias.Count)
                                    {
                                        Console.WriteLine("Opção inválida.");
                                        break;
                                    }

                                    string categoriaEscolhida = categorias[escolha - 1];
                                    Console.WriteLine($"Você escolheu: {categoriaEscolhida}");

                                    Console.Write("Digite a descrição da despesa: ");
                                    string descricao = Console.ReadLine();

                                    Console.Write("Digite o valor da despesa: ");
                                    if (!double.TryParse(Console.ReadLine(), out double valor))
                                    {
                                        Console.WriteLine("Valor inválido.");
                                        break;
                                    }

                                    despesas.AdicionarDespesa(categoriaEscolhida, descricao, valor);
                                    Console.WriteLine($"Despesa adicionada com sucesso na categoria {categoriaEscolhida}!");
                                    break;

                                case "2":
                                    // Lógica para obter o valor total
                                    Console.Write("Digite o nome da categoria para obter o total: ");
                                    string categoriaTotal = Console.ReadLine();
                                    Console.WriteLine($"Total em {categoriaTotal}: {despesas.ObterTotalPorCategoria(categoriaTotal):C}");
                                    break;

                                case "3":
                                    // Lógica para listar despesas
                                    Console.Write("Digite o nome da categoria para listar as despesas: ");
                                    string categoriaListar = Console.ReadLine();

                                    var listaDespesas = despesas.ListarDespesas(categoriaListar);
                                    if (listaDespesas.Any())
                                    {
                                        Console.WriteLine($"Despesas na categoria {categoriaListar}:");
                                        foreach (var despesa in listaDespesas)
                                        {
                                            Console.WriteLine($"Descrição: {despesa.Descricao}, Valor: {despesa.Valor:C}");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Nenhuma despesa encontrada para a categoria {categoriaListar}.");
                                    }
                                    break;

                                case "4":
                                    // Lógica para sair
                                    Console.WriteLine("Saindo do gerenciamento de despesas.");
                                    continue;

                                default:
                                    Console.WriteLine("Opção inválida. Por favor, escolha uma opção de 1 a 4.");
                                    break;
                            }

                            Console.WriteLine(); // Adiciona uma linha em branco para melhor visualização
                        }
                        
                    case "2":

                        while (true)
                        {
                            Console.WriteLine("--- Menu de Gerenciamento de Receitas ---");
                            Console.WriteLine("1. Adicionar nova receita");
                            Console.WriteLine("2. Listar receitas de uma categoria");
                            Console.WriteLine("3. Listar categorias de receita");
                            Console.WriteLine("4. Sair");
                            Console.Write("Escolha uma opção: ");

                            string opcaoReceita = Console.ReadLine();

                            switch (opcaoReceita)
                            {
                                case "1":
                                    // Lógica para adicionar receita
                                    Console.Write("Digite a categoria da receita: ");
                                    string categoriaReceita = Console.ReadLine();

                                    Console.Write("Digite a descrição da receita: ");
                                    string descricaoReceita = Console.ReadLine();

                                    Console.Write("Digite o valor da receita: ");
                                    if (!double.TryParse(Console.ReadLine(), out double valorReceita))
                                    {
                                        Console.WriteLine("Valor inválido.");
                                        break;
                                    }

                                    receitas.AdicionarReceita(categoriaReceita, descricaoReceita, valorReceita);
                                    Console.WriteLine($"Receita adicionada com sucesso na categoria {categoriaReceita}!");
                                    break;

                                case "2":
                                    // Lógica para listar receitas
                                    Console.Write("Digite o nome da categoria para listar as receitas: ");
                                    string categoriaListar = Console.ReadLine();

                                    var listaReceitas = receitas.ListarReceitas(categoriaListar);
                                    if (listaReceitas.Any())
                                    {
                                        Console.WriteLine($"Receitas na categoria {categoriaListar}:");
                                        foreach (var receita in listaReceitas)
                                        {
                                            Console.WriteLine($"Descrição: {receita.Descricao}, Valor: {receita.Valor:C}");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine($"Nenhuma receita encontrada para a categoria {categoriaListar}.");
                                    }
                                    break;

                                case "3":
                                    // Lógica para listar categorias
                                    var categoriasReceita = new List<string>(receitas.ListarCategorias());
                                    if (categoriasReceita.Any())
                                    {
                                        Console.WriteLine("Categorias de receita existentes:");
                                        foreach (var categoria in categoriasReceita)
                                        {
                                            Console.WriteLine($"- {categoria}");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Nenhuma categoria de receita encontrada.");
                                    }
                                    break;

                                case "4":
                                    // Lógica para sair
                                    Console.WriteLine("Saindo do gerenciamento de receitas.");
                                    continue;

                                default:
                                    Console.WriteLine("Opção inválida. Por favor, escolha uma opção de 1 a 4.");
                                    break;
                            }

                            Console.WriteLine(); // Adiciona uma linha em branco para melhor visualização
                        }
                }

            }
        }
    }
}
