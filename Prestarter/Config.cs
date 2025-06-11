using Prestarter.Downloaders;

namespace Prestarter
{
    internal class Config
    {
        public static string Project = "Minecraft";
        public static string Version = "0.1.0";

        public static string LauncherDownloadUrl = null;

        public static bool DownloadQuestionEnabled = true;

        public static bool UseGlobalJava = true;
        public static IRuntimeDownloader JavaDownloader = new CompositeDownloader(new AdoptiumJavaDownloader(), new OpenJFXDownloader(true));
    }
}
