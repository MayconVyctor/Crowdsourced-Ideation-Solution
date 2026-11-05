package jalau.usersapi;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

/**
 * Composition root for Users API. Scans all components under jalau.usersapi.
 * Technology-specific repository and entity scanning are configured within their
 * respective infrastructure modules, ensuring presentation remains technology-agnostic.
 */
@SpringBootApplication(scanBasePackages = "jalau.usersapi")
public class Application {

    public static void main(String[] args) {
        SpringApplication.run(Application.class, args);
    }
}
