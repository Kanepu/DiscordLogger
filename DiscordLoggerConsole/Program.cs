using DiscordLoggerConsole.Classes;
using DiscordLoggerConsole.Commands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SQLite;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using static DiscordLoggerConsole.Classes.Database;

namespace DiscordLoggerConsole
{
    internal class Program
    {
        public static SQLiteAsyncConnection database;
        public static DiscordClient client { get; set; }
        public static settings settings { get; set; }

        private static void Main(string[] args)
        {
            new Program().StartBot().GetAwaiter().GetResult();
        }

        public async Task StartBot()
        {
            if (File.Exists($"{settings.configname}.json"))
                settings = JsonConvert.DeserializeObject<settings>(await File.ReadAllTextAsync($"{settings.configname}.json"));
            else
            {
                settings = settings.GetDefault();
                await File.WriteAllTextAsync($"{settings.configname}.json", JsonConvert.SerializeObject(settings, Formatting.Indented));
            }

            database = new SQLiteAsyncConnection($"{settings.databasename}.db");

            if ((await database.GetTableInfoAsync("MessageData")).Count == 0)
                await database.CreateTableAsync<MessageData>();

            client = new DiscordClient(new DiscordConfiguration
            {
                Token = settings.token,
                AutoReconnect = true,
                ReconnectIndefinitely = true,
                MinimumLogLevel = LogLevel.Debug
            });

            var commands = client.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = settings.prefixes,
                EnableDms = true,
                CaseSensitive = false,
                DmHelp = true
            });

            commands.RegisterCommands<AdminCommands>();
            commands.RegisterCommands<StalkingCommands>();
            commands.RegisterCommands<StalkingManagementCommands>();

            if (!string.IsNullOrWhiteSpace(settings.status))
                client.Ready += async e =>
                {
                    await client.UpdateStatusAsync(new DSharpPlus.Entities.DiscordActivity(settings.status, settings.statusmode), settings.userstatus);
                };

            client.MessageCreated += async e =>
            {
                if (e.Guild != null && settings.guildstostalk.Contains(e.Guild.Id))
                {
                    string con = e.Message.Content;
                    if (e.Message.Attachments.Count != 0)
                        foreach (var aa in e.Message.Attachments)
                            con += Environment.NewLine + aa.Url;
                    string channel = string.Empty;
                    string guild = string.Empty;
                    if (e.Channel.Equals(null) || string.IsNullOrWhiteSpace(e.Channel.Name))
                        channel = "DM CHANNEL";
                    else channel = e.Channel.Name;
                    if (e.Guild == null)
                        guild = "DM CHANNEL";
                    else guild = e.Guild.Name;
                    string base64of0attachment = null;
                    string extension0attachment = null;
                    if (!(e.Message.Attachments.Equals(null) || e.Message.Attachments.Count < 1))
                    {
                        base64of0attachment = Convert.ToBase64String(new WebClient().DownloadData(e.Message.Attachments[0].Url));
                        extension0attachment = e.Message.Attachments[0].Url.Substring(e.Message.Attachments[0].Url.LastIndexOf("."));
                    }
                    await database.InsertAsync(new MessageData()
                    {
                        date = DateTime.UtcNow.Day + "/" + DateTime.UtcNow.Month + "/" + DateTime.UtcNow.Year,
                        datetime = DateTime.UtcNow,
                        author = e.Author.Username,
                        authorid = e.Author.Id.ToString(),
                        isbot = e.Author.IsBot,
                        messageid = e.Message.Id.ToString(),
                        channel = channel,
                        guild = guild,
                        attachmentscount = e.Message.Attachments.Count,
                        attachmentsextension = extension0attachment,
                        attachmentsbase = base64of0attachment,
                        embedcount = e.Message.Embeds.Count,
                        isedited = e.Message.IsEdited,
                        isdeleted = false,
                        content = e.Message.Content,
                        guildid = e.Guild?.Id.ToString(),
                        channelid = e.Channel?.Id.ToString()
                    });
                }
            };

            client.MessageUpdated += async e =>
            {
                try
                {
                    string con = e.Message.Content;
                    if (e.Message.Attachments.Count != 0)
                        foreach (var aa in e.Message.Attachments)
                            con += Environment.NewLine + aa.Url;
                    string channel = e.Channel?.Name;
                    string guild = e.Guild?.Name;
                    string base64of0attachment = string.Empty;
                    if (!(e.Message.Attachments.Equals(null) || e.Message.Attachments.Count < 1))
                        base64of0attachment = Convert.ToBase64String(new WebClient().DownloadData(e.Message.Attachments[0].Url));
                    string content = string.Empty;
                    if (database.FindWithQueryAsync<MessageData>("select * from MessageData where messageid = ?", e.Message.Id.ToString()).Result.afteredit != null)
                        content += (await database.FindWithQueryAsync<MessageData>("select * from MessageData where messageid = ?", e.Message.Id.ToString())).afteredit + Environment.NewLine;
                    content += e.Message.Content;
                    await database.ExecuteAsync("UPDATE MessageData SET isedited = ?, afteredit = ? WHERE messageid = ?;", e.Message.IsEdited, content, e.Message.Id.ToString());
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }
            };

            client.MessageDeleted += async e =>
            {
                await database.ExecuteAsync("UPDATE MessageData SET isdeleted = ? WHERE messageid = ?;", true, e.Message.Id.ToString());
            };

            await client.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}