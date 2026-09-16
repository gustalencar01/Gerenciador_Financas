package com.gerenciador.financas.dto;

import com.gerenciador.financas.entity.Receita;

import java.math.BigDecimal;
import java.time.LocalDate;

public record ReceitaResponse(Integer id, String descricao, BigDecimal valor, String categoria,
                              LocalDate data) {

    public static ReceitaResponse from(Receita receita) {
        return new ReceitaResponse(receita.getId(), receita.getDescricao(), receita.getValor(),
                receita.getCategoria(), receita.getData());
    }
}
