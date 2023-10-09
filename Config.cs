using System.ComponentModel;
using Exiled.API.Interfaces;

namespace SCP056Plugin
{
    /// <inheritdoc />
    public class Config : IConfig
    {
        /// <inheritdoc />
        [Description("Whether or not this _056Plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether debug messages will be shown.
        /// </summary>
        [Description("Whether or not to display debug messages in the server console.")]
        public bool Debug { get; set; }


        /// <summary>
        /// Role configs for 056.
        /// </summary>
        [Description("Configs for the SCP-056 role players turn into.")]
        public Scp056Role Scp056RoleConfig { get; set; } = new();

        [Description("The player count when 056 starts to spawn")]
        public int PlayerCount { get; set; } = 16;

        [Description("The spawn chance of the Role")]
        public int SpawnChance { get; set; } = 10;

        [Description("ConsoleMessage when 056 changes to ClassD")]
        public string ClassDMessage { get; set; } = "Your disguise has changed to: Class";

        [Description("ConsoleMessage when 056 changes to Scientist")]
        public string ScientistMessage { get; set; } = "Your disguise has changed to: Scientist";

        [Description("ConsoleMessage when 056 changes to Facility Guard")]
        public string GuardMessage { get; set; } = "Your disguise has changed to: Facility Guard";



    }
}