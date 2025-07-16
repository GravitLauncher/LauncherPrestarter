using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Prestarter.Controls
{
    public class CircularButton : Button
    {
        private Color _hoverColor = Color.FromArgb(232, 17, 35);
        private Color _normalColor = Color.FromArgb(200, 200, 200);
        private Color _textColor = Color.White;
        private bool _isHovered = false;

        public Color HoverColor
        {
            get => _hoverColor;
            set { _hoverColor = value; Invalidate(); }
        }

        public Color NormalColor
        {
            get => _normalColor;
            set { _normalColor = value; Invalidate(); }
        }

        public Color TextButtonColor
        {
            get => _textColor;
            set { _textColor = value; Invalidate(); }
        }

        public CircularButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Size = new Size(30, 30);
            BackColor = Color.Transparent;
            ForeColor = _textColor;
            Text = "×";
            Font = new Font("Arial", 14, FontStyle.Bold);
            Cursor = Cursors.Hand;

            // Включаем двойную буферизацию
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw | 
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.UserPaint,
                true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Создаем круглую кнопку
            using (var path = new GraphicsPath())
            {
                path.AddEllipse(0, 0, Width - 1, Height - 1);
                this.Region = new Region(path);

                // Цвет кнопки зависит от того, наведена ли на нее мышь
                using (SolidBrush brush = new SolidBrush(_isHovered ? _hoverColor : _normalColor))
                {
                    e.Graphics.FillPath(brush, path);
                }

                // Рисуем текст на кнопке
                TextRenderer.DrawText(
                    e.Graphics,
                    Text,
                    Font,
                    new Rectangle(0, 0, Width, Height),
                    ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _isHovered = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _isHovered = false;
            Invalidate();
            base.OnMouseLeave(e);
        }
    }
}
