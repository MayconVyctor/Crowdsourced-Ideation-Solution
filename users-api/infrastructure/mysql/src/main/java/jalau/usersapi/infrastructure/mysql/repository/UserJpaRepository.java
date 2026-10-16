package jalau.usersapi.infrastructure.mysql.repository;

import jalau.usersapi.infrastructure.mysql.entity.UserEntity;

import java.util.Optional;

import org.springframework.data.jpa.repository.JpaRepository;

public interface UserJpaRepository extends JpaRepository<UserEntity, String> {

    Optional<UserEntity> findByLogin(String login);

    boolean existsByLogin(String login);
}