using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

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
