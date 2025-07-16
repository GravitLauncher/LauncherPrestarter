namespace Prestarter.Helpers
{
    internal class LvLocale : ILocale
    {
        public string JavaUpdateAvailableMessage => @"Pieejama Java atjauninājums. Vai atjaunināt?";

        public string ForLauncherStartupSoftwareIsRequiredMessage =>
            @"Launcherim {0} nepieciešama Java programmatūra. Vai lejupielādēt {1}?";


        public string InitializationStatus => @"Inicializācija";
        public string SearchingForLauncherStatus => @"Meklē launcheru";
        public string DownloadingLauncherStatus => @"Lejupielādē launcheru";
        public string StartingStatus => @"Sākums";
        public string DownloadingStatus => @"Lejupielādē {0}";
        public string UnpackingStatus => @"Izpako {0}";
        public string DeletingOldJavaStatus => @"Dzēst veco Java versiju";
        public string BellSoftApiQueryStatus => @"Pieprasījums uz BellSoft API";
        public string BellSoftApiResponseParsingStatus => @"Atbildes no BellSoft API apstrāde";


        public string ParsingResponseError => @"Kļūda, apstrādājot atbildi";
        public string InitializationError => @"Kļūda, inicializējot: serveris atgrieza kodu {0}";
        public string LauncherHasExitedTooFastError => @"Launcheris aizgāja pārāk ātri";
        public string HashsumIsIncorrectError => @"Kontrolsumma nesakrīt: {0} != {1}";
    }
}