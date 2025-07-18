using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Prestarter.Controls
{
    public class CustomProgressBar : ProgressBar
    {
        private Color _customColor = Color.Green;
        private int _radius = 6;

        public Color ProgressBarColor
        {
            get => _customColor;
            set { _customColor = value; Invalidate(); }
        }

        public int BorderRadius
        {
            get => _radius;
            set { _radius = value; Invalidate(); }
        }

        public CustomProgressBar()
        {
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var rec = e.ClipRectangle;

            // Включаем сглаживание для лучшего качества рисования
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Создаем прямоугольную область с закругленными углами для фона
            using (GraphicsPath path = CreateRoundedRectangle(new Rectangle(0, 0, rec.Width, rec.Height), BorderRadius))
            {
                // Рисуем фон
                using (SolidBrush brush = new SolidBrush(BackColor))
                {
                    e.Graphics.FillPath(brush, path);
                }

                // Рисуем прогресс
                if (Value > 0)
                {
                    float percent = (float)Value / Maximum;
                    int width = (int)(rec.Width * percent);

                    // Создаем прямоугольную область с закругленными углами для прогресса
                    // Прогресс должен быть обрезан до текущего значения
                    Rectangle progressRect = new Rectangle(0, 0, width, rec.Height);
                    using (GraphicsPath progressPath = CreateRoundedRectangle(progressRect, BorderRadius))
                    {
                        using (SolidBrush brush = new SolidBrush(ProgressBarColor))
                        {
                            e.Graphics.FillPath(brush, progressPath);
                        }
                    }
                }
            }
        }

        // Вспомогательный метод для создания пути с закругленными углами
        private GraphicsPath CreateRoundedRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            // Левый верхний угол
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);

            // Верхний край
            path.AddLine(rect.X + radius, rect.Y, rect.Right - radius, rect.Y);

            // Правый верхний угол
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);

            // Правый край
            path.AddLine(rect.Right, rect.Y + radius, rect.Right, rect.Bottom - radius);

            // Правый нижний угол
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);

            // Нижний край
            path.AddLine(rect.Right - radius, rect.Bottom, rect.X + radius, rect.Bottom);

            // Левый нижний угол
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);

            // Левый край
            path.AddLine(rect.X, rect.Bottom - radius, rect.X, rect.Y + radius);

            path.CloseFigure();
            return path;
        }
    }
}