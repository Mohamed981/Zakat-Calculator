package com.app.zakat.repo;

import org.springframework.data.jpa.repository.JpaRepository;

import com.app.zakat.model.User;

public interface UserRepo extends JpaRepository<User, Integer>{

}
