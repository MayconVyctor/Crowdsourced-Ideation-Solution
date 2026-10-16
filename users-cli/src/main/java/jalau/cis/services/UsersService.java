package jalau.cis.services;

import jalau.cis.models.User;

import java.util.List;

public interface UsersService {
    int deleteUser(String id) throws Exception;
    List<User> getUsers()  throws Exception;
    void createUser(User user)  throws Exception;
    void updateUser(User user)  throws Exception;
}
