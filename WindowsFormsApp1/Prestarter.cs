using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    internal class Prestarter
    {
        private string projectName;
        private static HttpClient sharedClient = new HttpClient();
        private IProgress<float> progress;
        private IStatusPeporter reporter;
        private string launcherUrl;

        public Prestarter(string projectName, IProgress<float> progress, string launcherUrl, IStatusPeporter reporter)
        {
            this.projectName = projectName;
            this.progress = progress;
            this.launcherUrl = launcherUrl;
            this.reporter = reporter;
        }

        public async void run()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string basePath = Environment.GetEnvironmentVariable("APPDATA") + "\\" + projectName;
            Directory.CreateDirectory(basePath);
            string javaPath = basePath + "\\" + "jre-full";
            if(!File.Exists(javaPath) && !File.Exists(javaPath+"\\complete-flag"))
            {
                var bitness = System.Environment.Is64BitOperatingSystem ? "64" : "32";
                reporter.updateStatus("Запрос к BellSoft API");
                reporter.requestWaitProgressbar();
                var url = "https://api.bell-sw.com/v1/liberica/releases?version-modifier=latest&bitness="+bitness+"&release-type=lts&os=windows&arch=x86&package-type=zip&bundle-type=jre-full";
                var result = await sharedClient.GetAsync(url);
                if (result != null)
                {
                    if(!result.IsSuccessStatusCode)
                    {
                        ReportErrorAndExit("Прооизошла ошибка во время инициализации: сервер вернул код " + result.StatusCode);
                    }
                    reporter.updateStatus("Обработка ответа от BellSoft API");
                    var bellsoftApiResult = await result.Content.ReadAsStringAsync();
                    // Json parsing not supported in .NET 4.5.1 and any C# libraries can't be static linked
                    // Very bad json parsing here:
                    int downloadUrlIndex = bellsoftApiResult.IndexOf("downloadUrl\":\"")+ "downloadUrl\":\"".Length;
                    int endIndex = bellsoftApiResult.IndexOf("\"", downloadUrlIndex + 1);
                    string downloadUrl = bellsoftApiResult.Substring(downloadUrlIndex, endIndex-downloadUrlIndex);
                    //
                    string zipPath = javaPath + ".zip";
                    reporter.updateStatus("Скачивание Liberica Full JRE");
                    reporter.requestNormalProgressbar();
                    using (var file = new FileStream(zipPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await sharedClient.DownloadAsync(downloadUrl, file, progress);
                    }
                    reporter.requestWaitProgressbar();
                    reporter.updateStatus("Распаковка");
                    Directory.CreateDirectory(javaPath);
                    using (ZipArchive archive = ZipFile.OpenRead(zipPath))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            int index = entry.FullName.IndexOf("/");
                            string sub = entry.FullName.Substring(index + 1).Replace('/', '\\');
                            if (sub == "")
                            {
                                continue;
                            }
                            string path = Path.Combine(javaPath, sub);
                            if(entry.FullName.EndsWith("/"))
                            {
                                Directory.CreateDirectory(path);
                            } else
                            {
                                entry.ExtractToFile(path);
                            }
                        }
                    }
                    File.Delete(zipPath);
                    File.WriteAllBytes(javaPath + "\\complete-flag", new byte[0]);
                }
            }
            reporter.updateStatus("Поиск лаунчера");
            string launcherPath = basePath + "\\Launcher.jar";
            if(launcherUrl == null)
            {
                launcherPath = System.Reflection.Assembly.GetEntryAssembly().Location;
            } else if(!File.Exists(launcherPath))
            {
                reporter.updateStatus("Скачивание лаунчера");
                reporter.requestNormalProgressbar();
                using (var file = new FileStream(launcherPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await sharedClient.DownloadAsync(launcherUrl, file, progress);
                }
                reporter.requestWaitProgressbar();
            }
            reporter.updateStatus("Запуск");
            Process process = new Process();
            // Configure the process using the StartInfo properties.
            process.StartInfo.FileName = javaPath + "\\bin\\java.exe";
            process.StartInfo.Arguments = "-Dlauncher.noJavaCheck=true -jar \""+launcherPath+"\"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            Thread startThread = new Thread(() =>
            {
                if(process.WaitForExit(500))
                {
                    ReportErrorAndExit("Процесс лаунчера завершился слишком быстро");
                }
                Application.Exit();
            });
            startThread.Start();
        }

        private void ReportErrorAndExit(string message)
        {
            MessageBox.Show(message, "GravitLauncher Prestarter", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
        }
    }
}