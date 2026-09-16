package com.gerenciador.financas.controller;

import com.gerenciador.financas.dto.LimiteRequest;
import com.gerenciador.financas.dto.LimiteResponse;
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
@RequestMapping("/api/Limites")
public class LimitesController {

    private final FinanceiroService service;

    public LimitesController(FinanceiroService service) {
        this.service = service;
    }

    @GetMapping
    public List<LimiteResponse> listar() {
        return service.listarLimites();
    }

    @PostMapping
    public ResponseEntity<ApiResponse> criarOuAtualizar(@Valid @RequestBody LimiteRequest request) {
        return ResponseEntity.status(HttpStatus.OK).body(service.salvarLimite(request));
    }

    @PutMapping("/{id}")
    public ApiResponse atualizar(@PathVariable Integer id, @Valid @RequestBody LimiteRequest request) {
        return service.atualizarLimite(id, request);
    }

    @DeleteMapping("/{id}")
    public ApiResponse excluir(@PathVariable Integer id) {
        return service.excluirLimite(id);
    }
}
