using Prestarter.Downloaders;

namespace Prestarter
{
    /// <summary>
    ///     Класс конфигурации приложения
    /// </summary>
    internal class Config
    {
        /// <summary>
        ///     Название проекта, как указано в настройках лаунчсервера
        /// </summary>
        public static readonly string Project = "Minecraft";

        /// <summary>
        ///     Версия приложения
        /// </summary>
        public static readonly string Version = "0.1.0";

        /// <summary>
        ///     URL для скачивания лаунчера. Если null - использовать встроенный в модуль
        /// </summary>
        public static readonly string LauncherDownloadUrl = null;

        /// <summary>
        ///     Показывать ли диалог перед скачиванием Java
        /// </summary>
        public static readonly bool DownloadQuestionEnabled = true;

        /// <summary>
        ///     Использовать общую Java для всех лаунчеров
        /// </summary>
        public static readonly bool UseGlobalJava = true;

        /// <summary>
        ///     Загрузчик Java, использует Adoptium и OpenJFX
        /// </summary>
        public static readonly IRuntimeDownloader JavaDownloader =
            new CompositeDownloader(new AdoptiumJavaDownloader(), new OpenJFXDownloader(true));
        
        /// <summary>
        ///     Наименование диалога
        /// </summary>
        public static string DialogName => $"{Project} Prestarter";
        
        /// <summary>
        /// Основной цвет
        /// </summary>
        public static readonly string PrimaryColorHex = "#6C70F1";
        /// <summary>
        /// Цвето фона
        /// </summary>
        public static readonly string BackgroundColorHex = "#1A1A1A";
        /// <summary>
        /// Цвет текста
        /// </summary>
        public static readonly string ForegroundColorHex = "#969696";
        /// <summary>
        /// Цвет Кнопки
        /// </summary>
        public static readonly string ButtonColorHex = "#202020";
        /// <summary>
        /// Цвет Кнопки при наведении
        /// </summary>
        public static readonly string ButtonHoverColorHex = "#2E2E2E";
    }
}