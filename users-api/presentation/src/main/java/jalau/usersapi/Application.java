package jalau.usersapi;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.persistence.autoconfigure.EntityScan;
import org.springframework.data.jpa.repository.config.EnableJpaRepositories;

/**
 * Composition root. Spring component scanning + JPA repository/entity scanning are pointed
 * explicitly at the infrastructure-mysql package, since that module has no idea it is being
 * run inside a Spring Boot app - Presentation is what turns it into one.
 */
@SpringBootApplication(scanBasePackages = "jalau.usersapi")
@EnableJpaRepositories(basePackages = "jalau.usersapi.infrastructure.mysql.repository")
@EntityScan(basePackages = "jalau.usersapi.infrastructure.mysql.entity")
public class Application {

    public static void main(String[] args) {
        SpringApplication.run(Application.class, args);
    }
}
