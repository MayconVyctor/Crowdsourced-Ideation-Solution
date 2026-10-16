package jalau.usersapi.presentation.controller;

import jakarta.validation.Valid;
import jalau.usersapi.domain.entity.User;
import jalau.usersapi.domain.service.IUserCommandService;
import jalau.usersapi.presentation.dto.request.CreateUserRequest;
import jalau.usersapi.presentation.dto.request.UpdateUserRequest;
import jalau.usersapi.presentation.dto.response.UserResponse;
import jalau.usersapi.presentation.mapper.UserDtoMapper;
import java.net.URI;
import java.util.UUID;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController 
@RequestMapping("/api/v1/users")
public class UserCommandController {

    private final IUserCommandService userCommandService;
    
    public UserCommandController(IUserCommandService userCommandService) {
        this.userCommandService = userCommandService;
    }

    @PostMapping 
    public ResponseEntity<UserResponse> createUser(@Valid @RequestBody CreateUserRequest request) {
        User toCreate = UserDtoMapper.toDomain(request);
        User created = userCommandService.createUser(toCreate);
        UserResponse response = UserDtoMapper.toResponse(created);
        return ResponseEntity.created(URI.create("/api/v1/users/" +
        response.getId())).body(response);
    }

    @PutMapping("/{id}")
    public ResponseEntity<UserResponse> updateUser(
            @PathVariable("id") UUID id,
            @Valid @RequestBody UpdateUserRequest request) {

        User update = userCommandService.updateUser(
                id.toString(),
                request.getName(),
                request.getLogin(),
                request.getPassword()
        );
        UserResponse response = UserDtoMapper.toResponse(update);
        return ResponseEntity.ok(response);
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Void> deleteUser(@PathVariable("id") UUID id) {
        userCommandService.deleteUser(id.toString());
        return ResponseEntity.noContent().build();
    }
}
