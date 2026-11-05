package jalau.usersapi.infrastructure.mongodb.document;

import java.time.Instant;
import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.assertEquals;

public class UserDocumentTest {

    @Test
    public void shouldConstructWithAllFields() {
        Instant now = Instant.parse("2026-11-03T12:00:00Z");
        UserDocument doc = new UserDocument("id-1", "Maycon", "mike", "hash", now, now, 1);

        assertEquals("id-1", doc.getId());
        assertEquals("Maycon", doc.getName());
        assertEquals("mike", doc.getLogin());
        assertEquals("hash", doc.getPasswordHash());
        assertEquals(now, doc.getCreatedAt());
        assertEquals(now, doc.getUpdatedAt());
        assertEquals(1, doc.getSchemaVersion());
    }

    @Test
    public void shouldConstructWithDefaultSchemaVersion() {
        Instant now = Instant.parse("2026-11-03T12:00:00Z");
        UserDocument doc = new UserDocument("id-1", "Maycon", "mike", "hash", now, now);

        assertEquals(UserDocument.CURRENT_SCHEMA_VERSION, doc.getSchemaVersion());
    }

    @Test
    public void shouldSetAndGetProperties() {
        Instant now = Instant.parse("2026-11-03T12:00:00Z");
        UserDocument doc = new UserDocument();
        doc.setId("id-2");
        doc.setName("Ellen");
        doc.setLogin("ellen");
        doc.setPasswordHash("pass");
        doc.setCreatedAt(now);
        doc.setUpdatedAt(now);
        doc.setSchemaVersion(2);

        assertEquals("id-2", doc.getId());
        assertEquals("Ellen", doc.getName());
        assertEquals("ellen", doc.getLogin());
        assertEquals("pass", doc.getPasswordHash());
        assertEquals(now, doc.getCreatedAt());
        assertEquals(now, doc.getUpdatedAt());
        assertEquals(2, doc.getSchemaVersion());
    }
}
