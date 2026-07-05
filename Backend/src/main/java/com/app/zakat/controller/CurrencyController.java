package com.app.zakat.controller;

import java.util.List;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.app.zakat.service.CurrencyService;

@RestController
@RequestMapping("/currencies")
public class CurrencyController {

	@Autowired
	private CurrencyService currencyService;
	
	@PostMapping("/populate")
	public void populate() {
		currencyService.populateDB();
	}
	
	@GetMapping("")
	public ResponseEntity<List<Map<String, String>>> getCurrencies(){
		List<Map<String, String>> response = currencyService.getCurrencies();
		
		return new ResponseEntity<>(response,HttpStatus.OK);
	}
}
