package com.app.zakat.repo;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import com.app.zakat.model.Transaction;

@Repository
public interface TransactionRepo extends JpaRepository<Transaction, Integer>{

}
