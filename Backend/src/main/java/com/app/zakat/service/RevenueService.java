package com.app.zakat.service;

import java.time.LocalDate;
import java.time.chrono.HijrahDate;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.app.zakat.dto.RevenueDTO;
import com.app.zakat.model.Currency;
import com.app.zakat.model.Revenue;
import com.app.zakat.model.User;
import com.app.zakat.repo.CurrencyRepo;
import com.app.zakat.repo.RevenueRepo;
import com.app.zakat.repo.UserRepo;
import com.app.zakat.util.ZakatConstant;
import com.app.zakat.util.ZakatUtil;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.ObjectMapper;

import jakarta.transaction.Transactional;

@Transactional
@Service
public class RevenueService {

	@Autowired
	private RevenueRepo revenueRepo;
	
	@Autowired
	private UserRepo userRepo;
	
	@Autowired
	private CurrencyRepo currencyRepo;
	
	
	public void addRevenue(RevenueDTO revenueDTO) throws JsonMappingException, JsonProcessingException {
		
		Revenue revenue = new Revenue();
		User user = userRepo.findById(revenueDTO.getUserId()).get();
		Currency currency = currencyRepo.findById(revenueDTO.getCurrencyId()).get();
		String kerat21 = currencyRepo.get21K();
		
		//Calculate zakat in 21k
		double zakatIn21K = Double.parseDouble(currency.getRates()) / Double.parseDouble(kerat21) * 0.025 * revenueDTO.getValue();

		revenue.setUser(user);
		revenue.setCurrency(currency);
		revenue.setZakatIn21K(String.valueOf(zakatIn21K));
		revenue.setValue(String.valueOf(revenueDTO.getValue()));
		revenueRepo.save(revenue);
		
		//Add zakat of revenue to user total zakat
		double userZakat = Double.parseDouble(user.getTotalZakatIn21K()) + zakatIn21K;
		
		user.setTotalZakatIn21K(String.valueOf(userZakat));
		
		if(userZakat >= ZakatConstant.ZAKAT_THRESHOLD) {
		    if(user.getZakatDeadLine() == null) {
		    	LocalDate gregorianDate = LocalDate.now();
			    String hijriDate = ZakatUtil.usingHijrahChronology(gregorianDate);
			    user.setZakatDeadLine(hijriDate);
		    }
			for(Revenue revenue2: user.getRevenues()) {
				if(revenue2.getZakatRemaining().equals("0")) {
					revenue2.setZakatRemaining(String.valueOf(Double.parseDouble(revenue2.getValue()) * 0.025));
				
					revenueRepo.save(revenue2);
				}
			}
		}
	}
	
public void updateRevenue(RevenueDTO revenueDTO) throws JsonMappingException, JsonProcessingException {
		
		Revenue revenue = revenueRepo.findById(revenueDTO.getRevenueId()).get();
		User user = userRepo.findById(revenueDTO.getUserId()).get();
		Currency currency = currencyRepo.findById(revenueDTO.getCurrencyId()).get();
		String kerat21 = currencyRepo.get21K();
		double zakatIn21K = Double.parseDouble(currency.getRates()) / Double.parseDouble(kerat21) * 0.025 * revenueDTO.getValue();
		double userZakat = Double.parseDouble(user.getTotalZakatIn21K()) - Double.parseDouble(revenue.getZakatIn21K()) + zakatIn21K;
		
		revenue.setZakatIn21K(String.valueOf(zakatIn21K));
		revenue.setValue(String.valueOf(revenueDTO.getValue()));
		revenueRepo.save(revenue);
		
		if(userZakat >= ZakatConstant.ZAKAT_THRESHOLD) {
			if(user.getZakatDeadLine() == null) {
		    	LocalDate gregorianDate = LocalDate.now();
			    String hijriDate = ZakatUtil.usingHijrahChronology(gregorianDate);
			    user.setZakatDeadLine(hijriDate);
		    }
			for(Revenue revenue2: user.getRevenues()) {
				if(revenue2.getZakatRemaining().equals("0")) {
					revenue2.setZakatRemaining(String.valueOf(Double.parseDouble(revenue2.getValue()) * 0.025));
					revenueRepo.save(revenue2);
				}
				revenue.setZakatRemaining(String.valueOf(Double.parseDouble(revenue2.getValue()) * 0.025));
				revenueRepo.save(revenue);
			}
		}else if(Double.parseDouble(user.getTotalZakatIn21K()) > userZakat){
			user.setZakatDeadLine(null);
			for(Revenue revenue2: user.getRevenues()) {
					revenue2.setZakatRemaining("0");
					revenueRepo.save(revenue2);
				}
			}
//		else {
//			for(Revenue revenue2: user.getRevenues()) {
//				revenue2.setZakatRemaining(String.valueOf(valueIn21K));
//				revenueRepo.save(revenue2);
//			}
//		}
		
		user.setTotalZakatIn21K(String.valueOf(userZakat));
		userRepo.save(user);
	}
}
