package jalau.usersapi.application.service;

import jalau.usersapi.domain.entity.User;
import jalau.usersapi.domain.exception.DuplicateLoginException;
import jalau.usersapi.domain.exception.UserNotFoundException;
import jalau.usersapi.domain.repository.IUserRepository;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;
import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.never;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

import java.util.Optional;

@ExtendWith(MockitoExtension.class)
public class UserCommandServiceTest {

    @Mock
    private IUserRepository userRepository;

    private UserCommandService userCommandService;

    @BeforeEach
    public void setUp() {
        userCommandService = new UserCommandService(userRepository);
    }

    @Test
    public void shouldCreateUserSuccessfully() {
        User userToCreate = new User("Maycon", "mike", "password123");
        User savedUser = new User("uuid-123", "Maycon", "mike", "password123");

        when(userRepository.existsByLogin("mike")).thenReturn(false);
        when(userRepository.save(any(User.class))).thenReturn(savedUser);

        User result = userCommandService.createUser(userToCreate);

        assertNotNull(result);
        assertEquals("uuid-123", result.getId());
        assertEquals("Maycon", result.getName());
        assertEquals("mike", result.getLogin());
        verify(userRepository).existsByLogin("mike");
        verify(userRepository).save(any(User.class));
    }

    @Test
    public void shouldThrowDuplicateLoginExceptionWhenLoginAlreadyExists() {
        User userToCreate = new User("Maycon", "mike", "password123");

        when(userRepository.existsByLogin("mike")).thenReturn(true);

        DuplicateLoginException exception = assertThrows(
                DuplicateLoginException.class,
                () -> userCommandService.createUser(userToCreate)
        );

        assertEquals("A user with login 'mike' already exists", exception.getMessage());
        verify(userRepository).existsByLogin("mike");
        verify(userRepository, never()).save(any(User.class));
    }

    @Test 
    public void shouldUpdateUserSucessfuly() {
        User existingUser = new User("uuid-1", "Maycon Antigo", "mike", "senha123");

        when(userRepository.findById("uuid-1")).thenReturn(Optional.of(existingUser));
        when(userRepository.existsByLogin("mike_novo")).thenReturn(false);
        when(userRepository.save(any(User.class))).thenAnswer(invocation -> invocation.getArgument(0));

        User result = userCommandService.updateUser("uuid-1", "Maycon Novo", "mike_novo", "novaSenha123");

        assertEquals("Maycon Novo", result.getName());
        assertEquals("mike_novo", result.getLogin());
        verify(userRepository).save(any(User.class));
    }

    @Test 
    public void shouldThrowNotFoundExceptionWhenUserDoesNotExists() {
        when(userRepository.findById("id-invalido")).thenReturn(Optional.empty());

        UserNotFoundException exception = assertThrows(
                UserNotFoundException.class,
                () -> userCommandService.updateUser("id-invalido", "Nome", "login", "senha")
        );
        verify(userRepository, never()).save(any(User.class));
    }

    @Test
    public void shouldThrowDuplicateLoginExceptionWhenLoginAlreadyExistsInUpdate() {
        User existingUser = new User("uuid-1", "Mike Antigo", "mike", "senha123");

        when(userRepository.findById("uuid-1")).thenReturn(Optional.of(existingUser));
        when(userRepository.existsByLogin("login_ocupado")).thenReturn(true);

        DuplicateLoginException exception = assertThrows(
                DuplicateLoginException.class,
                () -> userCommandService.updateUser("uuid-1", "Mike Novo", "login_ocupado", "senha123")
        );

        assertEquals("A user with login 'login_ocupado' already exists", exception.getMessage());
        verify(userRepository).existsByLogin("login_ocupado");
        verify(userRepository, never()).save(any(User.class));
    }

    @Test
    public void shouldUpdateOnlyNameWhenOtherFieldsAreNull() {
        User existingUser = new User("uuid-1", "Maycon Antigo", "mike", "senha123");

        when(userRepository.findById("uuid-1")).thenReturn(Optional.of(existingUser));
        when(userRepository.save(any(User.class))).thenAnswer(invocation -> invocation.getArgument(0));

        User result = userCommandService.updateUser("uuid-1", "Novo Nome", null, null);

        assertEquals("Novo Nome", result.getName());
        assertEquals("mike", result.getLogin());
        assertEquals("senha123", result.getPasswordHash());
        verify(userRepository).save(any(User.class));
    }

    @Test
    public void shouldUpdateOnlyLoginWhenOtherFieldsAreNull() {
        User existingUser = new User("uuid-1", "Maycon Antigo", "mike", "senha123");

        when(userRepository.findById("uuid-1")).thenReturn(Optional.of(existingUser));
        when(userRepository.existsByLogin("mike_novo")).thenReturn(false);
        when(userRepository.save(any(User.class))).thenAnswer(invocation -> invocation.getArgument(0));

        User result = userCommandService.updateUser("uuid-1", null, "mike_novo", null);

        assertEquals("Maycon Antigo", result.getName());
        assertEquals("mike_novo", result.getLogin());
        assertEquals("senha123", result.getPasswordHash());
        verify(userRepository).save(any(User.class));
    }

    @Test
    public void shouldDeleteUserSuccessfullyWhenUserExists() {        
        when(userRepository.existsById("uuid-123")).thenReturn(true);
        userCommandService.deleteUser("uuid-123");
        
        verify(userRepository).existsById("uuid-123");
        verify(userRepository).deleteById("uuid-123");
    }

    @Test 
    public void shouldThrowUserNotFoundExceptionWhenUserDoesNotExist() {
        when(userRepository.existsById("uuid-123")).thenReturn(false);

        assertThrows(UserNotFoundException.class, 
            () -> userCommandService.deleteUser("uuid-123"));
        
        verify(userRepository).existsById("uuid-123");
        verify(userRepository, never()).deleteById(any());
    }
}