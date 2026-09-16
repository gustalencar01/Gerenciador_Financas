package com.gerenciador.financas.dto;

import jakarta.validation.constraints.DecimalMin;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;

import java.math.BigDecimal;
import java.time.LocalDate;

public record ReceitaRequest(
        @NotBlank(message = "A descrição é obrigatória.") String descricao,
        @NotNull(message = "O valor é obrigatório.")
        @DecimalMin(value = "0.01", message = "O valor deve ser maior que zero.") BigDecimal valor,
        @NotBlank(message = "A categoria é obrigatória.") String categoria,
        @NotNull(message = "A data é obrigatória.") LocalDate data
) {
}
