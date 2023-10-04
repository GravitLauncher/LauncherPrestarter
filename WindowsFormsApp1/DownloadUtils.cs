using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    internal class DownloadUtils
    {
        public static Task unpackZip(string zipPath, string targetPath, bool skipFirstDitrctory)
        {
            return Task.Run(() =>
            {
                using (ZipArchive archive = ZipFile.OpenRead(zipPath))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        string fileName;
                        if(skipFirstDitrctory)
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
            });
        }
        public static Task deleteDir(string path)
        {
            return Task.Run(() =>
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(path);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            });
        }
    }
}
