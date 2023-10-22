using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    internal class OpenJFXDownloader : IPartDownloader
    {
        private const string X86_64_NAME = "OpenJFX 21 (x86_64)";
        private const string X86_NAME = "OpenJFX 17 (x86)";
        public async Task<bool> Download(string javaPath, Prestarter prestarter)
        {
            string url;
            string name;
            if(System.Environment.Is64BitOperatingSystem)
            {
                url = "https://download2.gluonhq.com/openjfx/21.0.1/openjfx-21.0.1_windows-x64_bin-sdk.zip";
                name = X86_64_NAME;
            } else
            {
                url = "https://download2.gluonhq.com/openjfx/17.0.8/openjfx-17.0.8_windows-x86_bin-sdk.zip";
                name = X86_NAME;
            }
            string zipPath = javaPath + "-openjfx.zip";
            prestarter.reporter.updateStatus("Скачивание "+name);
            prestarter.reporter.requestNormalProgressbar();
            using (var file = new FileStream(zipPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                await Prestarter.sharedClient.DownloadAsync(url, file, prestarter.progress);
            }
            prestarter.reporter.requestWaitProgressbar();
            prestarter.reporter.updateStatus("Распаковка "+name);
            await DownloadUtils.unpackZip(zipPath, javaPath, true);
            File.Delete(zipPath);
            return false;
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
            return "openjfx";
        }
    }
}
