package com.app.zakat.model;

import java.time.LocalDateTime;
import java.time.chrono.HijrahDate;
import java.util.List;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.OneToMany;
import jakarta.persistence.Table;
import lombok.Data;

@Data
@Entity
@Table(name="users")
public class User {

	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	private int userId;
	
	private String name;
	
	private String totalZakatIn21K = "0";
	
	private String totalPaidZakatIn21K = "0";
	
	private String zakatDeadLine;
	
	@OneToMany(mappedBy = "user")
	private List<Revenue> revenues;  
}
