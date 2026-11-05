package jalau.usersapi.infrastructure.mongodb.mapper;

import jalau.usersapi.domain.entity.User;
import jalau.usersapi.infrastructure.mongodb.document.UserDocument;
import java.time.Instant;
import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;
import static org.junit.jupiter.api.Assertions.assertNull;

public class UserDocumentMapperTest {

    @Test
    public void shouldMapDomainUserToUserDocument() {
        Instant now = Instant.parse("2026-11-03T12:00:00Z");
        User user = new User("1", "Maycon", "mike", "hash123", now, now);

        UserDocument doc = UserDocumentMapper.toDocument(user);

        assertNotNull(doc);
        assertEquals("1", doc.getId());
        assertEquals("Maycon", doc.getName());
        assertEquals("mike", doc.getLogin());
        assertEquals("hash123", doc.getPasswordHash());
        assertEquals(now, doc.getCreatedAt());
        assertEquals(now, doc.getUpdatedAt());
        assertEquals(UserDocument.CURRENT_SCHEMA_VERSION, doc.getSchemaVersion());
    }

    @Test
    public void shouldReturnNullWhenUserIsNull() {
        assertNull(UserDocumentMapper.toDocument(null));
    }

    @Test
    public void shouldMapUserDocumentToDomainUser() {
        Instant now = Instant.parse("2026-11-03T12:00:00Z");
        UserDocument doc = new UserDocument("1", "Maycon", "mike", "hash123", now, now, 1);

        User user = UserDocumentMapper.toDomain(doc);

        assertNotNull(user);
        assertEquals("1", user.getId());
        assertEquals("Maycon", user.getName());
        assertEquals("mike", user.getLogin());
        assertEquals("hash123", user.getPasswordHash());
        assertEquals(now, user.getCreatedAt());
        assertEquals(now, user.getUpdatedAt());
    }

    @Test
    public void shouldReturnNullWhenUserDocumentIsNull() {
        assertNull(UserDocumentMapper.toDomain(null));
    }
}
