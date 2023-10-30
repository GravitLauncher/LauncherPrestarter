using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Prestarter
{
    internal class AdoptiumJavaDownloader : IRuntimeDownloader
    {
        private const string x64Name = "Adoptium JRE 21 (x86_64)";
        private const string x86Name = "Adoptium JRE 17 (x86)";

        private const string x64Url = "https://api.adoptium.net/v3/binary/latest/21/ga/windows/x64/jre/hotspot/normal/eclipse?project=jdk";
        private const string x86Url = "https://api.adoptium.net/v3/binary/latest/17/ga/windows/x86/jre/hotspot/normal/eclipse?project=jdk";

        public void Download(string javaPath, IUIReporter reporter)
        {
            var url = Environment.Is64BitOperatingSystem ? x64Url : x86Url;
            var name = GetName();
            string zipPath = $"{javaPath}.zip";
            reporter.SetStatus($"Скачивание {name}");
            reporter.SetProgressBarState(ProgressBarState.Progress);
            using (var file = new FileStream(zipPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Prestarter.SharedHttpClient.Download(url, file, reporter.SetProgress);
            }
            reporter.SetProgressBarState(ProgressBarState.Marqee);
            if (File.Exists(javaPath))
            {
                reporter.SetStatus("Удаление старой Java");
                Directory.Delete(javaPath, true);
            }
            reporter.SetStatus($"Распаковка {name}");
            Directory.CreateDirectory(javaPath);
            DownloaderHelper.UnpackZip(zipPath, javaPath, true);
            File.Delete(zipPath);
        }

        public string GetName() => Environment.Is64BitOperatingSystem ? x64Name : x86Name;

        public string GetDirectoryPrefix() => "adoptium-lts";
    }
}
