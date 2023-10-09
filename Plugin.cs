using System;
using Exiled.API.Features;

namespace SCP056Plugin
{
    using System.Collections.Generic;
    using Exiled.CustomItems.API;
    using Exiled.CustomItems.API.Features;
    using Exiled.CustomRoles.API;
    using Exiled.CustomRoles.API.Features;
    using HarmonyLib;
    using Exiled.API.Features.Roles;

    /// <inheritdoc />
    public class SCP056Plugin : Plugin<Config>
    {
        /// <summary>
        /// Static reference to the main instance of this class.
        /// </summary>
        public static SCP056Plugin Instance;

        /// <inheritdoc />
        public override string Author { get; } = "zoli2D";

        /// <inheritdoc />
        public override string Name { get; } = "Scp056";

        /// <inheritdoc />
        public override string Prefix { get; } = "Scp056";

        /// <inheritdoc />

        /// <summary>
        /// Gets the reference to this SCP056Plugin's Event Handler class.
        /// </summary>
        public EventHandlers EventHandlers { get; private set; }

        internal List<Player> StopRagdollsList = new();
        private Harmony _harmony;
        private string _harmonyId;

        /// <inheritdoc />
        public override void OnEnabled()
        {
            Instance = this;
            EventHandlers = new EventHandlers(this);
            Exiled.Events.Handlers.Server.EndingRound += EventHandlers.OnEndingRound;
            Exiled.Events.Handlers.Player.Spawned += EventHandlers.OnSpawned;

            _harmonyId = $"com.joker.035-{DateTime.Now.Ticks}";
            _harmony = new Harmony(_harmonyId);
            Log.Debug($"{nameof(OnEnabled)}: Patching..");
            _harmony.PatchAll();
            Log.Debug($"{nameof(OnEnabled)}: Registering item & role..");
            Config.Scp056RoleConfig.Register();
            base.OnEnabled();
        }

        /// <inheritdoc />
        public override void OnDisabled()
        {
            _harmony.UnpatchAll(_harmonyId);
            CustomItem.UnregisterItems();

            Exiled.Events.Handlers.Server.EndingRound -= EventHandlers.OnEndingRound;
            EventHandlers = null;
            Instance = null;

            base.OnDisabled();
        }
    }
}