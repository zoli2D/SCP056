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
    public class changes : ICommand
    {
        public string Command { get; } = "changes";

        public string[] Aliases { get; } = { "ChangeToScientist" };

        public string Description { get; } = "This command let 056 to change his apperance to a Scientist";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if (!CustomRole.Get(56).Check(player))
            {
                response = "Nem vagy 056!";
                return false;
            }


            response = "A kinézeted megváltozott Tudóra!";
            player.ChangeAppearance(RoleTypeId.Scientist);
            player.CustomInfo = $"{player.Nickname}\nScientist";
            return true;
        }
    }
}
