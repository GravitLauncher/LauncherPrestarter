using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Prestarter.Helpers
{
    internal abstract class DownloaderHelper
    {
        public static void UnpackZip(string zipPath, string targetPath, bool skipFirstDirectory)
        {
            using (var archive = ZipFile.OpenRead(zipPath))
            {
                string rootFolder = null;
                if (skipFirstDirectory)
                {
                    var firstEntry = archive.Entries.FirstOrDefault();
                    if (firstEntry != null)
                    {
                        rootFolder = firstEntry.FullName.Split('/')[0] + "/";
                    }
                }

                foreach (var entry in archive.Entries)
                {
                    string fileName;
                    if (skipFirstDirectory && !string.IsNullOrEmpty(rootFolder) && entry.FullName.StartsWith(rootFolder))
                    {
                        fileName = entry.FullName.Substring(rootFolder.Length);
                    }
                    else
                    {
                        fileName = entry.FullName;
                    }

                    fileName = fileName.Replace('/', '\\');
                    if (string.IsNullOrEmpty(fileName)) continue;
                    var path = Path.Combine(targetPath, fileName);
                    if (entry.FullName.EndsWith("/"))
                        Directory.CreateDirectory(path);
                    else
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(path));
                        entry.ExtractToFile(path, true);
                    }
                }
            }
        }
    }
}