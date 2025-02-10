package com.app.zakat.controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.app.zakat.dto.UserDTO;
import com.app.zakat.service.UserService;

@RestController
@RequestMapping("/users")
public class UserController {
	
	@Autowired
	private UserService userService;
	
	@PostMapping("")
	public ResponseEntity<String> createUser(@RequestBody UserDTO userDTO){
		
		String result = userService.createUser(userDTO);
		
		return new ResponseEntity<>(result,HttpStatus.OK);
	}
	
}
