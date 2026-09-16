package com.gerenciador.financas.dto;

import jakarta.validation.constraints.DecimalMin;
import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.NotNull;

import java.math.BigDecimal;

public record LimiteRequest(
        @NotBlank(message = "A categoria é obrigatória.") String categoria,
        @NotNull(message = "O valor limite é obrigatório.")
        @DecimalMin(value = "0.01", message = "O valor limite deve ser maior que zero.") BigDecimal valorLimite
) {
}
