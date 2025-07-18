using System;
using System.IO;
using System.Reflection;

namespace Prestarter.Helpers
{
    public static class SystemHelper
    {
        public enum JavaStatus
        {
            Ok,
            NotInstalled,
            NeedUpdate
        }
        
        public static string InitializeJavaPath(string basePath)
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            string javaPath;
            if (Config.UseGlobalJava)
                javaPath = Path.Combine(appData, "GravitLauncherStore", "Java",
                    Config.JavaDownloader.GetDirectoryPrefix());
            else
                javaPath = Path.Combine(basePath, "jre-full");
            
            if (!Directory.Exists(javaPath))
                Directory.CreateDirectory(javaPath);

            return javaPath;
        }

        public static string InitializeBasePath()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            var basePath = Path.Combine(appData, Config.Project);
            
            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            return basePath;
        }

        public static JavaStatus GetJavaStatus(string lastUpdateDateFile)
        {
            try
            {
                if (!File.Exists(lastUpdateDateFile)) 
                    return JavaStatus.NotInstalled;
                
                var text = File.ReadAllText(lastUpdateDateFile);
                
                var parsed = DateTime.Parse(text);
                
                return parsed.AddDays(30) < DateTime.Now 
                    ? JavaStatus.NeedUpdate 
                    : JavaStatus.Ok;
            }
            catch (Exception)
            {
                return JavaStatus.NotInstalled;
            }
        }

        public static bool NeedDownloadLauncher(string launcherPath)
        {
            return Config.LauncherDownloadUrl != null && !File.Exists(launcherPath);
        }

        public static string GetLauncherPath(string basePath)
        {
            if (Config.LauncherDownloadUrl != null)
            {
                return Path.Combine(basePath, "Launcher.jar");
            }
            
            return Assembly.GetExecutingAssembly().Location;
        }
    }
}