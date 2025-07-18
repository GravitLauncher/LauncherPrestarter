using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Windows.Forms;
using Prestarter.Helpers;

namespace Prestarter
{
    internal class PrestarterCore
    {
        public static readonly HttpClient SharedHttpClient = new HttpClient();

        private readonly IUIReporter _reporter;

        public PrestarterCore(IUIReporter reporter)
        {
            _reporter = reporter;
        }

        private void VerifyAndDownloadJava(string javaPath)
        {
            var lastUpdateDateFile = Path.Combine(javaPath, "date-updated");
            
            var javaStatus = SystemHelper.GetJavaStatus(lastUpdateDateFile);

            if (Config.DownloadQuestionEnabled)
            {
                switch (javaStatus)
                {
                    case SystemHelper.JavaStatus.NeedUpdate when CheckNeedJavaUpdate() == false:
                    case SystemHelper.JavaStatus.NotInstalled when CheckNeedDownloadDialog() == false:
                    case SystemHelper.JavaStatus.Ok:
                        return;
                    default:
                        Console.WriteLine(javaStatus);
                        break;
                }
            }

            _reporter.ShowForm();
            
            Config.JavaDownloader.Download(javaPath, _reporter);

            File.WriteAllText(lastUpdateDateFile, DateTime.Now.ToString(CultureInfo.InvariantCulture));
        }

        private static bool CheckNeedDownloadDialog()
        {
            var message = string.Format(
                I18n.ForLauncherStartupSoftwareIsRequiredMessage,
                Config.Project,
                Config.JavaDownloader.GetName()
            );
                    
            var result = MessageBox.Show(message, Config.DialogName, MessageBoxButtons.YesNo);

            return result == DialogResult.Yes;
        }

        private static bool CheckNeedJavaUpdate()
        {
            var result = MessageBox.Show(
                I18n.JavaUpdateAvailableMessage,
                Config.DialogName,
                MessageBoxButtons.YesNoCancel
            );
            
            return result == DialogResult.Yes;
        }

        public void Run()
        {
            _reporter.SetStatus(I18n.InitializationStatus);

            var basePath = SystemHelper.InitializeBasePath();
            var javaPath = SystemHelper.InitializeJavaPath(basePath);

            VerifyAndDownloadJava(basePath);

            _reporter.SetStatus(I18n.SearchingForLauncherStatus);
            
            var launcherPath = Path.Combine(basePath, "Launcher.jar");

            if (SystemHelper.NeedDownloadLauncher(launcherPath))
            {
                DownloadLauncher(launcherPath);
            }

            _reporter.SetStatus(I18n.StartingStatus);
            
            var process = GetProcess(javaPath, launcherPath);

            process.Start();
            
            if (process.WaitForExit(500)) 
                throw new Exception(I18n.LauncherHasExitedTooFastError);
        }

        private static Process GetProcess(string javaPath, string launcherPath)
        {
            var additionalArguments = string.Join(" ", Program.Arguments.Select(e => $"\"{e}\""));

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Path.Combine(javaPath, "bin", "java.exe"),
                    Arguments = $"-Dlauncher.noJavaCheck=true -jar \"{launcherPath}\" {additionalArguments}",
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            
            return process;
        }

        private void DownloadLauncher(string launcherPath)
        {
            _reporter.ShowForm();
            _reporter.SetStatus(I18n.DownloadingLauncherStatus);
            _reporter.SetProgress(0);
            _reporter.SetProgressBarState(ProgressBarState.Progress);
            using (var file = new FileStream(launcherPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                SharedHttpClient.Download(Config.LauncherDownloadUrl, file, value => _reporter.SetProgress(value));
            }

            _reporter.SetProgressBarState(ProgressBarState.Marqee);
        }
    }
}