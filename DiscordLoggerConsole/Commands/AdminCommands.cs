using DiscordLoggerConsole.Classes;
using DiscordLoggerConsole.Commands.CheckBase;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DiscordLoggerConsole.Commands
{
    public class AdminCommands : BaseCommandModule
    {
        [Command("save"), AdminChecker(), Description("Saves the current configuration to the file")]
        public async Task save(CommandContext ctx)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                await File.WriteAllTextAsync($"{settings.configname}.json", JsonConvert.SerializeObject(Program.settings));
                stopwatch.Stop();
                await ctx.RespondAsync("", false, new DiscordEmbedBuilder
                {
                    Title = "Success",
                    Description = $"Successfully saved to {settings.configname}.json in {stopwatch.Elapsed.TotalMilliseconds} ms"
                }.WithFooter($"Replying to command: {ctx.Command.Name}"));
            }
            catch (Exception e)
            {
                await ctx.RespondAsync("", false, new DiscordEmbedBuilder
                {
                    Title = "Error",
                    Description = $"Error message: {e.Message}"
                }.WithFooter($"Replying to command: {ctx.Command.Name}"));
            }
        }
    }
}
