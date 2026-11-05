package jalau.usersapi.infrastructure.mongodb.repository;

import jalau.usersapi.domain.entity.User;
import jalau.usersapi.infrastructure.mongodb.document.UserDocument;
import java.time.Instant;
import java.util.List;
import java.util.Optional;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.ArgumentCaptor;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertFalse;
import static org.junit.jupiter.api.Assertions.assertNotNull;
import static org.junit.jupiter.api.Assertions.assertTrue;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

@ExtendWith(MockitoExtension.class)
public class MongoUserRepositoryTest {

    @Mock
    private SpringDataMongoUserRepository mongoRepository;

    @InjectMocks
    private MongoUserRepository userRepository;

    @Test
    public void shouldFindAllUsers() {
        Instant now = Instant.parse("2026-11-03T12:00:00Z");
        UserDocument doc = new UserDocument("id-1", "Maycon", "mike", "hash", now, now);
        when(mongoRepository.findAll()).thenReturn(List.of(doc));

        List<User> users = userRepository.findAll();

        assertEquals(1, users.size());
        assertEquals("id-1", users.get(0).getId());
        assertEquals("Maycon", users.get(0).getName());
        assertEquals(now, users.get(0).getCreatedAt());
    }

    @Test
    public void shouldSaveNewUserWithGeneratedTimestamps() {
        User userToSave = new User("id-1", "Maycon", "mike", "hash");
        when(mongoRepository.findById("id-1")).thenReturn(Optional.empty());
        when(mongoRepository.save(any(UserDocument.class))).thenAnswer(invocation -> invocation.getArgument(0));

        User result = userRepository.save(userToSave);

        assertNotNull(result);
        assertEquals("id-1", result.getId());
        assertNotNull(result.getCreatedAt());
        assertNotNull(result.getUpdatedAt());

        ArgumentCaptor<UserDocument> captor = ArgumentCaptor.forClass(UserDocument.class);
        verify(mongoRepository).save(captor.capture());
        assertEquals("id-1", captor.getValue().getId());
        assertEquals(UserDocument.CURRENT_SCHEMA_VERSION, captor.getValue().getSchemaVersion());
    }

    @Test
    public void shouldSaveExistingUserPreservingCreatedAt() {
        Instant past = Instant.parse("2026-10-01T10:00:00Z");
        UserDocument existingDoc = new UserDocument("id-1", "Maycon", "mike", "hash", past, past);
        when(mongoRepository.findById("id-1")).thenReturn(Optional.of(existingDoc));
        when(mongoRepository.save(any(UserDocument.class))).thenAnswer(invocation -> invocation.getArgument(0));

        User userToUpdate = new User("id-1", "Maycon Updated", "mike", "hash");
        User result = userRepository.save(userToUpdate);

        assertNotNull(result);
        assertEquals("id-1", result.getId());
        assertEquals(past, result.getCreatedAt());
        assertNotNull(result.getUpdatedAt());
    }

    @Test
    public void shouldCheckIfLoginExists() {
        when(mongoRepository.existsByLogin("mike")).thenReturn(true);

        assertTrue(userRepository.existsByLogin("mike"));
        verify(mongoRepository).existsByLogin("mike");
    }

    @Test
    public void shouldFindByIdWhenPresent() {
        Instant now = Instant.parse("2026-11-03T12:00:00Z");
        UserDocument doc = new UserDocument("id-1", "Maycon", "mike", "hash", now, now);
        when(mongoRepository.findById("id-1")).thenReturn(Optional.of(doc));

        Optional<User> user = userRepository.findById("id-1");

        assertTrue(user.isPresent());
        assertEquals("id-1", user.get().getId());
        assertEquals(now, user.get().getCreatedAt());
    }

    @Test
    public void shouldFindByIdWhenEmpty() {
        when(mongoRepository.findById("non-existent")).thenReturn(Optional.empty());

        Optional<User> user = userRepository.findById("non-existent");

        assertFalse(user.isPresent());
    }

    @Test
    public void shouldCheckIfExistsById() {
        when(mongoRepository.existsById("id-1")).thenReturn(true);

        assertTrue(userRepository.existsById("id-1"));
        verify(mongoRepository).existsById("id-1");
    }

    @Test
    public void shouldDeleteById() {
        userRepository.deleteById("id-1");

        verify(mongoRepository).deleteById("id-1");
    }
}
