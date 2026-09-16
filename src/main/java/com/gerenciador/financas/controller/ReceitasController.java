package com.gerenciador.financas.controller;

import com.gerenciador.financas.dto.ReceitaRequest;
import com.gerenciador.financas.dto.ReceitaResponse;
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
@RequestMapping("/api/Receitas")
public class ReceitasController {

    private final FinanceiroService service;

    public ReceitasController(FinanceiroService service) {
        this.service = service;
    }

    @GetMapping
    public List<ReceitaResponse> listar() {
        return service.listarReceitas();
    }

    @PostMapping
    public ResponseEntity<ApiResponse> criar(@Valid @RequestBody ReceitaRequest request) {
        return ResponseEntity.status(HttpStatus.CREATED).body(service.adicionarReceita(request));
    }

    @PutMapping("/{id}")
    public ApiResponse atualizar(@PathVariable Integer id, @Valid @RequestBody ReceitaRequest request) {
        return service.atualizarReceita(id, request);
    }

    @DeleteMapping("/{id}")
    public ApiResponse excluir(@PathVariable Integer id) {
        return service.excluirReceita(id);
    }
}
