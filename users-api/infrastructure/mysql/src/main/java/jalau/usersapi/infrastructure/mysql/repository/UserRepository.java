package jalau.usersapi.infrastructure.mysql.repository;

import jalau.usersapi.domain.entity.User;
import jalau.usersapi.domain.repository.IUserRepository;
import jalau.usersapi.infrastructure.mysql.entity.UserEntity;
import jalau.usersapi.infrastructure.mysql.mapper.UserEntityMapper;
import org.springframework.stereotype.Repository;

import jakarta.persistence.EntityManager;
import jakarta.persistence.PersistenceContext;
import jakarta.transaction.Transactional;

import java.util.List;
import java.util.Optional;

@Repository
public class UserRepository implements IUserRepository {

    private final UserJpaRepository jpaRepository;

    @PersistenceContext
    private EntityManager entityManager;

    public UserRepository(UserJpaRepository jpaRepository) {
        this.jpaRepository = jpaRepository;
    }

    @Override
    public List<User> findAll() {
        return jpaRepository.findAll().stream()
                .map(UserEntityMapper::toDomain)
                .toList();
    }

    @Override
    @Transactional
    public User save(User user) {
        UserEntity saved = jpaRepository.saveAndFlush(UserEntityMapper.toEntity(user));
        entityManager.refresh(saved);
        return UserEntityMapper.toDomain(saved);
    }

    @Override
    public boolean existsByLogin(String login) {
        return jpaRepository.existsByLogin(login);
    }    
    
    @Override
    public Optional<User> findById(String id) {
        return jpaRepository.findById(id)
                .map(UserEntityMapper::toDomain);
    }

    @Override 
    public boolean existsById(String id) {
        return jpaRepository.existsById(id);
    }

    @Override 
    public void deleteById(String id) {
        jpaRepository.deleteById(id);
    }
}