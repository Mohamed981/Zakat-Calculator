package com.app.zakat.repo;

import java.util.List;
import java.util.Map;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.stereotype.Repository;

import com.app.zakat.model.Currency;

@Repository
public interface CurrencyRepo extends JpaRepository<Currency, Integer>{

	@Query(value = "SELECT currency_id, code, name from currency where code != 'xau'",nativeQuery =true)
	List<Map<String,String>> getAllCurrencies();
	
	@Query(value="SELECT code from currency where code != 'xau'",nativeQuery = true)
	String getAllCodes();
	
	@Query(value="SELECT rates from currency where code = '21K'",nativeQuery = true)
	String get21K();
}
