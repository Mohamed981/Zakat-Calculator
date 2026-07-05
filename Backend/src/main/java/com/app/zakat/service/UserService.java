package com.app.zakat.service;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.app.zakat.dto.UserDTO;
import com.app.zakat.model.User;
import com.app.zakat.repo.UserRepo;

import jakarta.transaction.Transactional;

@Service
@Transactional
public class UserService {

	@Autowired
	private UserRepo userRepo;
	
	public String createUser(UserDTO userDTO) {
		
		User user = new User();
		user.setName(userDTO.getName());
		userRepo.save(user);
		
		return "CREATED";
	}
}
