package com.gerenciador.financas.repository;

import com.gerenciador.financas.entity.Limite;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.Optional;

public interface LimiteRepository extends JpaRepository<Limite, Integer> {

    Optional<Limite> findByCategoriaIgnoreCase(String categoria);
}
