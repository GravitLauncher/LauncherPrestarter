package pro.gravit.launcher.launchermodules.prestarter;

import pro.gravit.launchserver.LaunchServer;
import pro.gravit.launchserver.binary.LauncherBinary;

public class PrestarterLauncherBinary extends LauncherBinary {
    private final PrestarterModule module;
    protected PrestarterLauncherBinary(LaunchServer server, PrestarterModule module) {
        super(server, LauncherBinary.resolve(server, ".exe"), "Launcher-%s.exe");
        this.module = module;
    }

    @Override
    public void init() {
        tasks.add(new PrestarterTask(server, module));
    }
}
