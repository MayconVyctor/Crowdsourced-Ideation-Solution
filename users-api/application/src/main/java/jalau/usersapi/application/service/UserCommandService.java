package jalau.usersapi.application.service;


import jalau.usersapi.domain.entity.User;
import jalau.usersapi.domain.exception.DuplicateLoginException;
import jalau.usersapi.domain.exception.UserNotFoundException;
import jalau.usersapi.domain.repository.IUserRepository;
import jalau.usersapi.domain.service.IUserCommandService;
import java.util.UUID;

public class UserCommandService implements IUserCommandService {
    
    private final IUserRepository userRepository;

    public UserCommandService(IUserRepository userRepository) {
        this.userRepository = userRepository;
    } 

    @Override 
    public User createUser(User user) {
        if (userRepository.existsByLogin(user.getLogin())) {
            throw new DuplicateLoginException(user.getLogin());
        }
        String id = user.getId() == null ? UUID.randomUUID().toString() : user.getId();
        User userToSave = new User(id, user.getName(), user.getLogin(), user.getPasswordHash());
        return userRepository.save(userToSave);
    }

    @Override 
    public User updateUser(String id, String name, String login, String password) {
        User existingUser = userRepository.findById(id)
                .orElseThrow(() -> new UserNotFoundException(id));

        String nameToSave = (name != null && !name.isBlank()) ? name : existingUser.getName();
        String loginToSave = (login != null && !login.isBlank()) ? login : existingUser.getLogin();

        if (!existingUser.getLogin().equals(loginToSave) && userRepository.existsByLogin(loginToSave)) {
            throw new DuplicateLoginException(loginToSave);
        }

        String passwordToSave;
        if (password != null && !password.isBlank()) {
            passwordToSave = password; 
        } else {
            passwordToSave = existingUser.getPasswordHash(); 
        }

        User userToSave = new User(existingUser.getId(), nameToSave, loginToSave, passwordToSave);
        return userRepository.save(userToSave);
    }

    @Override
    public void deleteUser(String id) {
        if (!userRepository.existsById(id)) {
            throw new UserNotFoundException(id);
        }
        userRepository.deleteById(id);
    }
}
