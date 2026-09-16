package com.gerenciador.financas.dto;

import com.gerenciador.financas.entity.Limite;

import java.math.BigDecimal;

public record LimiteResponse(Integer id, String categoria, BigDecimal valorLimite) {

    public static LimiteResponse from(Limite limite) {
        return new LimiteResponse(limite.getId(), limite.getCategoria(), limite.getValorLimite());
    }
}
