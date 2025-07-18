using System;
using System.IO;
using Prestarter.Helpers;

namespace Prestarter.Downloaders
{
    internal class AdoptiumJavaDownloader : IRuntimeDownloader
    {
        private const string x64Name = "Adoptium JRE 21 (x86_64)";
        private const string x86Name = "Adoptium JRE 17 (x86)";

        private const string x64Url =
            "https://api.adoptium.net/v3/binary/latest/21/ga/windows/x64/jre/hotspot/normal/eclipse?project=jdk";

        private const string x86Url =
            "https://api.adoptium.net/v3/binary/latest/17/ga/windows/x86/jre/hotspot/normal/eclipse?project=jdk";

        public void Download(string javaPath, IUIReporter reporter)
        {
            var url = Environment.Is64BitOperatingSystem ? x64Url : x86Url;
            var name = GetName();
            var zipPath = Path.Combine(javaPath, "java.zip");
            reporter.SetStatus(string.Format(I18n.DownloadingStatus, name));
            reporter.SetProgress(0);
            reporter.SetProgressBarState(ProgressBarState.Progress);
            using (var file = new FileStream(zipPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                PrestarterCore.SharedHttpClient.Download(url, file, reporter.SetProgress);
            }

            reporter.SetProgressBarState(ProgressBarState.Marqee);
            if (File.Exists(javaPath))
            {
                reporter.SetStatus(I18n.DeletingOldJavaStatus);
                Directory.Delete(javaPath, true);
            }

            reporter.SetStatus(string.Format(I18n.UnpackingStatus, name));

            if (!Directory.Exists(javaPath))
                Directory.CreateDirectory(javaPath);
            
            DownloaderHelper.UnpackZip(zipPath, javaPath, true);
            File.Delete(zipPath);
        }

        public string GetName()
        {
            return Environment.Is64BitOperatingSystem ? x64Name : x86Name;
        }

        public string GetDirectoryPrefix()
        {
            return "adoptium-lts";
        }
    }
}