using DiscordLoggerConsole.Classes;
using DiscordLoggerConsole.Commands.CheckBase;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
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

        [Command("backup"), AdminChecker(), Description("Backs up the configuration file and the database")]
        public async Task backup(CommandContext ctx)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var d = Directory.CreateDirectory("backups");
                File.Copy($"{settings.configname}.json", Path.Combine(d.FullName, $"{settings.configname}.{DateTime.Now.ToFileTime()}.json"));
                File.Copy($"{Program.settings.databasename}.db", Path.Combine(d.FullName, $"{Program.settings.databasename}.{DateTime.Now.ToFileTime()}.db"));
                stopwatch.Stop();
                await ctx.RespondAsync("", false, new DiscordEmbedBuilder
                {
                    Title = "Success",
                    Description = $"Successfully backed up {settings.configname}.json and {Program.settings.databasename}.db in {stopwatch.Elapsed.TotalMilliseconds} ms"
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

        [Command("refresh"), AdminChecker(), Description("Refreshes the current loaded configuration")]
        public async Task refresh(CommandContext ctx)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                if (File.Exists($"{settings.configname}.json"))
                    Program.settings = JsonConvert.DeserializeObject<settings>(await File.ReadAllTextAsync($"{settings.configname}.json"));
                else Program.settings = settings.GetDefault();
                if (!string.IsNullOrWhiteSpace(Program.settings.status))
                    await Program.client.UpdateStatusAsync(new DiscordActivity(Program.settings.status, Program.settings.statusmode), Program.settings.userstatus);
                else await Program.client.UpdateStatusAsync(null, Program.settings.userstatus);
                stopwatch.Stop();
                await ctx.RespondAsync("", false, new DiscordEmbedBuilder
                {
                    Title = "Success",
                    Description = $"Refreshed configuration in {stopwatch.Elapsed.TotalMilliseconds} ms"
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