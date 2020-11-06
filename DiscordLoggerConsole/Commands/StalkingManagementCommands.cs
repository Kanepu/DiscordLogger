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
    public class StalkingManagementCommands : BaseCommandModule
    {
        [Command("addguilds"), AdminChecker(), Description("Add a guild id to listen to")]
        public async Task addguilds(CommandContext ctx, string guildid)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                Program.settings.guildstostalk.Add(ulong.Parse(guildid));
                await File.WriteAllTextAsync($"{settings.configname}.json", JsonConvert.SerializeObject(Program.settings, Formatting.Indented));
                stopwatch.Stop();
                await ctx.RespondAsync("", false, new DiscordEmbedBuilder
                {
                    Title = "Success",
                    Description = $"Added {guildid} in {stopwatch.Elapsed.TotalMilliseconds} ms"
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

        [Command("listguilds"), AdminChecker(), Description("List all guilds in listening list")]
        public async Task listguilds(CommandContext ctx)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var embed = new DiscordEmbedBuilder
                {
                    Title = "Success"
                }.WithFooter($"Replying to command: {ctx.Command.Name}");
                foreach (var x in Program.settings.guildstostalk)
                    embed.AddField(Program.settings.guildstostalk.IndexOf(x).ToString(), x.ToString());
                stopwatch.Stop();
                await ctx.RespondAsync("", false, embed.WithDescription($"Fetched in {stopwatch.Elapsed.TotalMilliseconds} ms"));
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

        [Command("removeguilds"), AdminChecker(), Description("Remove a guild id from listening list")]
        public async Task removeguilds(CommandContext ctx, string guildid)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                Program.settings.guildstostalk.Remove(ulong.Parse(guildid));
                await File.WriteAllTextAsync($"{settings.configname}.json", JsonConvert.SerializeObject(Program.settings, Formatting.Indented));
                stopwatch.Stop();
                await ctx.RespondAsync("", false, new DiscordEmbedBuilder
                {
                    Title = "Success",
                    Description = $"Removed {guildid} in {stopwatch.Elapsed.TotalMilliseconds} ms"
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

        [Command("addadmin"), OwnerCheck(), Description("Add a user id to the admin list")]
        public async Task addadmin(CommandContext ctx, string userid)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                Program.settings.admins.Add(ulong.Parse(userid));
                await File.WriteAllTextAsync($"{settings.configname}.json", JsonConvert.SerializeObject(Program.settings, Formatting.Indented));
                stopwatch.Stop();
                await ctx.RespondAsync("", false, new DiscordEmbedBuilder
                {
                    Title = "Success",
                    Description = $"Added {userid} in {stopwatch.Elapsed.TotalMilliseconds} ms"
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

        [Command("listadmins"), OwnerCheck(), Description("List all admins")]
        public async Task listadmins(CommandContext ctx)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var embed = new DiscordEmbedBuilder
                {
                    Title = "Success"
                }.WithFooter($"Replying to command: {ctx.Command.Name}");
                foreach (var x in Program.settings.admins)
                    embed.AddField(Program.settings.admins.IndexOf(x).ToString(), x.ToString());
                stopwatch.Stop();
                await ctx.RespondAsync("", false, embed.WithDescription($"Fetched in {stopwatch.Elapsed.TotalMilliseconds} ms"));
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

        [Command("removeadmin"), OwnerCheck(), Description("Remove a user id from the admin list")]
        public async Task removeadmin(CommandContext ctx, string userid)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                Program.settings.admins.Remove(ulong.Parse(userid));
                await File.WriteAllTextAsync($"{settings.configname}.json", JsonConvert.SerializeObject(Program.settings, Formatting.Indented));
                stopwatch.Stop();
                await ctx.RespondAsync("", false, new DiscordEmbedBuilder
                {
                    Title = "Success",
                    Description = $"Removed {userid} in {stopwatch.Elapsed.TotalMilliseconds} ms"
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