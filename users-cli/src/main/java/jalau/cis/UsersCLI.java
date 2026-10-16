package jalau.cis;

import jalau.cis.commands.UsersCommand;
import picocli.CommandLine;

public class UsersCLI {
    public static void main(String[] args) {
        System.out.println("Users CLI");
        CommandLine cmd = new CommandLine(new UsersCommand());
        cmd.setExecutionStrategy(new CommandLine.RunAll()).execute(args);
    }
}