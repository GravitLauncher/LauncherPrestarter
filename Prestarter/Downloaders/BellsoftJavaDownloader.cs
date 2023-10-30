using Prestarter.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Prestarter
{
    internal class BellsoftJavaDownloader : IRuntimeDownloader
    {
        private const string x64Name = "Bellsoft JRE Full (x86_64)";
        private const string x86Name = "Bellsoft JRE Full (x86)";

        public void Download(string javaPath, IUIReporter reporter)
        {
            var bitness = Environment.Is64BitOperatingSystem ? "64" : "32";
            reporter.SetStatus("Запрос к BellSoft API");
            reporter.SetProgressBarState(ProgressBarState.Marqee);
            var url = $"https://api.bell-sw.com/v1/liberica/releases?version-modifier=latest&bitness={bitness}&release-type=lts&os=windows&arch=x86&package-type=zip&bundle-type=jre-full";
            var result = Prestarter.SharedHttpClient.GetAsync(url).Result;
            if (!result.IsSuccessStatusCode)
            {
                throw new Exception($"Произошла ошибка во время инициализации: сервер вернул код {result.StatusCode}");
            }

            reporter.SetStatus("Обработка ответа от BellSoft API");
            var bellsoftApiResult = result.Content.ReadAsStringAsync().Result;

            var parsed = new JsonParser().Parse(bellsoftApiResult);
            var downloadUrl = ((parsed as List<object>)?[0] as Dictionary<string, object>)?["downloadUrl"] as string;
            if (downloadUrl == null)
            {
                throw new Exception("Произошла ошибка во время обработки ответа");
            }

            var zipPath = $"{javaPath}.zip";
            reporter.SetStatus("Скачивание Liberica Full JRE");
            reporter.SetProgressBarState(ProgressBarState.Progress);
            using (var file = new FileStream(zipPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Prestarter.SharedHttpClient.Download(downloadUrl, file, reporter.SetProgress);
            }
            reporter.SetProgressBarState(ProgressBarState.Marqee);
            if (File.Exists(javaPath))
            {
                reporter.SetStatus("Удаление старой Java");
                Directory.Delete(javaPath, true);
            }
            reporter.SetStatus("Распаковка");
            Directory.CreateDirectory(javaPath);
            DownloaderHelper.UnpackZip(zipPath, javaPath, true);
            File.Delete(zipPath);
        }

        public string GetName() => Environment.Is64BitOperatingSystem ? x64Name : x86Name;

        public string GetDirectoryPrefix() => "bellsoft-lts";
    }
}
