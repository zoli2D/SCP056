using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using PlayerRoles;
using Exiled.API.Features.Roles;
using Exiled.CustomRoles.Events;
using CommandSystem;
using Exiled.CustomRoles.Commands;
using System;
using Exiled.API.Extensions;

namespace SCP056Plugin2.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class changeg : ICommand
    {
        public string Command { get; } = "changeg";

        public string[] Aliases { get; } = { "ChangeToGuard" };

        public string Description { get; } = "This command let 056 to change his apperance to a Guard";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if (!CustomRole.Get(56).Check(player))
            {
                response = "Nem vagy 056!";
                return false;
            }


            response = "A kinézeted megváltozott Létesítmény Őrre!";
            player.ChangeAppearance(RoleTypeId.FacilityGuard);
            player.CustomInfo = $"{player.Nickname}\nFacility Guard";
            return true;
        }
    }
}
