package com.gerenciador.financas.repository;

import com.gerenciador.financas.entity.Receita;
import org.springframework.data.jpa.repository.JpaRepository;

public interface ReceitaRepository extends JpaRepository<Receita, Integer> {
}
