package jalau.usersapi.presentation.config;

import jalau.usersapi.application.service.UserCommandService;
import jalau.usersapi.application.service.UserQueryService;
import jalau.usersapi.domain.service.IUserCommandService;
import jalau.usersapi.domain.service.IUserQueryService;
import jalau.usersapi.infrastructure.mysql.repository.UserRepository;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

/**
 * Core implementation is intentionally free of Spring annotations.
 * Presentation, as the composition root, is responsible for
 * instantiating application services as beans and wiring them
 * with infrastructure implementations.
 */
@Configuration
public class ServiceConfig {

    @Bean
    public IUserQueryService userQueryService(UserRepository userRepository) {
        return new UserQueryService(userRepository);
    }

    @Bean
    public IUserCommandService userCommandService(UserRepository userRepository) {
        return new UserCommandService(userRepository);
    }
}