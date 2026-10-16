package jalau.usersapi.presentation.security;

import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import java.io.IOException;
import java.time.Instant;
import org.springframework.http.MediaType;
import org.springframework.security.core.AuthenticationException;
import org.springframework.security.web.AuthenticationEntryPoint;
import tools.jackson.databind.ObjectMapper;
import tools.jackson.databind.ObjectWriter;

public class SecurityAuthenticationEntryPoint implements AuthenticationEntryPoint {

    private final ObjectWriter objectWriter;

    public SecurityAuthenticationEntryPoint(ObjectMapper objectMapper) {
        this.objectWriter = objectMapper.writer();
    }

    @Override
    public void commence(
            HttpServletRequest request,
            HttpServletResponse response,
            AuthenticationException authException) throws IOException {

        SecurityErrorResponse body = new SecurityErrorResponse(
                Instant.now(),
                HttpServletResponse.SC_UNAUTHORIZED,
                "Unauthorized",
                "Authentication is required",
                request.getRequestURI()
        );

        response.setStatus(HttpServletResponse.SC_UNAUTHORIZED);
        response.setContentType(MediaType.APPLICATION_JSON_VALUE);
        objectWriter.writeValue(response.getOutputStream(), body);
    }
}
