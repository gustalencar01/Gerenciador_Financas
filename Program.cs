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
                       System.Console.WriteLine("Você escolheu gerenciar despesas.");
                        var categorias = new List<string>(despesas.ListarCategorias());

                        for (int i = 0; i < categorias.Count; i++)
                            Console.WriteLine($"{i + 1}. {categorias[i]}");

                        Console.WriteLine("Escolha o número da categoria:");
                        if (!int.TryParse(Console.ReadLine(), out int escolha) || escolha < 1 || escolha > categorias.Count)
                        {
                            Console.WriteLine("Opção inválida.");
                            return;
                        }

                        string categoriaEscolhida = categorias[escolha - 1];

                        Console.WriteLine($"Você escolheu: {categoriaEscolhida}"); 
                        Console.WriteLine("Digite a descrição da despesa:");
                        string descricao = Console.ReadLine();

                        Console.WriteLine();
                        Console.WriteLine("Digite o valor da despesa:");
                        string v = Console.ReadLine();
                        double valor = double.Parse(v);
                        
                        despesas.AdicionarDespesa(categoriaEscolhida, descricao, valor);

                        Console.WriteLine($"Despesa adicionada com sucesso na categoria {categoriaEscolhida}!");
                        System.Console.WriteLine();
                        System.Console.WriteLine($"Esse é o total das desepesas da cetgoria {categoriaEscolhida}!");
                        Console.WriteLine($"Total em {categoriaEscolhida}: {despesas.ObterTotalPorCategoria(categoriaEscolhida):C}");

                        System.Console.WriteLine();
                        System.Console.WriteLine("Deseja listar as despesas dessa categoria? (s/n)");
                        string respostaListar = Console.ReadLine().ToLower();
                        if (respostaListar == "s")
                        {
                            Console.WriteLine($"Despesas na categoria {categoriaEscolhida}:");
                            foreach (var despesa in despesas.ListarDespesas(categoriaEscolhida))
                            {
                                Console.WriteLine($"Descrição: {despesa.Descricao}, Valor: {despesa.Valor:C}");
                            }
                        }
                        else
                        {
                            System.Console.WriteLine("Saindo do gerenciamento de despesas.");
                        }

                        break;
                    case "2":
                        Gerenciamento_Receitas receitas = new Gerenciamento_Receitas();
                        receitas.CadastrarReceitas();
                        System.Console.WriteLine();
                        System.Console.WriteLine("Deseja cadastrar outra receitas? (s/n)");
                        string respostareceitas = Console.ReadLine().ToLower();
                        if (respostareceitas != "s")
                        {
                            System.Console.WriteLine("Saindo do gerenciamento de despesas.");
                            break;
                        }
                        System.Console.WriteLine();
                        break;
                    default:
                        System.Console.WriteLine("Opção inválida. Tente novamente.");
                        break;
                }

            }
        }
    }
}
