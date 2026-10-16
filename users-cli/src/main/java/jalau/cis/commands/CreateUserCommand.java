package jalau.cis.commands;

import jalau.cis.models.User;
import jalau.cis.services.ServicesFacade;
import picocli.CommandLine;

import java.util.UUID;
import java.util.concurrent.Callable;

@CommandLine.Command(name="-create", description = "Create a new user")
public class CreateUserCommand extends Command implements Callable<Integer> {
    @CommandLine.Option(description = "User Name", required = true, names = {"-n"})
    protected String userName;
    @CommandLine.Option(description = "User Login", required = true, names = {"-l"})
    protected String userLogin;
    @CommandLine.Option(description = "User Password", required = true, names = {"-p"})
    protected String userPassword;

    @Override
    public Integer call() throws Exception {
        try {
            var id = UUID.randomUUID().toString();
            User user = new User(id, userName, userLogin, userPassword);
            System.out.printf("Creating user {%s}\n", user);
            getUsersService().createUser(user);
            return 0;
        }
        catch (Exception ex) {
            System.out.printf("Cannot create user. %s", ex.getMessage());
            throw ex;
        }
    }
}
