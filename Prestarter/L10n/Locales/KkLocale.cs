namespace Prestarter.Helpers
{
    internal class KkLocale : ILocale
    {
        public string JavaUpdateAvailableMessage => @"Java жаңарту қол жетімді. Жаңарту?";

        public string ForLauncherStartupSoftwareIsRequiredMessage =>
            @"{0} лаунчерін іске қосу үшін Java бағдарламасы қажет. {1} жүктеп алу керек пе?";


        public string InitializationStatus => @"Іске қосу";
        public string SearchingForLauncherStatus => @"Лаунчерді іздеу";
        public string DownloadingLauncherStatus => @"Лаунчерді жүктеу";
        public string StartingStatus => @"Іске қосу";
        public string DownloadingStatus => @"{0} жүктеу";
        public string UnpackingStatus => @"{0} шығару";
        public string DeletingOldJavaStatus => @"Ескі Java нұсқасын жою";
        public string BellSoftApiQueryStatus => @"BellSoft API сұрауы";
        public string BellSoftApiResponseParsingStatus => @"BellSoft API жауапты өңдеу";


        public string ParsingResponseError => @"Жауапты өңдеу кезінде қате пайда болды";
        public string InitializationError => @"Іске қосу кезінде қате пайда болды: сервер {0} кодты қайтарды";
        public string LauncherHasExitedTooFastError => @"Лаунчер процесі жылдам шығарылды";
        public string HashsumIsIncorrectError => @"Хеш-сумма сәйкес келмейді: {0} != {1}";
    }
}