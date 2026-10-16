package jalau.usersapi.application.service;

import jalau.usersapi.domain.entity.User;
import jalau.usersapi.domain.exception.UserNotFoundException;
import jalau.usersapi.domain.repository.IUserRepository;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;

import java.util.Collections;
import java.util.List;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertFalse;
import static org.junit.jupiter.api.Assertions.assertNotNull;
import static org.junit.jupiter.api.Assertions.assertTrue;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

@ExtendWith(MockitoExtension.class)
class UserQueryServiceTest {

    @Mock
    private IUserRepository userRepository;

    private UserQueryService userQueryService;

    @BeforeEach
    void setUp() {
        userQueryService = new UserQueryService(userRepository);
    }

    @Test
    void shouldReturnEmptyAndNonNullListWhenNoUsersExist() {

        when(userRepository.findAll()).thenReturn(Collections.emptyList());

        List<User> result = userQueryService.getAllUsers();

        assertNotNull(result, "List should not be null");
        assertTrue(result.isEmpty(), "List should be empty");
        verify(userRepository).findAll();
    }

    @Test
    void shouldReturnNonNullInstancesWhenUsersListIsNotEmpty() {
        User user1 = new User("1", "Maycon", "mvyctor", "hash1");
        User user2 = new User("2", "Maria", "mmarquina", "hash2");
        when(userRepository.findAll()).thenReturn(List.of(user1, user2));

        List<User> result = userQueryService.getAllUsers();

        assertNotNull(result, "List should not be null");
        assertFalse(result.isEmpty(), "List should not be empty");
        assertEquals(2, result.size());
        for (User user : result) {
            assertNotNull(user, "Each user instance in the list should not be null");
        }
        verify(userRepository).findAll();
    }

    @Test
    void shouldReturnUserWhenUserIdExists() {
        User user = new User("123", "Maria", "maria", "hash123");

        when(userRepository.findById("123")).thenReturn(java.util.Optional.of(user));

        User result = userQueryService.getUserById("123");

        assertNotNull(result);
        assertEquals("123", result.getId());
        assertEquals("Maria", result.getName());
        assertEquals("maria", result.getLogin());

        verify(userRepository).findById("123");
    }

    @Test
    void shouldThrowUserNotFoundExceptionWhenUserIdDoesNotExist() {
        when(userRepository.findById("999")).thenReturn(java.util.Optional.empty());

        UserNotFoundException exception = org.junit.jupiter.api.Assertions.assertThrows(
                UserNotFoundException.class,
                () -> userQueryService.getUserById("999")
        );

        assertEquals("User not found with id: 999", exception.getMessage());

        verify(userRepository).findById("999");
    }
}

