using System;
using System.IO;
using System.IO.Compression;

namespace Prestarter.Helpers
{
    internal abstract class DownloaderHelper
    {
        public static void UnpackZip(string zipPath, string targetPath, bool skipFirstDirectory)
        {
            using (var archive = ZipFile.OpenRead(zipPath))
            {
                foreach (var entry in archive.Entries)
                {
                    string fileName;
                    if (skipFirstDirectory)
                    {
                        var index = entry.FullName.IndexOf("/", StringComparison.InvariantCultureIgnoreCase);
                        fileName = entry.FullName.Substring(index + 1);
                    }
                    else
                    {
                        fileName = entry.FullName;
                    }

                    fileName = fileName.Replace('/', '\\');
                    if (fileName == "") continue;
                    var path = Path.Combine(targetPath, fileName);
                    if (entry.FullName.EndsWith("/"))
                        Directory.CreateDirectory(path);
                    else
                        entry.ExtractToFile(path, true);
                }
            }
        }
    }
}