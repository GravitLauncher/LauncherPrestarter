using Prestarter.Helpers;

namespace Prestarter
{
    internal static class I18n
    {
        public static string JavaUpdateAvailableMessage => L10nManager.Locale.JavaUpdateAvailableMessage;

        public static string ForLauncherStartupSoftwareIsRequiredMessage =>
            L10nManager.Locale.ForLauncherStartupSoftwareIsRequiredMessage;


        public static string InitializationStatus => L10nManager.Locale.InitializationStatus;
        public static string SearchingForLauncherStatus => L10nManager.Locale.SearchingForLauncherStatus;
        public static string DownloadingLauncherStatus => L10nManager.Locale.DownloadingLauncherStatus;


        public static string StartingStatus => L10nManager.Locale.StartingStatus;
        public static string DownloadingStatus => L10nManager.Locale.DownloadingStatus;
        public static string UnpackingStatus => L10nManager.Locale.UnpackingStatus;
        public static string DeletingOldJavaStatus => L10nManager.Locale.DeletingOldJavaStatus;
        public static string BellSoftApiQueryStatus => L10nManager.Locale.BellSoftApiQueryStatus;
        public static string BellSoftApiResponseParsingStatus => L10nManager.Locale.BellSoftApiResponseParsingStatus;


        public static string ParsingResponseError => L10nManager.Locale.ParsingResponseError;
        public static string InitializationError => L10nManager.Locale.InitializationError;
        public static string LauncherHasExitedTooFastError => L10nManager.Locale.LauncherHasExitedTooFastError;
        public static string HashsumIsIncorrectError => L10nManager.Locale.HashsumIsIncorrectError;
    }
}