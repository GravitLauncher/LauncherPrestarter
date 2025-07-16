namespace Prestarter.Helpers
{
    internal class RuLocale : ILocale
    {
        public string JavaUpdateAvailableMessage => @"Доступно обновление Java. Обновить?";

        public string ForLauncherStartupSoftwareIsRequiredMessage =>
            @"Для запуска лаунчера {0} необходимо программное обеспечение Java. Скачать {1}?";


        public string InitializationStatus => @"Инициализация";
        public string SearchingForLauncherStatus => @"Поиск лаунчера";
        public string DownloadingLauncherStatus => @"Скачивание лаунчера";
        public string StartingStatus => @"Запуск";
        public string DownloadingStatus => @"Скачивание {0}";
        public string UnpackingStatus => @"Распаковка {0}";
        public string DeletingOldJavaStatus => @"Удаление старой Java";
        public string BellSoftApiQueryStatus => @"Запрос к BellSoft API";
        public string BellSoftApiResponseParsingStatus => @"Обработка ответа от BellSoft API";


        public string ParsingResponseError => @"Произошла ошибка во время обработки ответа";
        public string InitializationError => @"Произошла ошибка во время инициализации: сервер вернул код {0}";
        public string LauncherHasExitedTooFastError => @"Процесс лаунчера закрылся слишком быстро";
        public string HashsumIsIncorrectError => @"Хеш-сумма не совпадает: {0} != {1}";
    }
}