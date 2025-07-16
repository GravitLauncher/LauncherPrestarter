using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Windows.Forms;
using Prestarter.Helpers;

namespace Prestarter
{
    internal class Prestarter
    {
        public static HttpClient SharedHttpClient = new HttpClient();

        public IUIReporter reporter;

        public Prestarter(IUIReporter reporter)
        {
            this.reporter = reporter;
        }

        private static JavaStatus CheckJavaUpdateDate(string path)
        {
            try
            {
                if (!File.Exists(path)) return JavaStatus.NotInstalled;
                var text = File.ReadAllText(path);
                var parsed = DateTime.Parse(text);
                var now = DateTime.Now;
                if (parsed.AddDays(30) < now) return JavaStatus.NeedUpdate;
                return JavaStatus.Ok;
            }
            catch (Exception)
            {
                return JavaStatus.NotInstalled;
            }
        }

        private string VerifyAndDownloadJava(string basePath)
        {
            var appData = Environment.GetEnvironmentVariable("APPDATA");

            string javaPath;
            if (Config.UseGlobalJava)
                javaPath = Path.Combine(appData, "GravitLauncherStore", "Java",
                    Config.JavaDownloader.GetDirectoryPrefix());
            else
                javaPath = Path.Combine(basePath, "jre-full");
            Directory.CreateDirectory(javaPath);

            var dateFilePath = Path.Combine(javaPath, "date-updated");
            var javaStatus = CheckJavaUpdateDate(dateFilePath);
            if (javaStatus == JavaStatus.Ok) return javaPath;
            if (Config.DownloadQuestionEnabled)
            {
                if (javaStatus == JavaStatus.NeedUpdate)
                {
                    var dialog = MessageBox.Show(I18n.JavaUpdateAvailableMessage, "Prestarter",
                        MessageBoxButtons.YesNoCancel);
                    if (dialog == DialogResult.No) return javaPath;

                    if (dialog == DialogResult.Cancel) return null;
                }
                else
                {
                    var dialog = MessageBox.Show(
                        string.Format(I18n.ForLauncherStartupSoftwareIsRequiredMessage, Config.Project,
                            Config.JavaDownloader.GetName()),
                        "Prestarter", MessageBoxButtons.OKCancel);
                    if (dialog != DialogResult.OK) return null;
                }
            }
            else if (javaStatus == JavaStatus.Ok)
            {
                return javaPath;
            }

            reporter.ShowForm();
            Config.JavaDownloader.Download(javaPath, reporter);

            File.WriteAllText(dateFilePath, DateTime.Now.ToString());
            return javaPath;
        }

        public void Run()
        {
            reporter.SetStatus(I18n.InitializationStatus);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var appData = Environment.GetEnvironmentVariable("APPDATA");

            var basePath = Path.Combine(appData, Config.Project);
            Directory.CreateDirectory(basePath);

            var javaPath = VerifyAndDownloadJava(basePath);
            if (javaPath == null) return;

            reporter.SetStatus(I18n.SearchingForLauncherStatus);
            var launcherPath = Path.Combine(basePath, "Launcher.jar");

            if (Config.LauncherDownloadUrl == null)
            {
                launcherPath = Assembly.GetEntryAssembly().Location;
            }
            else if (!File.Exists(launcherPath))
            {
                reporter.ShowForm();
                reporter.SetStatus(I18n.DownloadingLauncherStatus);
                reporter.SetProgress(0);
                reporter.SetProgressBarState(ProgressBarState.Progress);
                using (var file = new FileStream(launcherPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    SharedHttpClient.Download(Config.LauncherDownloadUrl, file, value => reporter.SetProgress(value));
                }

                reporter.SetProgressBarState(ProgressBarState.Marqee);
            }

            reporter.SetStatus(I18n.StartingStatus);
            var args = "";
            foreach (var e in Program.Arguments)
            {
                args += " \"";
                args += e;
                args += "\"";
            }

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Path.Combine(javaPath, "bin", "java.exe"),
                    Arguments = $"-Dlauncher.noJavaCheck=true -jar \"{launcherPath}\" {args}",
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            if (process.WaitForExit(500)) throw new Exception(I18n.LauncherHasExitedTooFastError);
        }

        private enum JavaStatus
        {
            Ok,
            NotInstalled,
            NeedUpdate
        }
    }
}