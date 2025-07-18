namespace Prestarter.Helpers
{
    internal class FrLocale : ILocale
    {
        public string JavaUpdateAvailableMessage => @"Mise à jour Java disponible. Mettre à jour?";

        public string ForLauncherStartupSoftwareIsRequiredMessage =>
            @"Pour démarrer le lanceur {0}, le logiciel Java est requis. Télécharger {1}?";


        public string InitializationStatus => @"Initialisation";
        public string SearchingForLauncherStatus => @"Recherche du lanceur";
        public string DownloadingLauncherStatus => @"Téléchargement du lanceur";
        public string StartingStatus => @"Démarrage";
        public string DownloadingStatus => @"Téléchargement de {0}";
        public string UnpackingStatus => @"Décompression de {0}";
        public string DeletingOldJavaStatus => @"Suppression de l'ancienne version de Java";
        public string BellSoftApiQueryStatus => @"Requête à l'API BellSoft";
        public string BellSoftApiResponseParsingStatus => @"Analyse de la réponse de l'API BellSoft";


        public string ParsingResponseError => @"Erreur lors de l'analyse de la réponse";
        public string InitializationError => @"Erreur lors de l'initialisation: le serveur a renvoyé le code {0}";
        public string LauncherHasExitedTooFastError => @"Le processus du lanceur s'est terminé trop rapidement";
        public string HashsumIsIncorrectError => @"La somme de contrôle (hash) est incorrecte : {0} != {1}";
    }
}