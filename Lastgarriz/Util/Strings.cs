using System.Collections.Generic;

namespace Lastgarriz.Util
{
    /// <summary>
    /// Contain all constant strings needed by the application.
    /// </summary>
    /// <remarks>This is a static class and can not be instancied.</remarks>
    internal static class Strings
    {
        internal static readonly string HllClass = "UnrealWindow";
        internal static readonly string HllCaption = "Hell Let Loose  "; // HLL-Win64-Shipping.exe // "Hell Let Loose  "

        internal static readonly string[] Culture = { "en-US", "ko-KR", "fr-FR", "es-ES", "de-DE", "pt-BR", "ru-RU", "th-TH", "zh-TW", "zh-CN" };

        internal static readonly string VersionUrl = "https://raw.githubusercontent.com/maxensas/lastgarriz/master/VERSION";
        internal const string KEYLOG = "keylog";
        internal static readonly string OcrPath = @"./Assets/Data/Ocr";

        internal static class Aim
        {
            internal const string NOVALUE = "target";
            internal const string SCHRECK = "schreck";
            internal const string SCHRECK_STEADY = "steady schreck";
            internal const string BAZOOKA = "bazooka";
            internal const string BAZOOKA_STEADY = "steady bazooka";
            internal const string DISCLAIMER_BAZOOKA_ON = "Bazooka crossair enabled";
            internal const string DISCLAIMER_PANZERSCHRECK_ON = "Panzerschreck crossair enabled";
            internal const string DISCLAIMER_BAZOOKA_OFF = "Bazooka crossair disabled";
            internal const string DISCLAIMER_PANZERSCHRECK_OFF = "Panzerschreck crossair disabled";
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
            internal const string TASKBAR = "TaskBar";
        }

        internal static class Feature
        {
            internal const string CLOSE = "close";
            internal const string CONFIG = "config";
            internal const string ARTILLERY_USGER = "artillery_usger";
            internal const string ARTILLERY_RU = "artillery_ru";
            internal const string ARTILLERY_VALIDATE = "artillery_validate";
            internal const string ROCKETINDICATOR_GER = "rocket_ger";
            internal const string ROCKETINDICATOR_US = "rocket_us";
            internal const string ROCKETINDICATOR_START = "rocket_start";
            internal const string MAP_OPEN = "map_open";
            internal const string MAP_RECORD = "map_record";
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
