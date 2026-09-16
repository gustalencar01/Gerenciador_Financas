package com.gerenciador.financas.model;

import com.fasterxml.jackson.annotation.JsonInclude;

@JsonInclude(JsonInclude.Include.NON_NULL)
public record ApiResponse(String mensagem, String statusLimite) {

    public static ApiResponse mensagem(String mensagem) {
        return new ApiResponse(mensagem, null);
    }
}
