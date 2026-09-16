package com.gerenciador.financas.service;

import com.gerenciador.financas.dto.DespesaRequest;
import com.gerenciador.financas.dto.DespesaResponse;
import com.gerenciador.financas.dto.LimiteRequest;
import com.gerenciador.financas.dto.LimiteResponse;
import com.gerenciador.financas.dto.ReceitaRequest;
import com.gerenciador.financas.dto.ReceitaResponse;
import com.gerenciador.financas.entity.Despesa;
import com.gerenciador.financas.entity.Limite;
import com.gerenciador.financas.entity.Receita;
import com.gerenciador.financas.exception.ResourceNotFoundException;
import com.gerenciador.financas.model.ApiResponse;
import com.gerenciador.financas.repository.DespesaRepository;
import com.gerenciador.financas.repository.LimiteRepository;
import com.gerenciador.financas.repository.ReceitaRepository;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.YearMonth;
import java.util.List;

@Service
public class FinanceiroService {

    private final DespesaRepository despesaRepository;
    private final ReceitaRepository receitaRepository;
    private final LimiteRepository limiteRepository;

    public FinanceiroService(DespesaRepository despesaRepository,
                             ReceitaRepository receitaRepository,
                             LimiteRepository limiteRepository) {
        this.despesaRepository = despesaRepository;
        this.receitaRepository = receitaRepository;
        this.limiteRepository = limiteRepository;
    }

    @Transactional(readOnly = true)
    public List<DespesaResponse> listarDespesas() {
        return despesaRepository.findAll().stream().map(DespesaResponse::from).toList();
    }

    @Transactional
    public DespesaResponse buscarDespesa(Integer id) {
        return DespesaResponse.from(obterDespesa(id));
    }

    @Transactional
    public ApiResponse adicionarDespesa(DespesaRequest request) {
        Despesa despesa = new Despesa();
        aplicar(despesa, request);
        String statusLimite = verificarLimite(despesa.getCategoria(), despesa.getValor());
        despesaRepository.save(despesa);
        return new ApiResponse("Despesa salva com sucesso!", statusLimite);
    }

    @Transactional
    public ApiResponse atualizarDespesa(Integer id, DespesaRequest request) {
        Despesa despesa = obterDespesa(id);
        String categoriaAnterior = despesa.getCategoria();
        BigDecimal valorAnterior = despesa.getValor();
        BigDecimal total = totalDoMes(request.categoria()).subtract(
                categoriaAnterior.equalsIgnoreCase(request.categoria()) ? valorAnterior : BigDecimal.ZERO)
                .add(request.valor());
        String status = statusDoLimite(request.categoria(), total);
        aplicar(despesa, request);
        return new ApiResponse("Despesa atualizada com sucesso!", status);
    }

    @Transactional
    public ApiResponse excluirDespesa(Integer id) {
        despesaRepository.delete(obterDespesa(id));
        return ApiResponse.mensagem("Despesa removida com sucesso!");
    }

    @Transactional(readOnly = true)
    public List<ReceitaResponse> listarReceitas() {
        return receitaRepository.findAll().stream().map(ReceitaResponse::from).toList();
    }

    @Transactional
    public ApiResponse adicionarReceita(ReceitaRequest request) {
        Receita receita = new Receita();
        aplicar(receita, request);
        receitaRepository.save(receita);
        return ApiResponse.mensagem("Receita salva com sucesso!");
    }

    @Transactional
    public ApiResponse atualizarReceita(Integer id, ReceitaRequest request) {
        Receita receita = receitaRepository.findById(id)
                .orElseThrow(() -> new ResourceNotFoundException("Receita não encontrada."));
        aplicar(receita, request);
        return ApiResponse.mensagem("Receita atualizada com sucesso!");
    }

    @Transactional
    public ApiResponse excluirReceita(Integer id) {
        Receita receita = receitaRepository.findById(id)
                .orElseThrow(() -> new ResourceNotFoundException("Receita não encontrada."));
        receitaRepository.delete(receita);
        return ApiResponse.mensagem("Receita removida com sucesso!");
    }

    @Transactional(readOnly = true)
    public List<LimiteResponse> listarLimites() {
        return limiteRepository.findAll().stream().map(LimiteResponse::from).toList();
    }

    @Transactional
    public ApiResponse salvarLimite(LimiteRequest request) {
        Limite limite = limiteRepository.findByCategoriaIgnoreCase(request.categoria()).orElseGet(Limite::new);
        boolean novo = limite.getId() == null;
        limite.setCategoria(request.categoria().trim());
        limite.setValorLimite(request.valorLimite());
        limiteRepository.save(limite);
        String mensagem = novo
                ? "Novo limite cadastrado com sucesso!"
                : "Limite da categoria '" + limite.getCategoria() + "' atualizado com sucesso!";
        return ApiResponse.mensagem(mensagem);
    }

    @Transactional
    public ApiResponse atualizarLimite(Integer id, LimiteRequest request) {
        Limite limite = limiteRepository.findById(id)
                .orElseThrow(() -> new ResourceNotFoundException("Limite não encontrado."));
        limite.setCategoria(request.categoria().trim());
        limite.setValorLimite(request.valorLimite());
        return ApiResponse.mensagem("Limite atualizado com sucesso!");
    }

    @Transactional
    public ApiResponse excluirLimite(Integer id) {
        Limite limite = limiteRepository.findById(id)
                .orElseThrow(() -> new ResourceNotFoundException("Limite não encontrado."));
        limiteRepository.delete(limite);
        return ApiResponse.mensagem("Limite removido com sucesso!");
    }

    private Despesa obterDespesa(Integer id) {
        return despesaRepository.findById(id)
                .orElseThrow(() -> new ResourceNotFoundException("Despesa não encontrada."));
    }

    private void aplicar(Despesa despesa, DespesaRequest request) {
        despesa.setDescricao(request.descricao().trim());
        despesa.setValor(request.valor());
        despesa.setCategoria(request.categoria().trim());
        despesa.setData(request.data());
        despesa.setPago(request.pago());
    }

    private void aplicar(Receita receita, ReceitaRequest request) {
        receita.setDescricao(request.descricao().trim());
        receita.setValor(request.valor());
        receita.setCategoria(request.categoria().trim());
        receita.setData(request.data());
    }

    private String verificarLimite(String categoria, BigDecimal valorAdicional) {
        return statusDoLimite(categoria, totalDoMes(categoria).add(valorAdicional));
    }

    private BigDecimal totalDoMes(String categoria) {
        YearMonth mes = YearMonth.now();
        return despesaRepository.sumByCategoriaAndPeriod(categoria, mes.atDay(1), mes.atEndOfMonth());
    }

    private String statusDoLimite(String categoria, BigDecimal total) {
        return limiteRepository.findByCategoriaIgnoreCase(categoria)
                .filter(limite -> total.compareTo(limite.getValorLimite()) > 0)
                .map(limite -> "ALERTA: Limite de " + categoria + " excedido!")
                .orElse("Dentro do limite.");
    }
}
