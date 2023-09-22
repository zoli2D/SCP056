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

        

    }
}