package jalau.usersapi.presentation.mapper;

import jalau.usersapi.domain.entity.User;
import jalau.usersapi.presentation.dto.request.CreateUserRequest;
import jalau.usersapi.presentation.dto.response.UserResponse;

public final class UserDtoMapper {

    private UserDtoMapper() {
    }

    public static UserResponse toResponse(User user) {
        if (user == null) {
            return null;
        }
        return new UserResponse(user.getId(), user.getName(), user.getLogin(), 
                user.getCreatedAt(),
                user.getUpdatedAt());
    }

    public static User toDomain(CreateUserRequest request) {
        if (request == null) {
            return  null;
        }
        return new User(request.getName(), request.getLogin(), request.getPassword());
    }
}