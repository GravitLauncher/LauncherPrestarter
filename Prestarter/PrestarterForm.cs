using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
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
        private int borderRadius = 20;

        // Импорт необходимых WinAPI функций
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );

        [DllImport("dwmapi.dll")]
        private static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("dwmapi.dll")]
        private static extern int DwmIsCompositionEnabled(ref int pfEnabled);

        // Структура для определения полей окна
        private struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }

        public PrestarterForm()
        {
            InitializeComponent();
            this.logoLabel.Text = Config.Project;
            this.Text = $"{Config.Project} Prestarter";
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;

            // Создаем закругленную форму при загрузке
            this.FormBorderStyle = FormBorderStyle.None;
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, borderRadius, borderRadius));

            // Для обеспечения тени и корректного обновления региона при изменении размера
            this.Resize += new EventHandler(Form_Resize);
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            // Обновляем регион при изменении размера
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, borderRadius, borderRadius));
        }

        // Переопределение CreateParams для создания тени
        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_DROPSHADOW = 0x20000;

                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW; // Добавляем тень
                return cp;
            }
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

            // Настройка кнопки закрытия
            exitButton.NormalColor = ColorHelper.FromHex(Config.ButtonColorHex);
            exitButton.HoverColor = ColorHelper.FromHex(Config.ButtonHoverColorHex);
            
            new Thread(() =>
            {
                var prestarter = new PrestarterCore(this);
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