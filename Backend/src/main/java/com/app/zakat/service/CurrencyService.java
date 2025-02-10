package com.app.zakat.service;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.core.env.Environment;
import org.springframework.stereotype.Service;
import org.springframework.web.client.RestTemplate;

import com.app.zakat.dto.CurrencyDTO;
import com.app.zakat.model.Currency;
import com.app.zakat.repo.CurrencyRepo;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.ObjectMapper;

import jakarta.transaction.Transactional;


@Transactional
@Service
public class CurrencyService {

	@Autowired
	private CurrencyRepo currencyRepo;
	
	@Autowired
	private RestTemplate restTemplate;
	
	@Autowired
	private Environment env;
	
	private final Map<String,String> kerats = Map.of("24K", "24K Gold", "22K", "22K Gold", "21K", "21K Gold", "18K", "18K Gold", "14K", "14K Gold", "12K", "12K Gold", "10K", "10K Gold");
	
	public String populateRates(String code) {
		String url = env.getProperty("currency_url");
		String json = restTemplate.getForObject(url + code + ".json", String.class);
		
		return json;
	}
	
	public void populateDB() {
		String url = env.getProperty("currencies_url");
		String json = restTemplate.getForObject(url, String.class);
		Map<String,String> currencies = null;
		try {
			currencies = new ObjectMapper().readValue(json, Map.class);
			for(Map.Entry<String, String> currencyEntry:currencies.entrySet()) {
				Currency currency = new Currency();
				currency.setCode(currencyEntry.getKey());
				currency.setName(currencyEntry.getValue());
				json = populateRates(currency.getCode());
				currency.setRates(json);
				currencyRepo.save(currency);
			}
			for(String kerat:kerats.keySet()) {
				Currency currency = new Currency();
				currency.setCode(kerat);
				currency.setName(kerats.get(kerat));
				currencyRepo.save(currency);
			}
		} catch (JsonProcessingException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}	
	}
	
	public List<Map<String, String>> getCurrencies(){
		List<Map<String, String>> currencies = currencyRepo.getAllCurrencies();
		
		return currencies;
	}

	public String getCurrencyCodes(){
		String codes = currencyRepo.getAllCodes();
		
		return codes;
	}
}
