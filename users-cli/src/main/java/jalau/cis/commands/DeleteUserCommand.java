package jalau.cis.commands;

import picocli.CommandLine;
import java.util.concurrent.Callable;

@CommandLine.Command(name="-delete", description = "Delete a User by ID")
public class DeleteUserCommand extends  Command implements Callable<Integer> {
    @CommandLine.Parameters(index = "0", description = "User Id", paramLabel = "id") String userID;

    @Override
    public Integer call() throws Exception {
        System.out.printf("Will delete an user with id = [%s]\n", userID);
        try {
            int count = getUsersService().deleteUser(userID);
            if (count <= 0) {
                throw new Exception("User does not exist");
            }
            return 0;
        }
        catch (Exception ex) {
            System.out.println(ex.getMessage());
            throw ex;
        }


    }
}
