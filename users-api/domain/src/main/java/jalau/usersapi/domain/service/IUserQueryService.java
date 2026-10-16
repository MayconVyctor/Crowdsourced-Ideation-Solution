package jalau.usersapi.domain.service;

import jalau.usersapi.domain.entity.User;

import java.util.List;

public interface IUserQueryService {

    List<User> getAllUsers();

    User getUserById(String id);
}