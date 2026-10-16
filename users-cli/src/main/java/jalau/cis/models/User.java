package jalau.cis.models;

import jalau.cis.utils.StringUtils;

public class User {
    private String id;
    private String name;
    private String login;

    private String password;

    public User(String id, String name, String login, String password) {
        this.id = id;
        this.name = name;
        this.login = login;
        this.password = password;
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getLogin() {
        return login;
    }

    public void setLogin(String login) {
        this.login = login;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public String getPassword() {
        return password;
    }

    public User cloneFrom(User refUser) {
        User clonedUser = new User(id, name, login, password);
        if (StringUtils.isNullOrEmpty(clonedUser.getId())) {
            clonedUser.setId(refUser.getId());
        }

        if (StringUtils.isNullOrEmpty(clonedUser.getLogin())) {
            clonedUser.setLogin(refUser.getLogin());
        }

        if (StringUtils.isNullOrEmpty(clonedUser.getName())) {
            clonedUser.setName(refUser.getName());
        }

        if (StringUtils.isNullOrEmpty(clonedUser.getPassword())) {
            clonedUser.setPassword(refUser.getPassword());
        }
        return clonedUser;
    }
    @Override
    public String toString() {
        return String.format("Id: [%s], Name: [%s], Login: [%s]", id, name, login);
    }
}
