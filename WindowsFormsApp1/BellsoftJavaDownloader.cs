using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    internal class BellsoftJavaDownloader : IPartDownloader
    {
        private const string X86_64_NAME = "Bellsoft JRE Full (x86_64)";
        private const string X86_NAME = "Bellsoft JRE Full (x86)";
        public async Task<Boolean> Download(string javaPath, Prestarter prestarter)
        {
            var bitness = System.Environment.Is64BitOperatingSystem ? "64" : "32";
            prestarter.reporter.updateStatus("Запрос к BellSoft API");
            prestarter.reporter.requestWaitProgressbar();
            var url = "https://api.bell-sw.com/v1/liberica/releases?version-modifier=latest&bitness=" + bitness + "&release-type=lts&os=windows&arch=x86&package-type=zip&bundle-type=jre-full";
            var result = await Prestarter.sharedClient.GetAsync(url);
            if (!result.IsSuccessStatusCode)
            {
                prestarter.ReportErrorAndExit("Прооизошла ошибка во время инициализации: сервер вернул код " + result.StatusCode);
            }
            prestarter.reporter.updateStatus("Обработка ответа от BellSoft API");
            var bellsoftApiResult = await result.Content.ReadAsStringAsync();
            // Json parsing not supported in .NET 4.5.1 and any C# libraries can't be static linked
            // Very bad json parsing here:
            int downloadUrlIndex = bellsoftApiResult.IndexOf("downloadUrl\":\"") + "downloadUrl\":\"".Length;
            int endIndex = bellsoftApiResult.IndexOf("\"", downloadUrlIndex + 1);
            string downloadUrl = bellsoftApiResult.Substring(downloadUrlIndex, endIndex - downloadUrlIndex);
            //
            string zipPath = javaPath + ".zip";
            prestarter.reporter.updateStatus("Скачивание Liberica Full JRE");
            prestarter.reporter.requestNormalProgressbar();
            using (var file = new FileStream(zipPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await Prestarter.sharedClient.DownloadAsync(downloadUrl, file, prestarter.progress);
            }
            prestarter.reporter.requestWaitProgressbar();
            if (File.Exists(javaPath))
            {
                prestarter.reporter.updateStatus("Удаление старой Java");
                await DownloadUtils.deleteDir(javaPath);
            }
            prestarter.reporter.updateStatus("Распаковка");
            Directory.CreateDirectory(javaPath);
            await DownloadUtils.unpackZip(zipPath, javaPath, true);
            File.Delete(zipPath);
            return false; // Download OpenJFX not needed
        }

        public string GetName()
        {
            if (System.Environment.Is64BitOperatingSystem)
            {
                return X86_64_NAME;
            }
            else
            {
                return X86_NAME;
            }
        }

        public string getPrefix()
        {
            return "bellsoft-lts";
        }
    }
}
