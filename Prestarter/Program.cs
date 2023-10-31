using System.Windows.Forms;

using System;
using System.Threading;
using System.Windows.Forms;
using System.Security.Policy;

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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PrestarterForm());
        }
    }
}
