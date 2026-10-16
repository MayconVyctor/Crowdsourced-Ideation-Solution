package jalau.usersapi.presentation.security;


import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import java.io.IOException;
import java.time.Instant;
import org.springframework.http.MediaType;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.security.web.access.AccessDeniedHandler;
import tools.jackson.databind.ObjectWriter;
import tools.jackson.databind.ObjectMapper;

public class SecurityAccessDeniedHandler implements AccessDeniedHandler {

    private final ObjectWriter objectWriter;

    public SecurityAccessDeniedHandler(ObjectMapper objectMapper) {
        this.objectWriter = objectMapper.writer();
    }

    @Override
    public void handle(
            HttpServletRequest request,
            HttpServletResponse response,
            AccessDeniedException accessDeniedException) throws IOException {

        SecurityErrorResponse body = new SecurityErrorResponse(
                Instant.now(),
                HttpServletResponse.SC_FORBIDDEN,
                "Forbidden",
                "Access is denied",
                request.getRequestURI()
        );

        response.setStatus(HttpServletResponse.SC_FORBIDDEN);
        response.setContentType(MediaType.APPLICATION_JSON_VALUE);
        objectWriter.writeValue(response.getOutputStream(), body);
    }
}
