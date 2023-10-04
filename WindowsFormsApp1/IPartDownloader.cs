using System;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    internal interface IPartDownloader
    {
        Task<Boolean> Download(string javaPath, Prestarter prestarter);
        string GetName();
        string getPrefix();
    }
}
