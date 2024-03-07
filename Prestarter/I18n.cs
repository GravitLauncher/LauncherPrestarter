using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Prestarter
{
    internal static class I18n
    {
        public static string JavaUpdateAvailableMessage => Properties.Resources.JavaUpdateAvailableMessage;
        public static string ForLauncherStartupSoftwareIsRequiredMessage => Properties.Resources.ForLauncherStartupSoftwareIsRequiredMessage;


        public static string InitializationStatus => Properties.Resources.InitializationStatus;
        public static string SearchingForLauncherStatus => Properties.Resources.SearchingForLauncherStatus;
        public static string DownloadingLauncherStatus => Properties.Resources.DownloadingLauncherStatus;


        public static string StartingStatus => Properties.Resources.StartingStatus;
        public static string DownloadingStatus => Properties.Resources.DownloadingStatus;
        public static string UnpackingStatus => Properties.Resources.UnpackingStatus;
        public static string DeletingOldJavaStatus => Properties.Resources.DeletingOldJavaStatus;
        public static string BellSoftApiQueryStatus => Properties.Resources.BellSoftApiQueryStatus;
        public static string BellSoftApiResponseParsingStatus => Properties.Resources.BellSoftApiResponseParsingStatus;


        public static string ParsingResponseError => Properties.Resources.ParsingResponseError;
        public static string InitializationError => Properties.Resources.InitializationError;
        public static string LauncherHasExitedTooFastError => Properties.Resources.LauncherHasExitedTooFastError;
        public static string HashsumIsIncorrectError => Properties.Resources.HashsumIsIncorrectError;
    }
}
