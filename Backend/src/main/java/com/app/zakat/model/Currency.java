package com.app.zakat.model;

import java.util.List;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.OneToMany;
import jakarta.persistence.Table;
import lombok.Data;

@Entity
@Table(name = "currency")
@Data
public class Currency {

	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	private int currencyId;
	
	private String name;
	
	@Column(columnDefinition = "TEXT")
	private String rates;
	
	private String Code;
	
	@OneToMany(mappedBy = "currency")
	private List<Revenue> revenues;
	
	@OneToMany(mappedBy = "currency")
	private List<Transaction> transactions;
}
