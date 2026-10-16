package jalau.usersapi.infrastructure.mysql.mapper;

import jalau.usersapi.domain.entity.User;
import jalau.usersapi.infrastructure.mysql.entity.UserEntity;

public final class UserEntityMapper {
    private UserEntityMapper() {
    }

    public static UserEntity toEntity(User user) {
        if (user == null) {
            return null;
        }
        return new UserEntity(user.getId(), user.getName(), user.getLogin(), user.getPasswordHash(),
                user.getCreatedAt(),
                user.getUpdatedAt());
    }

    public static User toDomain(UserEntity userEntity) {
        if (userEntity == null) {
            return null;
        }
        return new User(userEntity.getId(), userEntity.getName(), userEntity.getLogin(), userEntity.getPasswordHash(),
                userEntity.getCreatedAt(),
                userEntity.getUpdatedAt());
    }
}