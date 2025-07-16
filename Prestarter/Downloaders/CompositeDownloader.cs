using System.Linq;

namespace Prestarter.Downloaders
{
    internal class CompositeDownloader : IRuntimeDownloader
    {
        private readonly IRuntimeDownloader[] downloaders;

        public CompositeDownloader(params IRuntimeDownloader[] downloaders)
        {
            this.downloaders = downloaders;
        }

        public void Download(string javaPath, IUIReporter reporter)
        {
            foreach (var downloader in downloaders) 
                downloader.Download(javaPath, reporter);
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