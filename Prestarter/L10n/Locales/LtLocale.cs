namespace Prestarter.Helpers
{
    internal class LtLocale : ILocale
    {
        public string JavaUpdateAvailableMessage => @"Prieinama Java atnaujinimas. Atnaujinti?";

        public string ForLauncherStartupSoftwareIsRequiredMessage =>
            @"Paleidimui reikalinga Java programinė įranga. Parsisiųsti {0}?";


        public string InitializationStatus => @"Inicijavimas";
        public string SearchingForLauncherStatus => @"Ieškoma launchero";
        public string DownloadingLauncherStatus => @"Siunčiamas launcheris";
        public string StartingStatus => @"Paleidimas";
        public string DownloadingStatus => @"Siunčiama {0}";
        public string UnpackingStatus => @"Išpakavimas {0}";
        public string DeletingOldJavaStatus => @"Trinama sena Java versija";
        public string BellSoftApiQueryStatus => @"Užklausa į BellSoft API";
        public string BellSoftApiResponseParsingStatus => @"Atsakymo iš BellSoft API apdorojimas";


        public string ParsingResponseError => @"Klaida apdorojant atsakymą";
        public string InitializationError => @"Klaida inicijavimo metu: serveris grąžino kodą {0}";
        public string LauncherHasExitedTooFastError => @"Launcherio procesas užsidarė per greitai";
        public string HashsumIsIncorrectError => @"Kontrolinė suma (hash) neteisinga: {0} != {1}";
    }
}