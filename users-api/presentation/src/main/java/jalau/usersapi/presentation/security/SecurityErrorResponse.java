package jalau.usersapi.presentation.security;

import java.time.Instant;

public class SecurityErrorResponse {

    private final Instant timestamp;

    private final int status;

    private final String error;

    private final String message;

    private final String path;

    public SecurityErrorResponse(
            Instant timestamp,
            int status,
            String error,
            String message,
            String path) {
        this.timestamp = timestamp;
        this.status = status;
        this.error = error;
        this.message = message;
        this.path = path;
    }

    public Instant getTimestamp() {
        return timestamp;
    }

    public int getStatus() {
        return status;
    }

    public String getError() {
        return error;
    }

    public String getMessage() {
        return message;
    }

    public String getPath() {
        return path;
    }
}
