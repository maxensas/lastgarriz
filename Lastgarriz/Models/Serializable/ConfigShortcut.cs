using System.Runtime.Serialization;

namespace Run.Models.Serializable
{
    [DataContract(Name = "shortcuts")]
    internal sealed class ConfigShortcut
    {
        [DataMember(Name = "enable")]
        internal bool Enable { get; set; } = false;

        [DataMember(Name = "modifier")]
        internal int Modifier { get; set; } = 0x0;

        [DataMember(Name = "fonction")]
        internal string Fonction { get; set; } = null;

        [DataMember(Name = "keycode")]
        internal int Keycode { get; set; } = 0;

        [DataMember(Name = "value")]
        internal string Value { get; set; } = null;
    }
}
