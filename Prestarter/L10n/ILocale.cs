namespace Prestarter.Helpers
{
    internal interface ILocale
    {
        string JavaUpdateAvailableMessage { get; }
        string ForLauncherStartupSoftwareIsRequiredMessage { get; }


        string InitializationStatus { get; }
        string SearchingForLauncherStatus { get; }
        string DownloadingLauncherStatus { get; }
        string StartingStatus { get; }
        string DownloadingStatus { get; }
        string UnpackingStatus { get; }
        string DeletingOldJavaStatus { get; }
        string BellSoftApiQueryStatus { get; }
        string BellSoftApiResponseParsingStatus { get; }


        string ParsingResponseError { get; }
        string InitializationError { get; }
        string LauncherHasExitedTooFastError { get; }
        string HashsumIsIncorrectError { get; }
    }
}