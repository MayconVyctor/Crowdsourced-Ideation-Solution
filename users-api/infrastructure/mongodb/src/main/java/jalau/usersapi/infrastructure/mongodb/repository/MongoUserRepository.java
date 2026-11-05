package jalau.usersapi.infrastructure.mongodb.repository;

import jalau.usersapi.domain.entity.User;
import jalau.usersapi.domain.repository.IUserRepository;
import jalau.usersapi.infrastructure.mongodb.document.UserDocument;
import jalau.usersapi.infrastructure.mongodb.mapper.UserDocumentMapper;
import java.time.Instant;
import java.util.List;
import java.util.Optional;
import org.springframework.stereotype.Repository;

@Repository
public class MongoUserRepository implements IUserRepository {

    private final SpringDataMongoUserRepository mongoRepository;

    public MongoUserRepository(SpringDataMongoUserRepository mongoRepository) {
        this.mongoRepository = mongoRepository;
    }

    @Override
    public List<User> findAll() {
        return mongoRepository.findAll().stream()
                .map(UserDocumentMapper::toDomain)
                .toList();
    }

    @Override
    public User save(User user) {
        Instant now = Instant.now();
        Instant createdAt = user.getCreatedAt();
        Instant updatedAt = now;

        if (user.getId() != null) {
            Optional<UserDocument> existing = mongoRepository.findById(user.getId());
            if (existing.isPresent()) {
                createdAt = existing.get().getCreatedAt() != null ? existing.get().getCreatedAt() : now;
            } else if (createdAt == null) {
                createdAt = now;
            }
        } else if (createdAt == null) {
            createdAt = now;
        }

        UserDocument document = new UserDocument(
                user.getId(),
                user.getName(),
                user.getLogin(),
                user.getPasswordHash(),
                createdAt,
                updatedAt,
                UserDocument.CURRENT_SCHEMA_VERSION);

        UserDocument saved = mongoRepository.save(document);
        return UserDocumentMapper.toDomain(saved);
    }

    @Override
    public boolean existsByLogin(String login) {
        return mongoRepository.existsByLogin(login);
    }

    @Override
    public Optional<User> findById(String id) {
        return mongoRepository.findById(id)
                .map(UserDocumentMapper::toDomain);
    }

    @Override
    public boolean existsById(String id) {
        return mongoRepository.existsById(id);
    }

    @Override
    public void deleteById(String id) {
        mongoRepository.deleteById(id);
    }
}
