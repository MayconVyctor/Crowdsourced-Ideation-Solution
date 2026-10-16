package jalau.usersapi.presentation.dto.request;

import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.Size;

public class CreateUserRequest {
    
    @NotBlank(message = "name must not be blank")
    @Size(max = 255, message = "name must be at most 255 characters")
    private String name;

    @NotBlank(message = "login must not be blank")
    @Size(min = 3, max = 100, message = "login must be between 3 and 100 characters")
    private String login;

    @NotBlank(message = "password must not be blank")
    @Size(min = 8, max = 128, message = "password must be at least 8 characters")
    private String password;

    public CreateUserRequest() {
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

    public String getPassword() {
        return password;
    }
    
    public void setPassword(String password) {
        this.password = password;
    }
}
