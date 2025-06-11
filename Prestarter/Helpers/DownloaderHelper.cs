using System.IO;
using System.IO.Compression;

namespace Prestarter.Helpers
{
    internal class DownloaderHelper
    {
        public static void UnpackZip(string zipPath, string targetPath, bool skipFirstDitrctory)
        {
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string fileName;
                    if (skipFirstDitrctory)
                    {
                        int index = entry.FullName.IndexOf("/");
                        fileName = entry.FullName.Substring(index + 1);
                    }
                    else
                    {
                        fileName = entry.FullName;
                    }
                    fileName = fileName.Replace('/', '\\');
                    if (fileName == "")
                    {
                        continue;
                    }
                    string path = Path.Combine(targetPath, fileName);
                    if (entry.FullName.EndsWith("/"))
                    {
                        Directory.CreateDirectory(path);
                    }
                    else
                    {
                        entry.ExtractToFile(path, overwrite: true);
                    }
                }
            }
        }
    }
}
