package jalau.usersapi.presentation.dto.response;

import java.util.List;

public class ErrorResponse {
    
    private int status;

    private String error;

    private String message;

    private List<String> details;

    public ErrorResponse() {
    }

    public ErrorResponse(int status, String error, String message, List<String> details) {
        this.status = status;
        this.error = error;
        this.message = message;
        this.details = details == null ? List.of() : List.copyOf(details);
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

    public List<String> getDetails() {
        return details == null ? List.of() : List.copyOf(details);
    }
}
