namespace Prestarter.Helpers
{
    internal class UkLocale : ILocale
    {
        public string JavaUpdateAvailableMessage => @"Доступне оновлення Java. Оновити?";

        public string ForLauncherStartupSoftwareIsRequiredMessage =>
            @"Для запуску лаунчера {0} потрібне програмне забезпечення Java. Завантажити {1}?";


        public string InitializationStatus => @"Ініціалізація";
        public string SearchingForLauncherStatus => @"Пошук лаунчера";
        public string DownloadingLauncherStatus => @"Завантаження лаунчера";
        public string StartingStatus => @"Запуск";
        public string DownloadingStatus => @"Завантаження {0}";
        public string UnpackingStatus => @"Розпакування {0}";
        public string DeletingOldJavaStatus => @"Видалення старої версії Java";
        public string BellSoftApiQueryStatus => @"Запит до BellSoft API";
        public string BellSoftApiResponseParsingStatus => @"Обробка відповіді від BellSoft API";


        public string ParsingResponseError => @"Виникла помилка під час обробки відповіді";
        public string InitializationError => @"Виникла помилка під час ініціалізації: сервер повернув код {0}";
        public string LauncherHasExitedTooFastError => @"Процес лаунчера закрився занадто швидко";
        public string HashsumIsIncorrectError => @"Хеш-сума не співпадає: {0} != {1}";
    }
}