package com.app.zakat.dto;

import lombok.Data;

@Data
public class TransactionDto {

	private int userId;
	
	private int revenueId;
	
	private int currencyId;
	
	private double value;

}
