using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Threading.Tasks;

namespace DiscordLoggerConsole.Commands.CheckBase
{
    [AttributeUsage(AttributeTargets.Method)]
    public partial class OwnerCheck : CheckBaseAttribute
    {
        public override async Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
        {
            if (Program.settings.owner.Equals(ctx.Message.Author.Id))
                return true;
            return false;
        }
    }
}