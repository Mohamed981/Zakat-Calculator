package com.app.zakat.controller;

import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.app.zakat.dto.RevenueDTO;
import com.app.zakat.service.RevenueService;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.JsonMappingException;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;


@RestController
@RequestMapping("/revenues")
public class RevenueController {

	@Autowired
	private RevenueService revenueService;
	
	@PostMapping("")
	public ResponseEntity<String> createRevenue(@RequestBody RevenueDTO revenueDTO) throws JsonMappingException, JsonProcessingException {
		//TODO: process POST request
		revenueService.addRevenue(revenueDTO);
		return new ResponseEntity<>("ADDED",HttpStatus.OK);
	}
	
	@PutMapping("")
	public ResponseEntity<String> updateRevenue(@RequestBody RevenueDTO revenueDTO) throws JsonMappingException, JsonProcessingException {
		//TODO: process POST request
		revenueService.updateRevenue(revenueDTO);
		return new ResponseEntity<>("UPDATED",HttpStatus.OK);
	}
	
}
