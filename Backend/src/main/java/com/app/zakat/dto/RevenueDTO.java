package com.app.zakat.dto;

import lombok.Data;

@Data
public class RevenueDTO {

	private int revenueId;
	
	private int userId;
	
	private int currencyId;
	
	private double value;
	
	private float zakatRemaining;
	
	private float zakatPaid = 0;
}
