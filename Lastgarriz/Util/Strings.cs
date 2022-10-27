using System.Collections.Generic;

namespace Lastgarriz.Util
{
    internal static class Strings // only const, no parameters
    {
        internal static readonly string HllClass = "UnrealWindow";
        internal static readonly string HllCaption = "Hell Let Loose  ";

        internal static readonly string[] Culture = { "en-US", "ko-KR", "fr-FR", "es-ES", "de-DE", "pt-BR", "ru-RU", "th-TH", "zh-TW", "zh-CN" };

        internal static readonly string VersionUrl = "https://raw.githubusercontent.com/maxensas/lastgarriz/master/VERSION";
        internal const string KEYLOG = "keylog";

        internal static class Aim
        {
            internal const string NOVALUE = "target";
            internal const string SCHRECK = "schreck";
            internal const string SCHRECK_STEADY = "steady schreck";
            internal const string BAZOOKA = "bazooka";
            internal const string BAZOOKA_STEADY = "steady bazooka";
        }

        internal static class File
        {
            internal const string CONFIG = "Config.json";
            internal const string DEFAULT_CONFIG = "DefaultConfig.json";
        }

        internal static class View
        {
            internal const string CONFIGURATION = "Configuration";
            internal const string ARTILLERY = "Artillery";
            internal const string ROCKET = "Rocket";
        }

        internal static class Feature
        {
            internal const string CLOSE = "close";
            internal const string CONFIG = "config";
            internal const string ARTILLERY_USGER = "artillery_usger";
            internal const string ARTILLERY_RU = "artillery_ru";
            internal const string ARTILLERY_VALIDATE = "artillery_validate";
            internal const string ROCKETINDICATOR_ENABLE = "rocket_enable";
            internal const string ROCKETINDICATOR_START = "rocket_start";
            internal const string AUTOQUEUE = "autoqueue";
            internal const string BROWSER = "browser";

            internal static readonly List<string> Unregisterable = new() { ARTILLERY_USGER, ARTILLERY_RU, AUTOQUEUE }; // BROWSER
        }

        internal static class Net
        {
            internal static readonly string UserAgent = "Lastgarriz/" + Common.GetFileVersion() + " (contact: x_toset_x@gmail.com)";
        }
    }
}
