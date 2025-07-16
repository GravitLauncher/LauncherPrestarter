using System;
using System.IO;

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

        public static JavaStatus GetJavaStatus(string javaPath)
        {
            try
            {
                var path = Path.Combine(javaPath, "date-updated");

                
                if (!File.Exists(path)) 
                    return JavaStatus.NotInstalled;
                
                var text = File.ReadAllText(path);
                
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
    }
}