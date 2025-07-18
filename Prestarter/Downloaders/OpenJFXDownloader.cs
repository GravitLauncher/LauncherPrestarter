using System;
using System.IO;
using System.Security.Cryptography;
using Prestarter.Helpers;

namespace Prestarter.Downloaders
{
    internal class OpenJFXDownloader : IRuntimeDownloader
    {
        private const string x64OriginalUrl =
            "https://download2.gluonhq.com/openjfx/23.0.1/openjfx-23.0.1_windows-x64_bin-sdk.zip";

        private const string x86OriginalUrl =
            "https://download2.gluonhq.com/openjfx/17.0.8/openjfx-17.0.8_windows-x86_bin-sdk.zip";

        private const string x64MirrorUrl =
            "https://gravit-jvm-mirror.re146.dev/openjfx-23.0.1_windows-x64_bin-sdk.zip";

        private const string x86MirrorUrl =
            "https://gravit-jvm-mirror.re146.dev/openjfx-17.0.8_windows-x86_bin-sdk.zip";

        // private const string x64ChecksumUrl = "https://download2.gluonhq.com/openjfx/23.0.1/openjfx-23.0.1_windows-x64_bin-sdk.zip.sha256";
        private const string x64Checksum = "0f4d58b15a7148a203ab72ce376f1fa17b243e38e6d66e29d04a0177ae5dc3d3";

        // private const string x86ChechsumUrl = "https://download2.gluonhq.com/openjfx/17.0.8/openjfx-17.0.8_windows-x86_bin-sdk.zip.sha256";
        private const string x86Checksum = "c3a56d545f2614465664b66cb4c178ddd7fe37d4ec7b53d2248a2d80f4a04ed7";

        private const string x64Name = "OpenJFX 23 (x86_64)";
        private const string x86Name = "OpenJFX 17 (x86)";

        private readonly string x64Url;
        private readonly string x86Url;

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
            var zipPath = Path.Combine(javaPath, "openjfx.zip");
            reporter.SetStatus(string.Format(I18n.DownloadingStatus, name));
            reporter.SetProgress(0);
            reporter.SetProgressBarState(ProgressBarState.Progress);
            using (var file = new FileStream(zipPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                PrestarterCore.SharedHttpClient.DownloadWithHash(url, checksum, SHA256.Create(),
                    file, reporter.SetProgress);
            }

            reporter.SetProgressBarState(ProgressBarState.Marqee);
            reporter.SetStatus(string.Format(I18n.UnpackingStatus, name));
            DownloaderHelper.UnpackZip(zipPath, javaPath, true);
            File.Delete(zipPath);
        }

        public string GetName()
        {
            return Environment.Is64BitOperatingSystem ? x64Name : x86Name;
        }

        public string GetDirectoryPrefix()
        {
            return "openjfx";
        }
    }
}