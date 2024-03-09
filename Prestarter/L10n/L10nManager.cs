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
            { "ru", new RuLocale() }
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
