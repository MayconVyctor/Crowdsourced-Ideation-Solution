package jalau.usersapi.infrastructure.mongodb.config;

import org.springframework.context.annotation.Configuration;
import org.springframework.data.mongodb.repository.config.EnableMongoRepositories;

/**
 * Spring configuration for MongoDB persistence layer.
 * Enables Mongo repositories scanning strictly within the MongoDB module.
 */
@Configuration
@EnableMongoRepositories(basePackages = "jalau.usersapi.infrastructure.mongodb.repository")
public class MongoDbConfig {
}
