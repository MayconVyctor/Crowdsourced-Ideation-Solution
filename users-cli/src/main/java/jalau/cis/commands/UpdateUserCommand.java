package jalau.cis.commands;

import jalau.cis.models.User;
import jalau.cis.services.ServicesFacade;
import picocli.CommandLine;

import java.util.concurrent.Callable;

@CommandLine.Command(name="-update", description = "Update an existing user")
public class UpdateUserCommand extends Command implements Callable<Integer> {
    @CommandLine.Option(description = "User Name", required = false, names = {"-n"})
    protected String userName;
    @CommandLine.Option(description = "User Login", required = false, names = {"-l"})
    protected String userLogin;
    @CommandLine.Option(description = "User Password", required = false, names = {"-p"})
    protected String userPassword;
    @CommandLine.Option(description = "User Id", required = true, names = {"-i"})
    private String userId;

    @Override
    public Integer call() throws Exception {
        try {
            User user = new User(userId, userName, userLogin, userPassword);
            System.out.printf("Updating user {%s}\n", userId);
            getUsersService().updateUser(user);
            return 0;
        }
        catch (Exception ex) {
            System.out.printf("Cannot update user. %s\n", ex.getMessage());
            throw ex;
        }
    }
}
