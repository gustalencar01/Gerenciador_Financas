package com.gerenciador.financas.controller;

import com.gerenciador.financas.dto.DespesaRequest;
import com.gerenciador.financas.dto.DespesaResponse;
import com.gerenciador.financas.model.ApiResponse;
import com.gerenciador.financas.service.FinanceiroService;
import jakarta.validation.Valid;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;

@RestController
@RequestMapping("/api/Despesas")
public class DespesasController {

    private final FinanceiroService service;

    public DespesasController(FinanceiroService service) {
        this.service = service;
    }

    @GetMapping
    public List<DespesaResponse> listar() {
        return service.listarDespesas();
    }

    @PostMapping
    public ResponseEntity<ApiResponse> criar(@Valid @RequestBody DespesaRequest request) {
        return ResponseEntity.status(HttpStatus.CREATED).body(service.adicionarDespesa(request));
    }

    @PutMapping("/{id}")
    public ApiResponse atualizar(@PathVariable Integer id, @Valid @RequestBody DespesaRequest request) {
        return service.atualizarDespesa(id, request);
    }

    @DeleteMapping("/{id}")
    public ApiResponse excluir(@PathVariable Integer id) {
        return service.excluirDespesa(id);
    }
}
