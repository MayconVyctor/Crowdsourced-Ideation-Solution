package jalau.usersapi.infrastructure.mongodb.mapper;

import jalau.usersapi.domain.entity.User;
import jalau.usersapi.infrastructure.mongodb.document.UserDocument;

public final class UserDocumentMapper {

    private UserDocumentMapper() {
    }

    public static UserDocument toDocument(User user) {
        if (user == null) {
            return null;
        }
        return new UserDocument(
                user.getId(),
                user.getName(),
                user.getLogin(),
                user.getPasswordHash(),
                user.getCreatedAt(),
                user.getUpdatedAt(),
                UserDocument.CURRENT_SCHEMA_VERSION);
    }

    public static User toDomain(UserDocument document) {
        if (document == null) {
            return null;
        }
        return new User(
                document.getId(),
                document.getName(),
                document.getLogin(),
                document.getPasswordHash(),
                document.getCreatedAt(),
                document.getUpdatedAt());
    }
}
