package jalau.cis.commands;

import jalau.cis.services.ServicesFacade;
import jalau.cis.services.UsersService;

public class Command {
    protected UsersService getUsersService() throws Exception {
        return ServicesFacade.getInstance().getUsersService();
    }
}
