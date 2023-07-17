using System.Runtime.Serialization;

namespace Run.Models.Serializable
{
    [DataContract(Name = "options")]
    internal sealed class ConfigOption
    {
        [DataMember(Name = "language")]
        internal int Language { get; set; } = 0;

        [DataMember(Name = "check_updates")]
        internal bool CheckUpdates { get; set; } = false;

        [DataMember(Name = "disable_startup_message")]
        internal bool DisableStartupMessage { get; set; } = false;

        [DataMember(Name = "devmode")]
        internal bool DevMode { get; set; } = false;

        [DataMember(Name = "show_nohold_values")]
        internal bool ShowNoHoldValues { get; set; } = false;

        [DataMember(Name = "opacity")]
        internal double Opacity { get; set; } = 100;

        // METHODS USING THEM NOT USED
        [DataMember(Name = "inverted_mouse")]
        internal bool InvertedMouse { get; set; } = false;

        [DataMember(Name = "steady_aim")]
        internal bool SteadyAim { get; set; } = false;

        [DataMember(Name = "schreck_zook")]
        internal bool SchreckZook { get; set; } = false;

        [DataMember(Name = "convert_indicator")]
        internal bool ConvertIndicator { get; set; } = false;
    }
}
