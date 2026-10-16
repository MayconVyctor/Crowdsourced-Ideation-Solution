package jalau.cis.utils;

public class StringUtils {
    public static boolean isNullOrEmpty(String value) {
        return (value == null || value.isEmpty() || value.trim().isEmpty());
    }
}
