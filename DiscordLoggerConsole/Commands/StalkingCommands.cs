using DiscordLoggerConsole.Commands.CheckBase;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using static DiscordLoggerConsole.Classes.Database;

namespace DiscordLoggerConsole.Commands
{
    public class StalkingCommands : BaseCommandModule
    {
        //https://stackoverflow.com/a/1344242/14147191
        public static Random random = new Random();
        public static string RandomString(int length)
        {
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        //https://stackoverflow.com/a/11153588/14147191
        public static void CompressFile(string path)
        {
            FileStream sourceFile = File.OpenRead(path);
            FileStream destinationFile = File.Create(path + ".gz");
            byte[] buffer = new byte[sourceFile.Length];
            sourceFile.Read(buffer, 0, buffer.Length);
            using (GZipStream output = new GZipStream(destinationFile,
                CompressionMode.Compress))
                output.Write(buffer, 0, buffer.Length);
            sourceFile.Close();
            destinationFile.Close();
        }

        [Command("rows"), AdminChecker(), Description("Fetch row count from database")]
        public async Task rows(CommandContext ctx)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var x = await Program.database.ExecuteScalarAsync<int>("select count(*) from MessageData");
                stopwatch.Stop();
                await ctx.RespondAsync("", false, new DiscordEmbedBuilder
                {
                    Title = "Success",
                    Description = $"{await Program.database.ExecuteScalarAsync<int>("select count(*) from MessageData")} rows were fetched in {stopwatch.Elapsed.TotalMilliseconds} ms"
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

        [Command("recreateattachment"), AdminChecker(), Description("Recreate an attachment present in a message by a given id")]
        public async Task recreateattachmenet(CommandContext ctx, string messageid)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var all = await Program.database.FindWithQueryAsync<MessageData>("Select * From MessageData Where messageid = ?", messageid);
                if (string.IsNullOrWhiteSpace(all?.author))
                    throw new Exception("Message by that id was not found");
                if (all.attachmentscount < 1)
                    throw new Exception("Attachments base64 is null or empty");
                string afteredit = "not registered";
                if (!string.IsNullOrWhiteSpace(all.afteredit))
                    afteredit = all.afteredit;
                string attachmentbase = "not registered";
                if (!string.IsNullOrWhiteSpace(all.attachmentsbase))
                    attachmentbase = "present use !kan recreateattachment " + ctx.RawArgumentString.Trim();
                string guildid = "not registered";
                if (!string.IsNullOrWhiteSpace(all.guildid))
                    guildid = all.guildid;
                string channelid = "not registered";
                if (!string.IsNullOrWhiteSpace(all.channelid))
                    channelid = all.channelid;
                string content = "not registered";
                if (!string.IsNullOrWhiteSpace(all.content))
                    content = all.content;
                string filename = RandomString(10) + all.attachmentsextension;
                await File.WriteAllBytesAsync(filename, Convert.FromBase64String(all.attachmentsbase));
                stopwatch.Stop();
                await ctx.RespondWithFileAsync(filename, "", false, new DiscordEmbedBuilder
                {
                    Title = "Success",
                    Description = $"Message by {all.messageid} ID was fetched in {stopwatch.Elapsed.TotalMilliseconds} ms"
                }.WithFooter($"Replying to command: {ctx.Command.Name}"));
                File.Delete(filename);
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

        [Command("uploaddatabase"), AdminChecker(), Description("Upload all saved records")]
        public async Task uploaddatabase(CommandContext ctx)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var all = await Program.database.QueryAsync<MessageData>("Select * From MessageData");
                string filename = RandomString(10) + ".txt";
                foreach (var y in all)
                    await File.AppendAllTextAsync(filename, "[" + y.date + "] [" + y.datetime.ToString() + "] [MID:" + y.messageid + "] [" + y.guild + " | " + y.channel + "] " + y.author + " (ORIGINAL CONTENT):[" + y.content + "] (EDITED CONTENT):[ " + y.afteredit + "] (ATTACHMENTS COUNT):[" + y.attachmentscount + "] (EMBEDS COUNT):[" + y.embedcount + "] (ISEDITED):[" + y.isedited + "] (ISDELETED):[" + y.isdeleted + "]" + Environment.NewLine);
                CompressFile(filename);
                stopwatch.Stop();
                await ctx.RespondWithFileAsync($"{filename}.gz", "", false, new DiscordEmbedBuilder
                {
                    Title = "Success",
                    Description = $"File created in {stopwatch.Elapsed.TotalMilliseconds} ms"
                }.WithFooter($"Replying to command: {ctx.Command.Name}"));
                File.Delete($"{filename}.gz");
                File.Delete(filename);
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

        [Command("filterbyauthor"), AdminChecker(), Description("Fetch records for a specific author")]
        public async Task filterbyauthor(CommandContext ctx, string authorid)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var all = await Program.database.QueryAsync<MessageData>("Select * From MessageData Where authorid = ?", authorid);
                if (string.IsNullOrWhiteSpace(all?.Count.ToString()) && all?.Count < 1)
                    throw new Exception("Messages by that author were not found");
                string filename = RandomString(10) + ".txt";
                foreach (var y in all)
                    await File.AppendAllTextAsync(filename, "[" + y.date + "] [" + y.datetime.ToString() + "] [MID:" + y.messageid + "] [" + y.guild + " | " + y.channel + "] " + y.author + " (ORIGINAL CONTENT):[" + y.content + "] (EDITED CONTENT):[ " + y.afteredit + "] (ATTACHMENTS COUNT):[" + y.attachmentscount + "] (EMBEDS COUNT):[" + y.embedcount + "] (ISEDITED):[" + y.isedited + "] (ISDELETED):[" + y.isdeleted + "]" + Environment.NewLine);
                CompressFile(filename);
                stopwatch.Stop();
                await ctx.RespondWithFileAsync($"{filename}.gz", "", false, new DiscordEmbedBuilder
                {
                    Title = "Success",
                    Description = $"File created in {stopwatch.Elapsed.TotalMilliseconds} ms"
                }.WithFooter($"Replying to command: {ctx.Command.Name}"));
                File.Delete($"{filename}.gz");
                File.Delete(filename);
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

        [Command("filterbydate"), AdminChecker(), Description("Fetch records by a specific date")]
        public async Task filterbydate(CommandContext ctx, string date)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var all = await Program.database.QueryAsync<MessageData>("Select * From MessageData Where date = ?", date);
                if (string.IsNullOrWhiteSpace(all?.Count.ToString()) && all?.Count < 1)
                    throw new Exception("Messages by that date were not found");
                string filename = RandomString(10) + ".txt";
                foreach (var y in all)
                    await File.AppendAllTextAsync(filename, "[" + y.date + "] [" + y.datetime.ToString() + "] [MID:" + y.messageid + "] [" + y.guild + " | " + y.channel + "] " + y.author + " (ORIGINAL CONTENT):[" + y.content + "] (EDITED CONTENT):[ " + y.afteredit + "] (ATTACHMENTS COUNT):[" + y.attachmentscount + "] (EMBEDS COUNT):[" + y.embedcount + "] (ISEDITED):[" + y.isedited + "] (ISDELETED):[" + y.isdeleted + "]" + Environment.NewLine);
                CompressFile(filename);
                stopwatch.Stop();
                await ctx.RespondWithFileAsync($"{filename}.gz", "", false, new DiscordEmbedBuilder
                {
                    Title = "Success",
                    Description = $"File created in {stopwatch.Elapsed.TotalMilliseconds} ms"
                }.WithFooter($"Replying to command: {ctx.Command.Name}"));
                File.Delete($"{filename}.gz");
                File.Delete(filename);
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

        [Command("filterbymessage"), AdminChecker(), Description("Fetch record for a specific message")]
        public async Task filterbymessage(CommandContext ctx, string messageid)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var all = await Program.database.QueryAsync<MessageData>("Select * From MessageData Where messageid = ?", messageid);
                if (string.IsNullOrWhiteSpace(all?.Count.ToString()) && all?.Count < 1)
                    throw new Exception("Message by that id was not found");
                string filename = RandomString(10) + ".txt";
                foreach (var y in all)
                    await File.AppendAllTextAsync(filename, "[" + y.date + "] [" + y.datetime.ToString() + "] [MID:" + y.messageid + "] [" + y.guild + " | " + y.channel + "] " + y.author + " (ORIGINAL CONTENT):[" + y.content + "] (EDITED CONTENT):[ " + y.afteredit + "] (ATTACHMENTS COUNT):[" + y.attachmentscount + "] (EMBEDS COUNT):[" + y.embedcount + "] (ISEDITED):[" + y.isedited + "] (ISDELETED):[" + y.isdeleted + "]" + Environment.NewLine);
                CompressFile(filename);
                stopwatch.Stop();
                await ctx.RespondWithFileAsync($"{filename}.gz", "", false, new DiscordEmbedBuilder
                {
                    Title = "Success",
                    Description = $"File created in {stopwatch.Elapsed.TotalMilliseconds} ms"
                }.WithFooter($"Replying to command: {ctx.Command.Name}"));
                File.Delete($"{filename}.gz");
                File.Delete(filename);
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

        [Command("filterbycontents"), AdminChecker(), Description("Fetch records for a specific message by contents")]
        public async Task filterbycontents(CommandContext ctx, string contents)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var all = await Program.database.QueryAsync<MessageData>("Select * From MessageData");
                if (string.IsNullOrWhiteSpace(all?.Count.ToString()) && all?.Count < 1)
                    throw new Exception("Messages by that keyword were not found");
                string filename = RandomString(10) + ".txt";
                foreach (var y in all)
                    if ((!string.IsNullOrWhiteSpace(y.content) && y.content.Contains(contents)) || (!string.IsNullOrWhiteSpace(y.afteredit) && y.afteredit.Contains(contents)))
                        await File.AppendAllTextAsync(filename, "[" + y.date + "] [" + y.datetime.ToString() + "] [MID:" + y.messageid + "] [" + y.guild + " | " + y.channel + "] " + y.author + " (ORIGINAL CONTENT):[" + y.content + "] (EDITED CONTENT):[ " + y.afteredit + "] (ATTACHMENTS COUNT):[" + y.attachmentscount + "] (EMBEDS COUNT):[" + y.embedcount + "] (ISEDITED):[" + y.isedited + "] (ISDELETED):[" + y.isdeleted + "]" + Environment.NewLine);
                CompressFile(filename);
                stopwatch.Stop();
                await ctx.RespondWithFileAsync($"{filename}.gz", "", false, new DiscordEmbedBuilder
                {
                    Title = "Success",
                    Description = $"File created in {stopwatch.Elapsed.TotalMilliseconds} ms"
                }.WithFooter($"Replying to command: {ctx.Command.Name}"));
                File.Delete($"{filename}.gz");
                File.Delete(filename);
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