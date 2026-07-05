package com.app.zakat.repo;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import com.app.zakat.model.Revenue;

@Repository
public interface RevenueRepo extends JpaRepository<Revenue, Integer>{

}
