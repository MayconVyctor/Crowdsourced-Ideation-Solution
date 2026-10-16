package jalau.usersapi.domain.service;

import jalau.usersapi.domain.entity.User;

public interface IUserCommandService {

    User createUser(User user);

    User updateUser(String id, String name, String login, String password);

    void deleteUser(String id);
} 
