package jalau.usersapi.infrastructure.mysql.config;

import org.springframework.boot.persistence.autoconfigure.EntityScan;
import org.springframework.context.annotation.Configuration;
import org.springframework.data.jpa.repository.config.EnableJpaRepositories;

/**
 * Spring configuration for MySQL persistence layer.
 * Enables JPA repositories and entity scanning strictly within the MySQL module.
 */
@Configuration
@EnableJpaRepositories(basePackages = "jalau.usersapi.infrastructure.mysql.repository")
@EntityScan(basePackages = "jalau.usersapi.infrastructure.mysql.entity")
public class MySqlConfig {
}
