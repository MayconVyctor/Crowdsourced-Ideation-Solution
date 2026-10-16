package jalau.usersapi.presentation.mapper;

import jalau.usersapi.domain.entity.User;
import jalau.usersapi.presentation.dto.request.CreateUserRequest;
import jalau.usersapi.presentation.dto.response.UserResponse;
import org.junit.jupiter.api.Test;

import java.time.Instant;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;
import static org.junit.jupiter.api.Assertions.assertNull;

public class UserDtoMapperTest {

    @Test
    public void shouldMapCreateUserRequestToDomainUser() {
        CreateUserRequest request = new CreateUserRequest();
        request.setName("Maycon");
        request.setLogin("mike");
        request.setPassword("password123");

        User user = UserDtoMapper.toDomain(request);

        assertNotNull(user);
        assertEquals("Maycon", user.getName());
        assertEquals("mike", user.getLogin());
        assertEquals("password123", user.getPasswordHash());
    }

    @Test
    public void shouldReturnNullWhenCreateUserRequestIsNull() {
        assertNull(UserDtoMapper.toDomain(null));
    }

    @Test
    public void shouldMapDomainUserToUserResponse() {
        Instant now = Instant.parse("2026-10-08T12:00:00Z");
        User user = new User("123", "Maycon", "mike", "password123", now, now);

        UserResponse response = UserDtoMapper.toResponse(user);

        assertNotNull(response);
        assertEquals("123", response.getId());
        assertEquals("Maycon", response.getName());
        assertEquals("mike", response.getLogin());
        assertEquals(now, response.getCreatedAt());
        assertEquals(now, response.getUpdatedAt());
    }

    @Test
    public void shouldReturnNullWhenUserIsNull() {
        assertNull(UserDtoMapper.toResponse(null));
    }
}