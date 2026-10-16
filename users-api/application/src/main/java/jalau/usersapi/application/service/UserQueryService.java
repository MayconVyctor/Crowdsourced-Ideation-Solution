package jalau.usersapi.application.service;

import jalau.usersapi.domain.entity.User;
import jalau.usersapi.domain.exception.UserNotFoundException;
import jalau.usersapi.domain.repository.IUserRepository;
import jalau.usersapi.domain.service.IUserQueryService;

import java.util.List;

public class UserQueryService implements IUserQueryService {

    private final IUserRepository userRepository;
    
    public UserQueryService(IUserRepository userRepository) {
        this.userRepository = userRepository;
    }

    @Override
    public List<User> getAllUsers() {
        return userRepository.findAll();
    }

    @Override
    public User getUserById(String id) {
        return userRepository.findById(id)
                .orElseThrow(() -> new UserNotFoundException(id));

    }
}    