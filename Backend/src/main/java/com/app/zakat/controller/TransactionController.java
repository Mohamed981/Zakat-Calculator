package com.app.zakat.controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.app.zakat.dto.TransactionDto;
import com.app.zakat.service.CurrencyService;
import com.app.zakat.service.TransactionService;

@RestController
@RequestMapping("/transactions")
public class TransactionController {

	@Autowired
	private TransactionService transactionService;
	
	@PostMapping("")
	public void addTransaction(@RequestBody TransactionDto transactionDto) {
		transactionService.addTransaction(transactionDto);
	}
	
}
