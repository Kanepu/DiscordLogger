using DiscordLoggerConsole.Classes;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordLoggerConsole.Commands.CheckBase
{
    [AttributeUsage(AttributeTargets.Method)]
    public partial class AdminChecker : CheckBaseAttribute
    {
        public override async Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
        {
            if (settings.admins.Contains(ctx.Message.Author.Id))
                return true;
            return false;
        }
    }
}
