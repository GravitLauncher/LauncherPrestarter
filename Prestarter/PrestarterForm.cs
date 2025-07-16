using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Prestarter.Helpers;

namespace Prestarter
{
    internal partial class PrestarterForm : Form, IUIReporter
    {
        private Point dragCursorPoint;
        private Point dragFormPoint;
        private bool dragging;

        public PrestarterForm()
        {
            InitializeComponent();
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
        }

        public void SetProgressBarState(ProgressBarState state)
        {
            Invoke(new Action(() =>
            {
                switch (state)
                {
                    case ProgressBarState.Marqee:
                        mainProgressBar.Style = ProgressBarStyle.Marquee;
                        break;
                    case ProgressBarState.Progress:
                        mainProgressBar.Style = ProgressBarStyle.Continuous;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(state));
                }
            }));
        }

        public void SetStatus(string status)
        {
            Invoke(new Action(() => statusLabel.Text = status));
        }

        public void SetProgress(float value)
        {
            Invoke(new Action(() => mainProgressBar.Value = (int)Math.Round(value * 100)));
        }

        public void ShowForm()
        {
            Invoke(new Action(() =>
            {
                WindowState = FormWindowState.Normal;
                ShowInTaskbar = true;
            }));
        }

        private void FormMouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void FormMouseMove(object sender, MouseEventArgs e)
        {
            if (!dragging) return;
            var difference = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
            Location = Point.Add(dragFormPoint, new Size(difference));
        }

        private void FormMouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = Location;
        }

        private void CloseWindow(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }

        private void FormLoaded(object sender, EventArgs args)
        {
            logoLabel.Text = Config.Project;
            BackColor = ColorHelper.FromHex(Config.BackgroundColorHex);
            ForeColor = ColorHelper.FromHex(Config.ForegroundColorHex);
            mainProgressBar.ProgressBarColor = ColorHelper.FromHex(Config.PrimaryColorHex);
            mainProgressBar.BackColor = BackColor;
            
            new Thread(() =>
            {
                var prestarter = new Prestarter(this);
                try
                {
                    prestarter.Run();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, Config.DialogName, MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

                Environment.Exit(0);
            }).Start();
        }
    }
}