using System;
using System.Threading;
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
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PrestarterForm());
        }
    }
}
