package jalau.usersapi.presentation.controller;

import jalau.usersapi.domain.entity.User;
import jalau.usersapi.domain.service.IUserQueryService;
import jalau.usersapi.presentation.dto.response.UserResponse;
import jalau.usersapi.presentation.mapper.UserDtoMapper;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;
import java.util.UUID;

@RestController
@RequestMapping("/api/v1/users")
public class UserQueryController {

    private final IUserQueryService userQueryService;

    public UserQueryController(IUserQueryService userQueryService) {
        this.userQueryService = userQueryService;
    }

    @GetMapping
    public ResponseEntity<List<UserResponse>> getAllUsers() {
        List<UserResponse> users = userQueryService.getAllUsers().stream()
                .map(UserDtoMapper::toResponse)
                .toList();
        return ResponseEntity.ok(users);
    }

    @GetMapping("/{id}")
    public ResponseEntity<UserResponse> getUser(@PathVariable("id") UUID id) {
        User user = userQueryService.getUserById(id.toString());
        return ResponseEntity.ok(UserDtoMapper.toResponse(user));
    }
}
