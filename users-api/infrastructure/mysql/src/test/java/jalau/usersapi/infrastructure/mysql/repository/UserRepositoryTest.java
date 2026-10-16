package jalau.usersapi.infrastructure.mysql.repository;

import jakarta.persistence.EntityManager;
import jalau.usersapi.domain.entity.User;
import jalau.usersapi.infrastructure.mysql.entity.UserEntity;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;
import org.springframework.test.util.ReflectionTestUtils;

import java.time.Instant;
import java.util.List;
import java.util.Optional;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;
import static org.junit.jupiter.api.Assertions.assertTrue;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.doAnswer;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

@ExtendWith(MockitoExtension.class)
public class UserRepositoryTest {

    @Mock
    private UserJpaRepository jpaRepository;

    @Mock
    private EntityManager entityManager;

    @InjectMocks
    private UserRepository userRepository;

    @Test
    public void shouldSaveAndRefreshEntityToCaptureDatabaseGeneratedTimestamps() {
        ReflectionTestUtils.setField(userRepository, "entityManager", entityManager);

        Instant now = Instant.parse("2026-10-08T12:00:00Z");
        User userToSave = new User("uuid-1", "Maycon", "mike", "hash123");
        UserEntity savedEntity = new UserEntity("uuid-1", "Maycon", "mike", "hash123", null, null);

        when(jpaRepository.saveAndFlush(any(UserEntity.class))).thenReturn(savedEntity);
        doAnswer(invocation -> {
            UserEntity entity = invocation.getArgument(0);
            ReflectionTestUtils.setField(entity, "createdAt", now);
            ReflectionTestUtils.setField(entity, "updatedAt", now);
            return null;
        }).when(entityManager).refresh(savedEntity);

        User result = userRepository.save(userToSave);

        assertNotNull(result);
        assertEquals("uuid-1", result.getId());
        assertEquals(now, result.getCreatedAt());
        assertEquals(now, result.getUpdatedAt());
        verify(jpaRepository).saveAndFlush(any(UserEntity.class));
        verify(entityManager).refresh(savedEntity);
    }

    @Test
    public void shouldFindAllUsers() {
        Instant now = Instant.parse("2026-10-08T12:00:00Z");
        UserEntity entity = new UserEntity("1", "Maycon", "mike", "hash", now, now);
        when(jpaRepository.findAll()).thenReturn(List.of(entity));

        List<User> users = userRepository.findAll();

        assertEquals(1, users.size());
        assertEquals("1", users.get(0).getId());
        assertEquals(now, users.get(0).getCreatedAt());
    }

    @Test
    public void shouldCheckIfLoginExists() {
        when(jpaRepository.existsByLogin("mike")).thenReturn(true);
        assertTrue(userRepository.existsByLogin("mike"));
        verify(jpaRepository).existsByLogin("mike");
    }

    @Test
    public void shouldFindById() {
        Instant now = Instant.parse("2026-10-08T12:00:00Z");
        UserEntity entity = new UserEntity("1", "Maycon", "mike", "hash", now, now);
        when(jpaRepository.findById("1")).thenReturn(Optional.of(entity));

        Optional<User> user = userRepository.findById("1");

        assertTrue(user.isPresent());
        assertEquals("1", user.get().getId());
        assertEquals(now, user.get().getCreatedAt());
    }
}

