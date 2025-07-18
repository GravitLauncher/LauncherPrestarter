namespace Prestarter.Downloaders
{
    internal interface IRuntimeDownloader
    {
        void Download(string javaPath, IUIReporter prestarter);
        string GetName();
        string GetDirectoryPrefix();
    }
}