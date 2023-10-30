using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prestarter.Downloaders
{
    class CompositeDownloader : IRuntimeDownloader
    {
        private readonly IRuntimeDownloader[] downloaders;

        public CompositeDownloader(params IRuntimeDownloader[] downloaders)
        {
            this.downloaders = downloaders;
        }

        public void Download(string javaPath, IUIReporter reporter)
        {
            foreach (var downloader in downloaders)
            {
                downloader.Download(javaPath, reporter);
            }
        }

        public string GetDirectoryPrefix()
        {
            return string.Join("-", downloaders.Select(item => item.GetDirectoryPrefix()));
        }

        public string GetName()
        {
            return string.Join(", ", downloaders.Select(item => item.GetName()));
        }
    }
}
