package jalau.usersapi.presentation.advice;

import jalau.usersapi.domain.exception.DuplicateLoginException;
import jalau.usersapi.domain.exception.UserNotFoundException;
import jalau.usersapi.presentation.dto.response.ErrorResponse;
import java.util.List;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.BindingResult;
import org.springframework.validation.FieldError;
import org.springframework.web.bind.MethodArgumentNotValidException;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;
import static org.junit.jupiter.api.Assertions.assertTrue;
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.when;

public class GlobalExceptionHandlerTest {

    private GlobalExceptionHandler handler;

    @BeforeEach 
    public void setUp() {
        handler = new GlobalExceptionHandler();
    }

    @Test 
    public void shouldHandlerUserNotFoundException() {
        UserNotFoundException ex = new UserNotFoundException("123");

        ResponseEntity<ErrorResponse> response = handler.handleNotFound(ex);

        assertNotNull(response);
        assertEquals(HttpStatus.NOT_FOUND, response.getStatusCode());
        assertNotNull(response.getBody());
        assertEquals(404, response.getBody().getStatus());
        assertEquals("Not Found", response.getBody().getError());
        assertEquals("User not found with id: 123", response.getBody().getMessage());

        assertTrue(response.getBody().getDetails().isEmpty());
    }

    @Test
    public void shouldHandleDuplicateLoginException() {
        DuplicateLoginException ex = new DuplicateLoginException("mike");
        ResponseEntity<ErrorResponse> response = handler.handleDuplicate(ex);
        assertNotNull(response);
        assertEquals(HttpStatus.CONFLICT, response.getStatusCode());
        assertNotNull(response.getBody());
        assertEquals(409, response.getBody().getStatus());
        assertEquals("Conflict", response.getBody().getError());
        assertEquals("A user with login 'mike' already exists", response.getBody().getMessage());
        assertTrue(response.getBody().getDetails().isEmpty());
    }

        @Test
    public void shouldHandleGenericException() {
        Exception ex = new RuntimeException("Database timeout");

        ResponseEntity<ErrorResponse> response = handler.handleGeneric(ex);

        assertNotNull(response);
        assertEquals(HttpStatus.INTERNAL_SERVER_ERROR, response.getStatusCode());
        assertNotNull(response.getBody());
        assertEquals(500, response.getBody().getStatus());
        assertEquals("Internal Server Error", response.getBody().getError());
        assertEquals("An unexpected error occurred", response.getBody().getMessage());
        assertTrue(response.getBody().getDetails().isEmpty());
    }

    @Test
    public void shouldHandleMethodArgumentNotValidException() {
        MethodArgumentNotValidException ex = mock(MethodArgumentNotValidException.class);
        BindingResult bindingResult = mock(BindingResult.class);
        FieldError fieldError = new FieldError("user", "login", "must not be blank");

        when(ex.getBindingResult()).thenReturn(bindingResult);
        when(bindingResult.getFieldErrors()).thenReturn(List.of(fieldError));

        ResponseEntity<ErrorResponse> response = handler.handleValidation(ex);
        
        assertNotNull(response);
        assertEquals(HttpStatus.BAD_REQUEST, response.getStatusCode());
        assertNotNull(response.getBody());
        assertEquals(400, response.getBody().getStatus());
        assertEquals("Bad Request", response.getBody().getError());
        assertEquals("Validation failed", response.getBody().getMessage());
        assertEquals(1, response.getBody().getDetails().size());
        assertEquals("login: must not be blank", response.getBody().getDetails().get(0));
    }
}
