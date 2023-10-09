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

namespace SCP056Plugin.Commands
{

    [CommandHandler(typeof(ClientCommandHandler))]
    public class change : ICommand
    {
        public string Command { get; } = "changed";

        public string[] Aliases { get; } = { "ChangeToClassD" };

        public string Description { get; } = "This command let 056 to change his apperance to a Class D";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);
            if (!CustomRole.Get(56).Check(player))
            {
                response = "You are not 056!";
                return false;
            }


            response = SCP056Plugin.Instance.Config.ClassDMessage;
            player.ChangeAppearance(RoleTypeId.ClassD);
            player.CustomInfo = $"{player.Nickname}\nClass D Personnel";
            return true;
        }
    }
}
