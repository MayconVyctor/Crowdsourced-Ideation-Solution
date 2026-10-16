package jalau.usersapi.domain.exception;

public class DuplicateLoginException extends RuntimeException {
    
    public DuplicateLoginException(String login) {
        super("A user with login '" + login + "' already exists");
    }
}