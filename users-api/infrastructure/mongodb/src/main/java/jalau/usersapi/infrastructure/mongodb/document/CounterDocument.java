package jalau.usersapi.infrastructure.mongodb.document;

import org.springframework.data.annotation.Id;
import org.springframework.data.mongodb.core.mapping.Document;
import org.springframework.data.mongodb.core.mapping.Field;

/**
 * MongoDB document representing an ID sequence counter in the 'counters' collection.
 * Required by database schema specification.
 */
@Document(collection = "counters")
public class CounterDocument {

    @Id
    private String id;

    @Field("sequence_value")
    private Long sequenceValue;

    public CounterDocument() {
    }

    public CounterDocument(String id, Long sequenceValue) {
        this.id = id;
        this.sequenceValue = sequenceValue;
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public Long getSequenceValue() {
        return sequenceValue;
    }

    public void setSequenceValue(Long sequenceValue) {
        this.sequenceValue = sequenceValue;
    }
}
