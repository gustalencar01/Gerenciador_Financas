using Microsoft.AspNetCore.Mvc;
using SistemaFinancasApi.Data;
using System.Linq;


public class FinanceiroService : IFinanceiroService
{
    private readonly AppDbContext _context;

    public FinanceiroService(AppDbContext context)
    {
        _context = context;
    }

    public object AdicionarDespesaComValidacao(DespesaEntity novaDespesa)
    {
        // 1. Lógica de Verificação de Limite
        var limiteConfig = _context.Limites
            .FirstOrDefault(l => l.Categoria.ToLower() == novaDespesa.Categoria.ToLower());

        string alerta = "Dentro do limite.";

        if (limiteConfig != null)
        {
            var gastoAtual = _context.Despesas
                .Where(d => d.Categoria.ToLower() == novaDespesa.Categoria.ToLower() && d.Data.Month == DateTime.Now.Month)
                .Sum(d => d.Valor);

            // ATENÇÃO: Aqui usamos 'ValorLimite' (o nome certo da sua Entity)
            if (gastoAtual + novaDespesa.Valor > limiteConfig.ValorLimite)
            {
                alerta = $"?? ALERTA: Limite de {novaDespesa.Categoria} excedido!";
            }
        }

        // 2. Persistência no Banco (Movido para cá!)
        _context.Despesas.Add(novaDespesa);
        _context.SaveChanges();

        // 3. Retorno para o Controller
        return new
        {
            mensagem = "Despesa salva com sucesso!",
            statusLimite = alerta
        };
    }

    public string AtualizarDespesa(int id, DespesaEntity despesaAtualizada)
    {
        var despesaBanco = _context.Despesas.Find(id);

        if (despesaBanco == null)
        {
            return "Despesa não encontrada.";
        }

        // Validação de Limite antes de atualizar
        var alertaLimite = ValidarLimite(despesaAtualizada.Categoria, despesaAtualizada.Valor - despesaBanco.Valor);

        // Atualiza os campos
        despesaBanco.Descricao = despesaAtualizada.Descricao;
        despesaBanco.Valor = despesaAtualizada.Valor;
        despesaBanco.Categoria = despesaAtualizada.Categoria;
        despesaBanco.Data = despesaAtualizada.Data;
        despesaBanco.Pago = despesaAtualizada.Pago;

        _context.SaveChanges();

        return alertaLimite == "Dentro do limite."
            ? "Despesa atualizada com sucesso!"
            : $"Despesa atualizada, mas {alertaLimite}";
    }

    public string ExcluirDespesa(int id)
    {
        var despesa = _context.Despesas.Find(id);

        if (despesa == null)
        {
            // Retornamos null ou uma mensagem específica para o Controller saber que não achou
            return "Despesa não encontrada.";
        }

        _context.Despesas.Remove(despesa);
        _context.SaveChanges();

        return "Despesa removida com sucesso!";
    }

    public string SalvarOuAtualizarLimite(LimiteEntity novoLimite)
    {
        // 1. Validação simples de entrada
        if (string.IsNullOrEmpty(novoLimite.Categoria))
        {
            return "Erro: A categoria não pode ser vazia.";
        }

        // 2. Busca se já existe um limite para essa categoria
        var limiteExistente = _context.Limites
            .FirstOrDefault(l => l.Categoria.ToLower() == novoLimite.Categoria.ToLower());

        if (limiteExistente != null)
        {
            // 3. ATUALIZAÇÃO: Se existe, apenas altera o valor
            limiteExistente.ValorLimite = novoLimite.ValorLimite; // Corrigido para ValorLimite
            _context.SaveChanges();
            return $"Limite da categoria '{limiteExistente.Categoria}' atualizado com sucesso!";
        }

        // 4. ADIÇÃO: Se não existe, cria um novo registro
        _context.Limites.Add(novoLimite);
        _context.SaveChanges();

        return "Novo limite cadastrado com sucesso!";
    }

    public string ExcluirLimite(int id)
    {
        var limite = _context.Limites.Find(id);

        if (limite == null)
        {
            return "Limite não encontrado.";
        }

        _context.Limites.Remove(limite);
        _context.SaveChanges();

        return "Limite removido com sucesso!";
    }


    public string AdicionarReceita(ReceitaEntity novaReceita)
    {
        if (novaReceita.Valor <= 0)
        {
            return "Erro: O valor da receita deve ser maior que zero.";
        }
        if (string.IsNullOrWhiteSpace(novaReceita.Descricao))
        {
            return "Erro: A descrição da receita é obrigatória.";
        }
        if (novaReceita.Data == default)
        {
            novaReceita.Data = DateTime.Now;
        }

        try
        {
            _context.Receitas.Add(novaReceita);
            _context.SaveChanges();
            return "Receita salva com sucesso!";
        }
        catch (Exception ex)
        {
            return "Erro interno ao salvar no banco de dados.";
        }
    }

    public string AtualizarReceita(int id, ReceitaEntity receitaAtualizada)
    {
        var receitaBanco = _context.Receitas.Find(id);

        if (receitaBanco == null)
        {
            return "Receita não encontrada.";
        }
        if (receitaAtualizada.Valor <= 0)
        {
            return "Erro: O valor da receita deve ser maior que zero.";
        }
        receitaBanco.Descricao = receitaAtualizada.Descricao;
        receitaBanco.Valor = receitaAtualizada.Valor;
        receitaBanco.Categoria = receitaAtualizada.Categoria;
        receitaBanco.Data = receitaAtualizada.Data;

        try
        {
            _context.SaveChanges();
            return "Receita atualizada com sucesso!";
        }
        catch (Exception)
        {
            return "Erro interno ao atualizar a receita no banco.";
        }
    }

    public string ExcluirReceita(int id)
    {
        var receita = _context.Receitas.Find(id);

        if (receita == null)
        {
            return "Receita não encontrada.";
        }

        try
        {
            _context.Receitas.Remove(receita);
            _context.SaveChanges();
            return "Receita removida com sucesso!";
        }
        catch (Exception)
        {
            return "Erro ao tentar excluir a receita no banco de dados.";
        }
    }

}