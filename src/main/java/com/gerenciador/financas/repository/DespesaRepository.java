package com.gerenciador.financas.repository;

import com.gerenciador.financas.entity.Despesa;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import java.math.BigDecimal;
import java.time.LocalDate;

public interface DespesaRepository extends JpaRepository<Despesa, Integer> {

    @Query("""
            select coalesce(sum(d.valor), 0)
            from Despesa d
            where lower(d.categoria) = lower(:categoria)
              and d.data between :inicio and :fim
            """)
    BigDecimal sumByCategoriaAndPeriod(@Param("categoria") String categoria,
                                       @Param("inicio") LocalDate inicio,
                                       @Param("fim") LocalDate fim);
}
