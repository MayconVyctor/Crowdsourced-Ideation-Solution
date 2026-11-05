package jalau.usersapi.infrastructure.mongodb.document;

import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.assertEquals;

public class CounterDocumentTest {

    @Test
    public void shouldConstructAndAccessProperties() {
        CounterDocument counter = new CounterDocument("user_id", 100L);

        assertEquals("user_id", counter.getId());
        assertEquals(100L, counter.getSequenceValue());

        counter.setId("other_id");
        counter.setSequenceValue(200L);

        assertEquals("other_id", counter.getId());
        assertEquals(200L, counter.getSequenceValue());
    }

    @Test
    public void shouldConstructWithNoArgs() {
        CounterDocument counter = new CounterDocument();
        counter.setId("seq");
        counter.setSequenceValue(1L);

        assertEquals("seq", counter.getId());
        assertEquals(1L, counter.getSequenceValue());
    }
}
