using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    internal class AdoptiumJavaDownloader : IPartDownloader
    {
        private const string X86_64_NAME = "Adoptium JRE 21 (x86_64)";
        private const string X86_NAME = "Adoptium JRE 17 (x86)";
        public async Task<bool> Download(string javaPath, Prestarter prestarter)
        {
            string url;
            string name;
            if(System.Environment.Is64BitOperatingSystem)
            {
                url = "https://api.adoptium.net/v3/binary/latest/21/ga/windows/x64/jre/hotspot/normal/eclipse?project=jdk";
                name = X86_64_NAME;
            } else
            {
                url = "https://api.adoptium.net/v3/binary/latest/17/ga/windows/x86/jre/hotspot/normal/eclipse?project=jdk";
                name = X86_NAME;
            }
            string zipPath = javaPath + ".zip";
            prestarter.reporter.updateStatus("Скачивание "+name);
            prestarter.reporter.requestNormalProgressbar();
            using (var file = new FileStream(zipPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await Prestarter.sharedClient.DownloadAsync(url, file, prestarter.progress);
            }
            prestarter.reporter.requestWaitProgressbar();
            if (File.Exists(javaPath))
            {
                prestarter.reporter.updateStatus("Удаление старой Java");
                await DownloadUtils.deleteDir(javaPath);
            }
            prestarter.reporter.updateStatus("Распаковка "+name);
            Directory.CreateDirectory(javaPath);
            await DownloadUtils.unpackZip(zipPath, javaPath, true);
            File.Delete(zipPath);
            return true; // Download OpenJFX required
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
            return "adoptium-lts";
        }
    }
}
