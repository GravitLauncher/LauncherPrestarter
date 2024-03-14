using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prestarter.Helpers
{

    internal class L10nManager
    {
        private static ILocale defaultLocale = new EnUsLocale();

        private static readonly Dictionary<string, ILocale> locales = new Dictionary<string, ILocale>
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

        public static ILocale Locale
        {
            get
            {
                if (bestLocale == null)
                {
                    bestLocale = GetBestLocale();
                }
                return bestLocale;
            }
        }

        private static ILocale bestLocale;

        private static ILocale GetBestLocale()
        {
            var languageName = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            if (locales.ContainsKey(languageName))
            {
                return locales[languageName];
            }

            return defaultLocale;
        }
    }
}
