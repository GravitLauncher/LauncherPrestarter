using System;
using System.Drawing;

namespace Prestarter.Helpers
{
    public class ColorHelper
    {
        /// <summary>
        /// Преобразует HEX строку в объект Color
        /// </summary>
        /// <param name="hexColor">HEX цвет в формате "#RRGGBB" или "RRGGBB"</param>
        /// <returns>Объект Color</returns>
        public static Color FromHex(string hexColor)
        {
            if (string.IsNullOrEmpty(hexColor))
                throw new ArgumentException("HEX цвет не может быть пустым", nameof(hexColor));

            // Убираем # если есть
            hexColor = hexColor.Replace("#", "");

            // Проверяем длину
            if (hexColor.Length != 6)
                throw new ArgumentException("HEX цвет должен содержать 6 символов", nameof(hexColor));

            try
            {
                // Преобразуем в RGB компоненты
                int r = Convert.ToInt32(hexColor.Substring(0, 2), 16);
                int g = Convert.ToInt32(hexColor.Substring(2, 2), 16);
                int b = Convert.ToInt32(hexColor.Substring(4, 2), 16);

                return Color.FromArgb(r, g, b);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Некорректный HEX цвет: {hexColor}", nameof(hexColor), ex);
            }
        }

        /// <summary>
        /// Преобразует HEX строку в объект Color с поддержкой альфа-канала
        /// </summary>
        /// <param name="hexColor">HEX цвет в формате "#AARRGGBB" или "AARRGGBB"</param>
        /// <returns>Объект Color</returns>
        public static Color FromHexWithAlpha(string hexColor)
        {
            if (string.IsNullOrEmpty(hexColor))
                throw new ArgumentException("HEX цвет не может быть пустым", nameof(hexColor));

            hexColor = hexColor.Replace("#", "");

            if (hexColor.Length != 8)
                throw new ArgumentException("HEX цвет с альфа-каналом должен содержать 8 символов", nameof(hexColor));

            try
            {
                int a = Convert.ToInt32(hexColor.Substring(0, 2), 16);
                int r = Convert.ToInt32(hexColor.Substring(2, 2), 16);
                int g = Convert.ToInt32(hexColor.Substring(4, 2), 16);
                int b = Convert.ToInt32(hexColor.Substring(6, 2), 16);

                return Color.FromArgb(a, r, g, b);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Некорректный HEX цвет с альфа-каналом: {hexColor}", nameof(hexColor), ex);
            }
        }

        /// <summary>
        /// Преобразует объект Color в HEX строку
        /// </summary>
        /// <param name="color">Цвет для преобразования</param>
        /// <param name="includeAlpha">Включить альфа-канал в результат</param>
        /// <returns>HEX строка</returns>
        public static string ToHex(Color color, bool includeAlpha = false)
        {
            if (includeAlpha)
                return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
            else
                return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        /// <summary>
        /// Создает цвет из RGB компонентов
        /// </summary>
        /// <param name="r">Красный компонент (0-255)</param>
        /// <param name="g">Зеленый компонент (0-255)</param>
        /// <param name="b">Синий компонент (0-255)</param>
        /// <returns>Объект Color</returns>
        public static Color FromRgb(int r, int g, int b)
        {
            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// Создает цвет из ARGB компонентов
        /// </summary>
        /// <param name="a">Альфа-канал (0-255)</param>
        /// <param name="r">Красный компонент (0-255)</param>
        /// <param name="g">Зеленый компонент (0-255)</param>
        /// <param name="b">Синий компонент (0-255)</param>
        /// <returns>Объект Color</returns>
        public static Color FromArgb(int a, int r, int g, int b)
        {
            return Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// Затемняет цвет на указанный процент
        /// </summary>
        /// <param name="color">Исходный цвет</param>
        /// <param name="percent">Процент затемнения (0-100)</param>
        /// <returns>Затемненный цвет</returns>
        public static Color Darken(Color color, int percent)
        {
            if (percent < 0 || percent > 100)
                throw new ArgumentOutOfRangeException(nameof(percent), "Процент должен быть в диапазоне 0-100");

            float factor = (100 - percent) / 100f;
            return Color.FromArgb(color.A, 
                (int)(color.R * factor), 
                (int)(color.G * factor), 
                (int)(color.B * factor));
        }

        /// <summary>
        /// Осветляет цвет на указанный процент
        /// </summary>
        /// <param name="color">Исходный цвет</param>
        /// <param name="percent">Процент осветления (0-100)</param>
        /// <returns>Осветленный цвет</returns>
        public static Color Lighten(Color color, int percent)
        {
            if (percent < 0 || percent > 100)
                throw new ArgumentOutOfRangeException(nameof(percent), "Процент должен быть в диапазоне 0-100");

            float factor = percent / 100f;
            return Color.FromArgb(color.A,
                (int)(color.R + (255 - color.R) * factor),
                (int)(color.G + (255 - color.G) * factor),
                (int)(color.B + (255 - color.B) * factor));
        }
    }
}