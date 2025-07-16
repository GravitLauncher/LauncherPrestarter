namespace Prestarter.Helpers
{
    internal class PlLocale : ILocale
    {
        public string JavaUpdateAvailableMessage => @"Dostępna aktualizacja Java. Zaktualizować?";

        public string ForLauncherStartupSoftwareIsRequiredMessage =>
            @"Do uruchomienia launchera {0} wymagane jest oprogramowanie Java. Pobrać {1}?";


        public string InitializationStatus => @"Inicjalizacja";
        public string SearchingForLauncherStatus => @"Wyszukiwanie launchera";
        public string DownloadingLauncherStatus => @"Pobieranie launchera";
        public string StartingStatus => @"Uruchamianie";
        public string DownloadingStatus => @"Pobieranie {0}";
        public string UnpackingStatus => @"Rozpakowywanie {0}";
        public string DeletingOldJavaStatus => @"Usuwanie starej wersji Java";
        public string BellSoftApiQueryStatus => @"Zapytanie do API BellSoft";
        public string BellSoftApiResponseParsingStatus => @"Przetwarzanie odpowiedzi z API BellSoft";


        public string ParsingResponseError => @"Wystąpił błąd podczas przetwarzania odpowiedzi";
        public string InitializationError => @"Wystąpił błąd podczas inicjalizacji: serwer zwrócił kod {0}";
        public string LauncherHasExitedTooFastError => @"Proces launchera zakończył się zbyt szybko";
        public string HashsumIsIncorrectError => @"Suma kontrolna (hash) jest niepoprawna: {0} != {1}";
    }
}