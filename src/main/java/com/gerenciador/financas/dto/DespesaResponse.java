package com.gerenciador.financas.dto;

import com.gerenciador.financas.entity.Despesa;

import java.math.BigDecimal;
import java.time.LocalDate;

public record DespesaResponse(Integer id, String descricao, BigDecimal valor, String categoria,
                              LocalDate data, Boolean pago) {

    public static DespesaResponse from(Despesa despesa) {
        return new DespesaResponse(despesa.getId(), despesa.getDescricao(), despesa.getValor(),
                despesa.getCategoria(), despesa.getData(), despesa.getPago());
    }
}
