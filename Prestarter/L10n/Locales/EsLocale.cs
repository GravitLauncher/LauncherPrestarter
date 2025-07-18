namespace Prestarter.Helpers
{
    internal class EsLocale : ILocale
    {
        public string JavaUpdateAvailableMessage => @"Actualización de Java disponible. ¿Actualizar?";

        public string ForLauncherStartupSoftwareIsRequiredMessage =>
            @"Para iniciar el lanzador {0} se requiere el software Java. ¿Descargar {1}?";


        public string InitializationStatus => @"Inicialización";
        public string SearchingForLauncherStatus => @"Búsqueda del lanzador";
        public string DownloadingLauncherStatus => @"Descarga del lanzador";
        public string StartingStatus => @"Inicio";
        public string DownloadingStatus => @"Descarga de {0}";
        public string UnpackingStatus => @"Desempaquetar {0}";
        public string DeletingOldJavaStatus => @"Eliminando la versión antigua de Java";
        public string BellSoftApiQueryStatus => @"Consulta a la API de BellSoft";
        public string BellSoftApiResponseParsingStatus => @"Análisis de la respuesta de la API de BellSoft";


        public string ParsingResponseError => @"Error al analizar la respuesta";
        public string InitializationError => @"Error durante la inicialización: el servidor devolvió el código {0}";
        public string LauncherHasExitedTooFastError => @"El proceso del lanzador se cerró demasiado rápido";
        public string HashsumIsIncorrectError => @"La suma de comprobación (hash) no coincide: {0} != {1}";
    }
}