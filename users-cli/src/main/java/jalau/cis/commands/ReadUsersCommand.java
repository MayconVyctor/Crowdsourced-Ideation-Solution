package jalau.cis.commands;

import jalau.cis.models.User;
import picocli.CommandLine;

import java.util.concurrent.Callable;
import java.util.concurrent.atomic.AtomicInteger;

@CommandLine.Command(name = "-read", description = "Read Users")
public class ReadUsersCommand extends Command implements Callable<Integer> {
    @Override
    public Integer call() throws Exception {
        try {
            var users = getUsersService().getUsers();
            System.out.printf("Users found: [%d]\n", users.size());
            AtomicInteger count = new AtomicInteger(0);
            for (User user: users) {
                System.out.printf("[%d] %s\n", count.addAndGet(1), user);
            }
            return 0;
        }
        catch (Exception ex) {
            System.out.printf("Cannot read users. %s", ex.getMessage());
            throw ex;
        }
    }
}
