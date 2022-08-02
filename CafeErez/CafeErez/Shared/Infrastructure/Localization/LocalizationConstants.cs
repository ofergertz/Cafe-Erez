using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeErez.Shared.Infrastructure.Localization
{
    public static class LocalizationConstants
    {
        public static readonly LanguageCode[] SupportedLanguages = {
            new LanguageCode
            {
                Code = "he-IL",
                DisplayName= "Hebrew",
                isRtl = true
            },
            new LanguageCode
            {
                Code = "en-US",
                DisplayName= "English"
            },
            new LanguageCode
            {
                Code = "fr-FR",
                DisplayName = "French"
            },
            new LanguageCode
            {
                Code = "de-DE",
                DisplayName = "German"
            },
            new LanguageCode
            {
                Code = "nl-NL",
                DisplayName = "Dutch - Netherlands"
            },
            new LanguageCode
            {
                Code = "es-ES",
                DisplayName = "Spanish"
            },
            new LanguageCode
            {
                Code = "ru-RU",
                DisplayName = "Russian"
            },
            new LanguageCode
            {
                Code = "it-IT",
                DisplayName = "Italian"
            },
            new LanguageCode
            {
                Code = "ar",
                DisplayName = "Arabic"
            }
        };
    }
}
