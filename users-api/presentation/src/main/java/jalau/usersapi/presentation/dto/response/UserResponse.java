package jalau.usersapi.presentation.dto.response;

import java.time.Instant;

public class UserResponse {

    private String id;

    private String name;

    private String login;

    private Instant createdAt;

    private Instant updatedAt;

    public UserResponse() {
    }

    public UserResponse(
            String id,
            String name,
            String login,
            Instant createdAt,
            Instant updatedAt) {
        this.id = id;
        this.name = name;
        this.login = login;
        this.createdAt = createdAt;
        this.updatedAt = updatedAt;
    }

    public UserResponse(String id, String name, String login) {
        this(id, name, login, null, null);
    }

    public String getId() {
        return id;
    }

    public String getName() {
        return name;
    }

    public String getLogin() {
        return login;
    }

    public Instant getCreatedAt() {
        return createdAt;
    }

    public Instant getUpdatedAt() {
        return updatedAt;
    }
}
