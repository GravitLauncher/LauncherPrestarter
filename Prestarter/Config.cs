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
    }
}