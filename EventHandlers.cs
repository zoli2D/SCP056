namespace SCP056Plugin
{
    using Exiled.API.Enums;
    using Exiled.API.Features;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs.Player;
    using Exiled.Events.EventArgs.Server;
    using PlayerRoles;
    using Exiled.API.Features.Roles;
    using Exiled.CustomRoles.Events;
  


    /// <summary>
    /// Handles general events for this _056Plugin.
    /// </summary>
    public class EventHandlers
    {
        private readonly SCP056Plugin _plugin;

        internal EventHandlers(SCP056Plugin plugin) => this._plugin = plugin;

        public int x = 0;
        public void OnJoin(JoinedEventArgs ev)
        {
            x = x + 1;
        }   
        public void OnLeft(LeftEventArgs ev)
        {
            x = x - 1;
        }


        private bool HasRoleSpawned = false;
        private int SpawnChance056 = 0;
        public void OnSpawned(SpawnedEventArgs ev)
        {
            if (Server.PlayerCount >= SCP056Plugin.Instance.Config.PlayerCount)
            {
                if (HasRoleSpawned == false)
                {
                    SpawnChance056 = UnityEngine.Random.Range(1, 101);
                    if (SpawnChance056 <= SCP056Plugin.Instance.Config.SpawnChance && HasRoleSpawned == false)
                    {
                        if (ev.Player.Role.Type == RoleTypeId.FacilityGuard)
                        {
                            HasRoleSpawned = true;
                            CustomRole.Get(typeof(Scp056Role)).AddRole(ev.Player);
                        }
                    }
                }
            }
        }

        public void OnDying(DyingEventArgs ev)
        {
            foreach (Player player in Player.Get(RoleTypeId.Tutorial))
            {
                if (CustomRole.Get(56).Check(player))
                {
                    Cassie.Message("SCP 0 5 6 has been terminated");
                }
            }
        }


        internal void OnEndingRound(EndingRoundEventArgs ev)
        {
            bool human = false;
            bool scps = false;
            CustomRole role = CustomRole.Get(typeof(Scp056Role));

            if (role == null)
            {
                Log.Debug($"{nameof(OnEndingRound)}: Custom role is null, returning.");
                return;
            }

            foreach (Player player in Player.List)
            {
                if (player == null)
                {
                    Log.Debug($"{nameof(OnEndingRound)}: Skipping a null player.");
                    continue;
                }

                if (role.Check(player) || player.Role.Side == Side.Scp)
                {
                    Log.Debug($"{nameof(OnEndingRound)}: Found an SCP player.");
                    scps = true;
                }
                else if (player.Role.Side == Side.Mtf || player.Role == RoleTypeId.ClassD)
                {
                    Log.Debug($"{nameof(OnEndingRound)}: Found a Human player.");
                    human = true;
                }

                if (scps && human)
                {
                    Log.Debug($"{nameof(OnEndingRound)}: Both humans and scps detected.");
                    break;
                }
                
            }

            Log.Debug($"{nameof(OnEndingRound)}: Should event be blocked: {(human && scps)} -- Should round end: {(human && scps)}");
            if (human && scps)
            {
                ev.IsRoundEnded = false;
            }
            HasRoleSpawned = false;
        }
    }
}