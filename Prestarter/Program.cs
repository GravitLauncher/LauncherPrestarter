using System.Net;
using System.Windows.Forms;

namespace Prestarter
{
    internal enum ProgressBarState
    {
        Marqee,
        Progress
    }

    internal interface IUIReporter
    {
        void SetProgressBarState(ProgressBarState state);
        void SetProgress(float value);
        void SetStatus(string status);
        void ShowForm();
    }

    internal static class Program
    {
        public static string[] Arguments;

        private static void Main(string[] args)
        {
            Arguments = args;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PrestarterForm());
        }
    }
}