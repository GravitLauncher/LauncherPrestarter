using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Prestarter
{
    internal partial class PrestarterForm : Form, IUIReporter
    {
        private bool dragging = false;
        
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public PrestarterForm()
        {
            InitializeComponent();
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
        }

        private void PreStartedForm_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void PreStartedForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (!dragging)
            {
                return;
            }
            var difference = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
            Location = Point.Add(dragFormPoint, new Size(difference));
        }

        private void PreStartedForm_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = Location;
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
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
            Invoke(new Action(() => mainProgressBar.Value = (int) Math.Round(value * 100)));
        }

        public void ShowForm()
        {
            Invoke(new Action(() =>
            {
                WindowState = FormWindowState.Normal;
                ShowInTaskbar = true;
            }));
        }

        private void PrestarterForm_Load(object sender, EventArgs args)
        {
            new Thread(() =>
            {
                var prestarter = new Prestarter(this);
                try
                {
                    prestarter.Run();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "GravitLauncher Prestarter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                Environment.Exit(0);
            }).Start();
        }
    }
}
