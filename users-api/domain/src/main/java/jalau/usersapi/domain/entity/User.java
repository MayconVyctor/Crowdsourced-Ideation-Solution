package jalau.usersapi.domain.entity;

import java.time.Instant;

public class User {

    private String id;

    private String name;

    private String login;

    private String passwordHash;

    private Instant createdAt;

    private Instant updatedAt;

    public User() {
    }

    public User(
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
    }

    public User(String id, String name, String login, String passwordHash) {
        this(id, name, login, passwordHash, null, null);
    }

    public User(String name, String login, String passwordHash) {
        this(null, name, login, passwordHash, null, null);
    }

    public String getId() {
        return id;
    }

    public String getName() {
        return name;
    }

    public String setName(String name) {
        this.name = name;
        return name;
    }

    public String getLogin() {
        return login;
    }

    public String setLogin(String login) {
        this.login = login;
        return login;
    }

    public String getPasswordHash() {
        return passwordHash;
    }

    public Instant getCreatedAt() {
        return createdAt;
    }

    public Instant getUpdatedAt() {
        return updatedAt;
    }
}
