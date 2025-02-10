package com.app.zakat.model;

import java.util.List;

import org.hibernate.annotations.ManyToAny;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.OneToMany;
import jakarta.persistence.Table;
import lombok.Data;

@Entity
@Table(name = "revenue")
@Data
public class Revenue {

	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	private int revenueId;
	
	private String value;
	
	private String zakatRemaining = "0";
	
	private String zakatPaid;
	
	private String zakatIn21K;
	
	@OneToMany(mappedBy = "revenue")
	private List<Transaction> transactions;
	
	@ManyToOne
	@JoinColumn(name = "currency_id")
	private Currency currency;
	
	@ManyToOne
	@JoinColumn(name = "user_id")
	private User user;
}
