using System;
using System.Globalization;
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

        private static string _appDataPath;
        private static string _projectPath;
        private static string _javaPath;
        private static string _launcherPath;

        public static string AppDataPath
        {
            get
            {
                if (_appDataPath == null)
                {
                    _appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                }
                return _appDataPath;
            }
        }
        public static string ProjectPath
        {
            get
            {
                if (_projectPath == null)
                {
                    _projectPath = Path.Combine(AppDataPath, Config.Project);

                    Directory.CreateDirectory(_projectPath);
                }
                return _projectPath;
            }
        }
        public static string JavaPath
        {
            get
            {
                if (_javaPath == null)
                {
                    if (Config.UseGlobalJava)
                        _javaPath = Path.Combine(AppDataPath, "GravitLauncherStore", "Java",
                            Config.JavaDownloader.GetDirectoryPrefix());
                    else
                        _javaPath = Path.Combine(ProjectPath, "jre-full");

                    Directory.CreateDirectory(_javaPath);
                }
                return _javaPath;
            }
        }
        public static string LauncherPath
        {
            get
            {
                if (_launcherPath == null)
                {
                    _launcherPath = Config.LauncherDownloadUrl != null
                        ? Path.Combine(ProjectPath, "Launcher.jar")
                        : Assembly.GetExecutingAssembly().Location;
                }

                return _launcherPath;
            }
        }

        public static JavaStatus GetJavaStatus(string lastUpdateDateFile)
        {
            try
            {
                if (!File.Exists(lastUpdateDateFile))
                    return JavaStatus.NotInstalled;

                var text = File.ReadAllText(lastUpdateDateFile);

                var parsed = DateTime.Parse(text, CultureInfo.InvariantCulture);

                return parsed.AddDays(30) < DateTime.Now
                    ? JavaStatus.NeedUpdate
                    : JavaStatus.Ok;
            }
            catch (Exception)
            {
                return JavaStatus.NotInstalled;
            }
        }

        public static bool NeedDownloadLauncher => Config.LauncherDownloadUrl != null && !File.Exists(LauncherPath);

    }
}