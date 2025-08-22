using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financas
{
    public class Gerenciamento_Receitas
    {

        struct Receita
        {
            public string Nome;
            public double Valor;
        }
        // Dicionário para armazenar listas de receitas por categoria
        Dictionary<string, List<Receita>> categorias = new Dictionary<string, List<Receita>>()
        {
            { "Salário", new List<Receita>() },
            { "Investimentos", new List<Receita>() },
            { "Freelance", new List<Receita>() },
            { "Outras", new List<Receita>() }
        };
        public void CadastrarReceitas()
        {
            Console.WriteLine("Informe a categoria da sua receita:");
            Console.WriteLine("1. Salário");
            Console.WriteLine("2. Investimentos");
            Console.WriteLine("3. Freelance");
            Console.WriteLine("4. Outras");
            string opcao = Console.ReadLine();
            string categoriaEscolhida = "";
            switch (opcao)
            {
                case "1": categoriaEscolhida = "Salário"; break;
                case "2": categoriaEscolhida = "Investimentos"; break;
                case "3": categoriaEscolhida = "Freelance"; break;
                case "4": categoriaEscolhida = "Outras"; break;
                default:
                    Console.WriteLine("Opção inválida.");
                    return;
            }
            Receita novaReceita;
            novaReceita.Nome = categoriaEscolhida;
            Console.WriteLine($"Informe o valor recebido de {categoriaEscolhida}:");
            novaReceita.Valor =+ double.Parse(Console.ReadLine());
            categorias[categoriaEscolhida].Add(novaReceita);
            Console.WriteLine($"Receita de {novaReceita.Nome} cadastrada com sucesso!");
        }
    }
}
