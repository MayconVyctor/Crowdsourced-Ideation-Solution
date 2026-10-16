package jalau.usersapi.domain.repository;

import jalau.usersapi.domain.entity.User;

import java.util.List;
import java.util.Optional;

public interface IUserRepository {

    List<User> findAll();

    User save(User user);

    boolean existsByLogin(String login);

    Optional<User> findById(String id);

    boolean existsById(String id);

    void deleteById(String id);
}