package jalau.usersapi.infrastructure.mongodb.document;

import java.time.Instant;
import org.springframework.data.annotation.Id;
import org.springframework.data.mongodb.core.index.Indexed;
import org.springframework.data.mongodb.core.mapping.Document;
import org.springframework.data.mongodb.core.mapping.Field;

/**
 * MongoDB document representing a user in the 'users' collection.
 * The schema version and field naming are specific to this persistence technology.
 */
@Document(collection = "users")
public class UserDocument {

    public static final int CURRENT_SCHEMA_VERSION = 1;

    @Id
    private String id;

    @Field("name")
    private String name;

    @Indexed(unique = true)
    @Field("login")
    private String login;

    @Field("password")
    private String passwordHash;

    @Field("created_at")
    private Instant createdAt;

    @Field("updated_at")
    private Instant updatedAt;

    @Field("schema_version")
    private Integer schemaVersion = CURRENT_SCHEMA_VERSION;

    public UserDocument() {
    }

    public UserDocument(
            String id,
            String name,
            String login,
            String passwordHash,
            Instant createdAt,
            Instant updatedAt) {
        this.id = id;
        this.name = name;
        this.login = login;
        this.passwordHash = passwordHash;
        this.createdAt = createdAt;
        this.updatedAt = updatedAt;
        this.schemaVersion = CURRENT_SCHEMA_VERSION;
    }

    public UserDocument(
            String id,
            String name,
            String login,
            String passwordHash,
            Instant createdAt,
            Instant updatedAt,
            Integer schemaVersion) {
        this.id = id;
        this.name = name;
        this.login = login;
        this.passwordHash = passwordHash;
        this.createdAt = createdAt;
        this.updatedAt = updatedAt;
        this.schemaVersion = schemaVersion != null ? schemaVersion : CURRENT_SCHEMA_VERSION;
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getLogin() {
        return login;
    }

    public void setLogin(String login) {
        this.login = login;
    }

    public String getPasswordHash() {
        return passwordHash;
    }

    public void setPasswordHash(String passwordHash) {
        this.passwordHash = passwordHash;
    }

    public Instant getCreatedAt() {
        return createdAt;
    }

    public void setCreatedAt(Instant createdAt) {
        this.createdAt = createdAt;
    }

    public Instant getUpdatedAt() {
        return updatedAt;
    }

    public void setUpdatedAt(Instant updatedAt) {
        this.updatedAt = updatedAt;
    }

    public Integer getSchemaVersion() {
        return schemaVersion;
    }

    public void setSchemaVersion(Integer schemaVersion) {
        this.schemaVersion = schemaVersion;
    }
}
