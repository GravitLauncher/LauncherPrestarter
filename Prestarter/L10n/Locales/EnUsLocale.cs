namespace Prestarter.Helpers
{
    internal class EnUsLocale : ILocale
    {
        public string JavaUpdateAvailableMessage => @"Java update available. Would you like us to update it?";

        public string ForLauncherStartupSoftwareIsRequiredMessage =>
            @"To launch {0} A speciffic Java version required. Download now {1}?";


        public string InitializationStatus => @"Initialization";
        public string SearchingForLauncherStatus => @"Searching existing launcher";
        public string DownloadingLauncherStatus => @"Downloading launcher";
        public string StartingStatus => @"Starting";
        public string DownloadingStatus => @"Downloading {0}";
        public string UnpackingStatus => @"Unpacking {0}";
        public string DeletingOldJavaStatus => @"Removing old Java";
        public string BellSoftApiQueryStatus => @"Querying BellSoft API";
        public string BellSoftApiResponseParsingStatus => @"Processing response from BellSoft API";


        public string ParsingResponseError => @"An error occurred while processing the response";

        public string InitializationError =>
            @"An error occurred during initialization: the server returned the code {0}";

        public string LauncherHasExitedTooFastError => @"The launcher process exited unexpectedly";
        public string HashsumIsIncorrectError => @"Checksum does not match: {0} != {1}";
    }
}