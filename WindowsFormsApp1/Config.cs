namespace WindowsFormsApp1
{
    internal class Config
    {
        public static string VERSION = "0.1.0";
        public static string PROJECT = "Minecraft";
        public static string LAUNCHER_URL = "http://localhost:9274/Launcher.jar";
        public static bool ENABLE_DOWNLOAD_QUESTION = true;
        public static bool USAGE_GLOBAL_JAVA = true;
        public static IPartDownloader javaDownloader = new AdoptiumJavaDownloader();
        public static IPartDownloader openjfxDownloader = new OpenJFXDownloader();
        public static string[] args; // Don't touch
    }
}
