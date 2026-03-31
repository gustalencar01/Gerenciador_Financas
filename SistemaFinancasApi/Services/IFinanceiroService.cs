using Microsoft.AspNetCore.Mvc;
using SistemaFinancasApi.Data;
using System.Linq;

public interface IFinanceiroService
{
	// Este método retorna um objeto com a mensagem de sucesso e o alerta de limite
	object AdicionarDespesaComValidacao(DespesaEntity novaDespesa);

	string AtualizarDespesa(int id, DespesaEntity despesaAtualizada);

	string ExcluirDespesa(int id);

	string SalvarOuAtualizarLimite(LimiteEntity novoLimite);

	string ExcluirLimite(int id);

	string AdicionarReeita(ReceitaEntity novaReceita);

	string AtualizarReceita(int id, ReceitaEntity receitaAtualizada);

	string ExcluirReceita(int id);
}