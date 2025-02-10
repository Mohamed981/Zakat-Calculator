package com.app.zakat.service;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.app.zakat.dto.TransactionDto;
import com.app.zakat.model.Currency;
import com.app.zakat.model.Revenue;
import com.app.zakat.model.Transaction;
import com.app.zakat.model.User;
import com.app.zakat.repo.CurrencyRepo;
import com.app.zakat.repo.RevenueRepo;
import com.app.zakat.repo.TransactionRepo;
import com.app.zakat.repo.UserRepo;

@Service
public class TransactionService {

	@Autowired
	private TransactionRepo transactionRepo;
	
	@Autowired
	private RevenueRepo revenueRepo;
	
	@Autowired
	private CurrencyRepo currencyRepo;
	
	@Autowired
	private UserRepo userRepo;
	
	public void addTransaction(TransactionDto transactionDto) {
		Transaction transaction = new Transaction();
		
		Revenue revenue = revenueRepo.findById(transactionDto.getRevenueId()).get();
		Currency currency = currencyRepo.findById(transactionDto.getCurrencyId()).get();
		User user = userRepo.findById(transactionDto.getUserId()).get();
		
		double paid = Double.parseDouble(currency.getRates()) * transactionDto.getValue() / Double.parseDouble(revenue.getCurrency().getRates());
		
		transaction.setCurrency(currency);
		transaction.setRevenue(revenue);
		transaction.setUser(user);
		transaction.setValue(String.valueOf(transactionDto.getValue()));
		transactionRepo.save(transaction);
		
		revenue.setZakatRemaining(String.valueOf(Double.parseDouble(revenue.getZakatRemaining()) - paid));
		
		if(revenue.getZakatPaid() == null)
			revenue.setZakatPaid(String.valueOf(paid));
		else
			revenue.setZakatPaid(String.valueOf(Double.parseDouble(revenue.getZakatPaid()) + paid));
		revenueRepo.save(revenue);
		
		double kerat21K = Double.parseDouble(currencyRepo.get21K());
		
		user.setTotalPaidZakatIn21K(String.valueOf(Double.parseDouble(user.getTotalPaidZakatIn21K()) + paid / kerat21K * Double.parseDouble(revenue.getCurrency().getRates())));
		userRepo.save(user);
	}
}
