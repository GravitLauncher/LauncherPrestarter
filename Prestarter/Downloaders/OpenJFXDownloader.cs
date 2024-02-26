using Prestarter.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Prestarter.Downloaders
{
    internal class OpenJFXDownloader : IRuntimeDownloader
    {
        private const string x64OriginalUrl = "https://download2.gluonhq.com/openjfx/21.0.2/openjfx-21.0.2_windows-x64_bin-sdk.zip";
        private const string x86OriginalUrl = "https://download2.gluonhq.com/openjfx/17.0.10/openjfx-17.0.10_windows-x86_bin-sdk.zip";

        private const string x64MirrorUrl = "https://gravit-jvm-mirror.re146.dev/openjfx-21.0.2_windows-x64_bin-sdk.zip";
        private const string x86MirrorUrl = "https://gravit-jvm-mirror.re146.dev/openjfx-17.0.10_windows-x64_bin-sdk.zip";

        // private const string x64ChecksumUrl = "https://download2.gluonhq.com/openjfx/21.0.2/openjfx-21.0.2_windows-x64_bin-sdk.zip.sha256";
        private const string x64Checksum = "503028499d0a935b721be1e682ac103e1919f80f3ec731f25501ce172b36b9b9";
        // private const string x86ChechsumUrl = "https://download2.gluonhq.com/openjfx/17.0.10/openjfx-17.0.10_windows-x64_bin-sdk.zip.sha256";
        private const string x86Checksum = "fcc52b66b74aed6b9c9252ee55f9eb39f9eabe3d0a712f2dd165554a35fccbb6";

        private const string x64Name = "OpenJFX 21 (x86_64)";
        private const string x86Name = "OpenJFX 17 (x86)";

        private string x64Url;
        private string x86Url;

        public OpenJFXDownloader(bool useMirror)
        {
            x64Url = useMirror ? x64MirrorUrl : x64OriginalUrl;
            x86Url = useMirror ? x86MirrorUrl : x86OriginalUrl;
        }

        public void Download(string javaPath, IUIReporter reporter)
        {
            var url = Environment.Is64BitOperatingSystem ? x64Url : x86Url;
            var checksum = Environment.Is64BitOperatingSystem ? x64Checksum : x86Checksum;
            var name = GetName();
            string zipPath = Path.Combine(javaPath, "openjfx.zip");
            reporter.SetStatus($"Скачивание {name}");
            reporter.SetProgress(0);
            reporter.SetProgressBarState(ProgressBarState.Progress);
            using (var file = new FileStream(zipPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Prestarter.SharedHttpClient.DownloadWithHash(url, checksum, SHA256.Create(),
                    file, reporter.SetProgress);
            }
            reporter.SetProgressBarState(ProgressBarState.Marqee);
            reporter.SetStatus($"Распаковка {name}");
            DownloaderHelper.UnpackZip(zipPath, javaPath, true);
            File.Delete(zipPath);
        }

        public string GetName() => Environment.Is64BitOperatingSystem ? x64Name : x86Name;

        public string GetDirectoryPrefix() => "openjfx";
    }
}
