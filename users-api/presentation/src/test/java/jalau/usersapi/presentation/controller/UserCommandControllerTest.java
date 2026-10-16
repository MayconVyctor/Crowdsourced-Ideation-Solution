package jalau.usersapi.presentation.controller;

import jalau.usersapi.domain.entity.User;
import jalau.usersapi.domain.service.IUserCommandService;
import jalau.usersapi.presentation.advice.GlobalExceptionHandler;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.ArgumentCaptor;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;
import org.springframework.http.MediaType;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.test.web.servlet.setup.MockMvcBuilders;

import java.time.Instant;

import static org.junit.jupiter.api.Assertions.assertNull;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.delete;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.post;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.put;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.header;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.jsonPath;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

@ExtendWith(MockitoExtension.class)
public class UserCommandControllerTest {

    private MockMvc mockMvc;

    @Mock
    private IUserCommandService userCommandService;

    @InjectMocks
    private UserCommandController userCommandController;

    @BeforeEach
    public void setUp() {
        mockMvc = MockMvcBuilders.standaloneSetup(userCommandController)
                .setControllerAdvice(new GlobalExceptionHandler())
                .build();
    }

    @Test
    public void shouldReturnCreatedWhenRequestBodyIsValid() throws Exception {
        String validJson = "{\"name\":\"Maycon\",\"login\":\"mike\",\"password\":\"password123\"}";

        Instant now = Instant.parse("2026-10-08T12:00:00Z");
        User savedUser = new User("uuid-1", "Maycon", "mike", "password123", now, now);
        when(userCommandService.createUser(any(User.class))).thenReturn(savedUser);

        mockMvc.perform(post("/api/v1/users")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(validJson))
                .andExpect(status().isCreated())
                .andExpect(header().string("Location", "/api/v1/users/uuid-1"))
                .andExpect(jsonPath("$.id").value("uuid-1"))
                .andExpect(jsonPath("$.name").value("Maycon"))
                .andExpect(jsonPath("$.login").value("mike"))
                .andExpect(jsonPath("$.createdAt").value("2026-10-08T12:00:00Z"))
                .andExpect(jsonPath("$.updatedAt").value("2026-10-08T12:00:00Z"))
                .andExpect(jsonPath("$.password").doesNotExist());
    }

    @Test
    public void shouldIgnoreClientProvidedTimestampsInRequestBody() throws Exception {
        String jsonWithClientTimestamps = "{"
                + "\"name\":\"Maycon\","
                + "\"login\":\"mike\","
                + "\"password\":\"password123\","
                + "\"createdAt\":\"2020-01-01T00:00:00Z\","
                + "\"updatedAt\":\"2020-01-01T00:00:00Z\""
                + "}";

        Instant serverTimestamp = Instant.parse("2026-10-08T12:00:00Z");
        User savedUser = new User("uuid-1", "Maycon", "mike", "password123", serverTimestamp, serverTimestamp);
        when(userCommandService.createUser(any(User.class))).thenReturn(savedUser);

        mockMvc.perform(post("/api/v1/users")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(jsonWithClientTimestamps))
                .andExpect(status().isCreated())
                .andExpect(jsonPath("$.createdAt").value("2026-10-08T12:00:00Z"))
                .andExpect(jsonPath("$.updatedAt").value("2026-10-08T12:00:00Z"));

        ArgumentCaptor<User> captor = ArgumentCaptor.forClass(User.class);
        verify(userCommandService).createUser(captor.capture());
        assertNull(captor.getValue().getCreatedAt());
        assertNull(captor.getValue().getUpdatedAt());
    }

    @Test
    public void shouldReturnBadRequestWhenRequestBodyIsInvalid() throws Exception {
        String invalidJson = "{\"name\":\"\",\"login\":\"m\",\"password\":\"short\"}";

        mockMvc.perform(post("/api/v1/users")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(invalidJson))
                .andExpect(status().isBadRequest())
                .andExpect(jsonPath("$.status").value(400))
                .andExpect(jsonPath("$.error").value("Bad Request"))
                .andExpect(jsonPath("$.message").value("Validation failed"))
                .andExpect(jsonPath("$.details").isArray());
    }

    @Test
    public void shouldReturnOkWhenUpdatingUserWithValidRequestBody() throws Exception {
        String validId = "123e4567-e89b-12d3-a456-426614174000";
        String validJson = "{\"name\":\"Maycon Atualizado\",\"login\":\"mike_novo\",\"password\":\"novaSenha123\"}";
        User updatedUser = new User(validId, "Maycon Atualizado", "mike_novo", "novaSenha123");
        when(userCommandService.updateUser(validId, "Maycon Atualizado", "mike_novo", "novaSenha123"))
                .thenReturn(updatedUser);
        mockMvc.perform(put("/api/v1/users/" + validId)
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(validJson))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.id").value(validId))
                .andExpect(jsonPath("$.name").value("Maycon Atualizado"))
                .andExpect(jsonPath("$.login").value("mike_novo"))
                .andExpect(jsonPath("$.password").doesNotExist());
    }

    @Test
    public void shouldReturnOkWhenPartialUpdatingUser() throws Exception {
        String validId = "123e4567-e89b-12d3-a456-426614174000";
        String partialJson = "{\"name\":\"Novo Nome\"}";
        User updatedUser = new User(validId, "Novo Nome", "mike", "senha123");

        when(userCommandService.updateUser(validId, "Novo Nome", null, null))
                .thenReturn(updatedUser);

        mockMvc.perform(put("/api/v1/users/" + validId)
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(partialJson))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.id").value(validId))
                .andExpect(jsonPath("$.name").value("Novo Nome"));
    }

    @Test
    public void shouldReturnBadRequestWhenUpdatingUserWithInvalidRequestBody() throws Exception {
        String validId = "123e4567-e89b-12d3-a456-426614174000";
        String invalidJson = "{\"login\":\"m\"}";

        mockMvc.perform(put("/api/v1/users/" + validId)
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(invalidJson))
                .andExpect(status().isBadRequest())
                .andExpect(jsonPath("$.status").value(400))
                .andExpect(jsonPath("$.error").value("Bad Request"))
                .andExpect(jsonPath("$.message").value("Validation failed"))
                .andExpect(jsonPath("$.details").isArray());
    }

    @Test
    public void shouldReturnBadRequestWhenIdIsNotAValidUuid() throws Exception {
        String invalidId = "abc";
        String validJson = "{\"name\":\"Maycon\"}";

        mockMvc.perform(put("/api/v1/users/" + invalidId)
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(validJson))
                .andExpect(status().isBadRequest())
                .andExpect(jsonPath("$.status").value(400))
                .andExpect(jsonPath("$.error").value("Bad Request"))
                .andExpect(jsonPath("$.message").value("Invalid parameter format"));
    }

    @Test 
    public void shouldReturnNoContentWhenUserIsDeletedSuccessfully() throws Exception {
        String validUuid = "123e4567-e89b-12d3-a456-426614174000";
        mockMvc.perform(delete("/api/v1/users/" + validUuid))
                .andExpect(status().isNoContent());
        verify(userCommandService).deleteUser(validUuid);
    }

    @Test
    public void shouldReturnBadRequestWhenDeleteIdIsNotAValidUuid() throws Exception {
        mockMvc.perform(delete("/api/v1/users/invalid-uuid"))
                .andExpect(status().isBadRequest())
                .andExpect(jsonPath("$.status").value(400))
                .andExpect(jsonPath("$.error").value("Bad Request"))
                .andExpect(jsonPath("$.message").value("Invalid parameter format"));
    }
}