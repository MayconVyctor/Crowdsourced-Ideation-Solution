package jalau.usersapi.presentation.config;

import java.util.Collection;
import java.util.List;
import java.util.Map;

import tools.jackson.databind.ObjectMapper;
import jalau.usersapi.presentation.security.SecurityAccessDeniedHandler;
import jalau.usersapi.presentation.security.SecurityAuthenticationEntryPoint;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.security.oauth2.server.resource.authentication.JwtAuthenticationConverter;
import org.springframework.security.web.SecurityFilterChain;

@Configuration
@EnableWebSecurity
public class SecurityConfig {

    @Bean
    public SecurityFilterChain securityFilterChain(HttpSecurity http,
                                                   ObjectMapper objectMapper) throws Exception {
        http
                .csrf(csrf -> csrf.disable())
                .authorizeHttpRequests(auth -> auth
                        .requestMatchers("/health").permitAll()
                        .requestMatchers("/api/v1/users/**").hasRole("user")
                        .anyRequest().denyAll()
                )
                .exceptionHandling(exception -> exception
                        .authenticationEntryPoint(new SecurityAuthenticationEntryPoint(objectMapper))
                        .accessDeniedHandler(new SecurityAccessDeniedHandler(objectMapper))
                )
                .oauth2ResourceServer(oauth2 -> oauth2
                        .authenticationEntryPoint(new SecurityAuthenticationEntryPoint(objectMapper))
                        .jwt(jwt -> jwt.jwtAuthenticationConverter(jwtAuthenticationConverter()))
                );

        return http.build();
    }

    @Bean
    public JwtAuthenticationConverter jwtAuthenticationConverter() {
        JwtAuthenticationConverter converter = new JwtAuthenticationConverter();

        converter.setJwtGrantedAuthoritiesConverter(jwt -> {
            Map<String, Object> realmAccess = jwt.getClaim("realm_access");

            if (realmAccess == null) {
                return List.of();
            }

            Object roles = realmAccess.get("roles");

            if (!(roles instanceof Collection<?> roleCollection)) {
                return List.of();
            }

            return roleCollection.stream()
                    .filter(String.class::isInstance)
                    .map(String.class::cast)
                    .map(role -> (org.springframework.security.core.GrantedAuthority)
                            new SimpleGrantedAuthority("ROLE_" + role))
                    .toList();
        });

        return converter;
    }
}