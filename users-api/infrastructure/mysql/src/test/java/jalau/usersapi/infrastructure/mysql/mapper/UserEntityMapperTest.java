package jalau.usersapi.infrastructure.mysql.mapper;

import jalau.usersapi.domain.entity.User;
import jalau.usersapi.infrastructure.mysql.entity.UserEntity;
import org.junit.jupiter.api.Test;

import java.time.Instant;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;
import static org.junit.jupiter.api.Assertions.assertNull;

public class UserEntityMapperTest {

    @Test
    public void shouldMapDomainUserToUserEntity() {
        Instant now = Instant.parse("2026-10-08T12:00:00Z");
        User user = new User("1", "Maycon", "mike", "hash123", now, now);

        UserEntity entity = UserEntityMapper.toEntity(user);

        assertNotNull(entity);
        assertEquals("1", entity.getId());
        assertEquals("Maycon", entity.getName());
        assertEquals("mike", entity.getLogin());
        assertEquals("hash123", entity.getPasswordHash());
        assertEquals(now, entity.getCreatedAt());
        assertEquals(now, entity.getUpdatedAt());
    }

    @Test
    public void shouldReturnNullWhenUserIsNull() {
        assertNull(UserEntityMapper.toEntity(null));
    }

    @Test
    public void shouldMapUserEntityToDomainUser() {
        Instant now = Instant.parse("2026-10-08T12:00:00Z");
        UserEntity entity = new UserEntity("1", "Maycon", "mike", "hash123", now, now);

        User user = UserEntityMapper.toDomain(entity);

        assertNotNull(user);
        assertEquals("1", user.getId());
        assertEquals("Maycon", user.getName());
        assertEquals("mike", user.getLogin());
        assertEquals("hash123", user.getPasswordHash());
        assertEquals(now, user.getCreatedAt());
        assertEquals(now, user.getUpdatedAt());
    }

    @Test
    public void shouldReturnNullWhenUserEntityIsNull() {
        assertNull(UserEntityMapper.toDomain(null));
    }
}
