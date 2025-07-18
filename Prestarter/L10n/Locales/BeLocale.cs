namespace Prestarter.Helpers
{
    internal class BeLocale : ILocale
    {
        public string JavaUpdateAvailableMessage => @"Даступна абнаўленне Java. Абнавіць?";

        public string ForLauncherStartupSoftwareIsRequiredMessage =>
            @"Для запуску лаунчэра {0} неабходна праграмнае забеспячэнне Java. Спампаваць {1}?";


        public string InitializationStatus => @"Ініцыялізацыя";
        public string SearchingForLauncherStatus => @"Пошук лаунчэра";
        public string DownloadingLauncherStatus => @"Спампоўка лаунчэра";
        public string StartingStatus => @"Запуск";
        public string DownloadingStatus => @"Спампоўка {0}";
        public string UnpackingStatus => @"Распакаванне {0}";
        public string DeletingOldJavaStatus => @"Выдаленне старой Java";
        public string BellSoftApiQueryStatus => @"Запыт да BellSoft API";
        public string BellSoftApiResponseParsingStatus => @"Апрацоўка адказу ад BellSoft API";


        public string ParsingResponseError => @"Адбылася памылка пры апрацоўцы адказу";
        public string InitializationError => @"Адбылася памылка пры ініцыялізацыі: сервер вярнуў код {0}";
        public string LauncherHasExitedTooFastError => @"Процэс лаунчэра зачыніўся занадта хутка";
        public string HashsumIsIncorrectError => @"Хеш-сума не супадае: {0} != {1}";
    }
}