using System.Collections.Generic;
using System.Globalization;

namespace Prestarter.Helpers
{
    internal class L10nManager
    {
        private static readonly ILocale DefaultLocale = new EnUsLocale();

        private static readonly Dictionary<string, ILocale> Locales = new Dictionary<string, ILocale>
        {
            { "be", new BeLocale() },
            { "es", new EsLocale() },
            { "fr", new FrLocale() },
            { "kk", new KkLocale() },
            { "lt", new LtLocale() },
            { "lv", new LvLocale() },
            { "pl", new PlLocale() },
            { "ru", new RuLocale() },
            { "uk", new UkLocale() }
        };

        private static ILocale _bestLocale;

        public static ILocale Locale => _bestLocale ?? (_bestLocale = GetBestLocale());

        private static ILocale GetBestLocale()
        {
            var languageName = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

            return Locales.TryGetValue(languageName, out var locale)
                ? locale
                : DefaultLocale;
        }
    }
}