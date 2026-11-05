package jalau.usersapi.infrastructure.mongodb.repository;

import jalau.usersapi.infrastructure.mongodb.document.UserDocument;
import java.util.Optional;
import org.springframework.data.mongodb.repository.MongoRepository;

public interface SpringDataMongoUserRepository extends MongoRepository<UserDocument, String> {

    Optional<UserDocument> findByLogin(String login);

    boolean existsByLogin(String login);
}
