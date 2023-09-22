namespace SCP056Plugin
{
    using System.Collections.Generic;
    using System.Linq;
    using CustomPlayerEffects;
    using Exiled.API.Enums;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.API.Features.Attributes;
    using Exiled.API.Features.Items;
    using Exiled.API.Features.Pickups;
    using Exiled.API.Features.Spawn;
    using Exiled.CustomItems.API.Features;
    using Exiled.CustomRoles.API.Features;
    using Exiled.Events.EventArgs.Player;
    using MEC;
    using PlayerRoles;
    using PlayerStatsSystem;
    using UnityEngine;
    using VoiceChat;
    using YamlDotNet.Serialization;
    using RoleTypeId = PlayerRoles.RoleTypeId;
    using Exiled.Events.EventArgs.Cassie;
    using Exiled.CustomRoles.Events;

    /// <summary>
    /// The <see cref="CustomRole"/> handler for SCP-056.
    /// </summary>
    [CustomRole(RoleTypeId.FacilityGuard)]
    public class Scp056Role : CustomRole
    {
        /// <inheritdoc />
        public override uint Id { get; set; } = 56;

        /// <summary>
        /// Gets or sets the role that is visible to players on the server aside from the player playing this role.
        /// </summary>
        public RoleTypeId VisibleRole { get; set; } = RoleTypeId.FacilityGuard;

        /// <inheritdoc />
        public override int MaxHealth { get; set; } = 300;

        /// <inheritdoc />
        public override string Name { get; set; } = "SCP-056";

        /// <inheritdoc />
        public override string Description { get; set; } =
            "A player who looks like a normal person but actually it is an scp";

        /// <inheritdoc />
        public override string CustomInfo { get; set; } = "";

        /// <inheritdoc />
        public override bool KeepInventoryOnSpawn { get; set; } = false;

        /// <summary>
        /// Gets or sets a multiplier used to modify the player's movement speed (running and walking).
        /// </summary>
        public byte MovementMultiplier { get; set; } = 1;

        public override List<string> Inventory { get; set; } = new()
        {
            $"{ItemType.KeycardGuard}",
            $"{ItemType.GunFSP9}",
            $"{ItemType.Medkit}",
            $"{ItemType.Radio}",
            $"{ItemType.GrenadeFlash}",
            $"{ItemType.ArmorCombat}"
        };

        public override Dictionary<AmmoType, ushort> Ammo { get; set; } = new()
        {
            { AmmoType.Nato556, 100 },
            { AmmoType.Nato9, 200 },
        };


        /// <summary>
        /// Gets or sets the custom scale factor for players when they are this role.
        /// </summary>
        public override Vector3 Scale { get; set; } = new(1f, 1f, 1f);

        // The following properties are only defined so that we can add the YamlIgnore attribute to them so they cannot be changed via configs.
        /// <inheritdoc />
        [YamlIgnore]
        public override RoleTypeId Role { get; set; } = RoleTypeId.Tutorial;

        /// <inheritdoc />
        [YamlIgnore]
        public override List<CustomAbility> CustomAbilities { get; set; } = new();

        /// <inheritdoc />
        [YamlIgnore]
        public override SpawnProperties SpawnProperties { get; set; } = null;


        /// <inheritdoc />
        /// Hacky override to bypass bug in Exiled.CustomRoles
        public override void AddRole(Player player)
        {
            Vector3 oldPos = player.Position;
            Log.Debug(this.Name + ": Adding role to " + player.Nickname + ".");
            if (this.Role != RoleTypeId.None)
                player.Role.Set(this.Role, RoleSpawnFlags.None);
            Timing.CallDelayed(1.5f, (System.Action)(() =>
            {
                Vector3 spawnPosition = this.GetSpawnPosition();
                Log.Debug(string.Format("{0}: Found {1} to spawn {2}", (object)nameof(AddRole), (object)spawnPosition,
                    (object)player.Nickname));
                player.Position = oldPos;
                if (spawnPosition != Vector3.zero)
                {
                    Log.Debug("AddRole: Setting " + player.Nickname + " position..");
                    player.Position = spawnPosition + Vector3.up * 1.5f;
                }

                if (!this.KeepInventoryOnSpawn)
                {
                    Log.Debug(this.Name + ": Clearing " + player.Nickname + "'s inventory.");
                    player.ClearInventory();
                }

                foreach (string itemName in this.Inventory)
                {
                    Log.Debug(this.Name + ": Adding " + itemName + " to inventory.");
                    this.TryAddItem(player, itemName);
                }

                foreach (AmmoType key in this.Ammo.Keys)
                {
                    Log.Debug(string.Format("{0}: Adding {1} {2} to inventory.", (object)this.Name,
                        (object)this.Ammo[key], (object)key));
                    player.SetAmmo(key, this.Ammo[key]);
                }

                Log.Debug(this.Name + ": Setting health values.");
                player.Health = (float)this.MaxHealth;
                player.MaxHealth = (float)this.MaxHealth;
                player.Scale = this.Scale;
            }));
            Log.Debug(this.Name + ": Setting player info");
            player.CustomInfo = this.CustomInfo;
            player.InfoArea &= ~PlayerInfoArea.Role;
            if (this.CustomAbilities != null)
            {
                foreach (CustomAbility customAbility in this.CustomAbilities)
                    customAbility.AddAbility(player);
            }

            this.ShowMessage(player);
            this.RoleAdded(player);
            this.TrackedPlayers.Add(player);
            player.UniqueRole = this.Name;
            player.TryAddCustomRoleFriendlyFire(this.Name, this.CustomRoleFFMultiplier);
        }


        /// <inheritdoc />
        protected override void RoleAdded(Player player)
        {
            Timing.CallDelayed(0.5f, () =>
            {
                player.ChangeAppearance(VisibleRole);
                if (MovementMultiplier > 0)
                {
                    StatusEffectBase? movement = player.GetEffect(EffectType.MovementBoost);
                    movement.Intensity = MovementMultiplier;
                }

                player.IsGodModeEnabled = false;
            });

            player.Scale = Scale;
            player.VoiceChannel = VoiceChatChannel.Proximity;


            Timing.RunCoroutine(Appearance(player), $"{player.UserId}-appearance");

            base.RoleAdded(player);
        }

        /// <inheritdoc />
        protected override void RoleRemoved(Player player)
        {
            Timing.KillCoroutines($"{player.UserId}-appearance");
            player.Scale = Vector3.one;

            base.RoleRemoved(player);
        }





        private void OnDying(DyingEventArgs ev)
        {
            if (Check(ev.Player))
                SCP056Plugin.Instance.StopRagdollsList.Add(ev.Player);
                string message = $"SCP 0 5 6 has been successfully terminated .";
                Cassie.Message(message);

        }


        private void OnHurting(HurtingEventArgs ev)
        {
            if (ev.Attacker?.IsScp == true && ev.Player.Role.Type == RoleTypeId.Tutorial)
            {
                ev.IsAllowed = false;
            }
        }



        private IEnumerator<float> Appearance(Player player)
        {
            for (; ; )
            {
                yield return Timing.WaitForSeconds(5f);
                if(VisibleRole == RoleTypeId.FacilityGuard)
                {
                    player.CustomInfo = $"{player.Nickname}\nFacility Guard";
                }
                if (VisibleRole == RoleTypeId.Scientist)
                {
                    player.CustomInfo = $"{player.Nickname}\nScientist";
                }
                if (VisibleRole == RoleTypeId.ClassD)
                {
                    player.CustomInfo = $"{player.Nickname}\nClass D Personnel";
                }
                player.ReferenceHub.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.Nickname;
                player.ReferenceHub.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.Role;
                player.ReferenceHub.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.PowerStatus;
                player.ReferenceHub.nicknameSync.ShownPlayerInfo &= ~PlayerInfoArea.UnitName;
            }
        }
    }
}