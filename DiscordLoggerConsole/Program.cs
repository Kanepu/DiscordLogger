using DiscordLoggerConsole.Classes;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SQLite;
using System;
using System.IO;
using System.Threading.Tasks;
using static DiscordLoggerConsole.Classes.Database;

namespace DiscordLoggerConsole
{
    class Program
    {
        public static SQLiteAsyncConnection database = new SQLiteAsyncConnection($"{settings.databasename}.db");
        public static DiscordClient client { get; set; }
        public static settings settings { get; set; }

        static void Main(string[] args)
        {
            new Program().StartBot().GetAwaiter().GetResult();
        }

        public async Task StartBot()
        {
            if (File.Exists($"{settings.configname}.json"))
                settings = JsonConvert.DeserializeObject<settings>(await File.ReadAllTextAsync($"{settings.configname}.json"));
            else settings = settings.GetDefault();

            if ((await database.GetTableInfoAsync("MessageData")).Count == 0)
                await database.CreateTableAsync<MessageData>();

            client = new DiscordClient(new DiscordConfiguration
            {
                Token = settings.token,
                AutoReconnect = true,
                ReconnectIndefinitely = true,
                MinimumLogLevel = LogLevel.Debug,
                Intents = DiscordIntents.All
            });

            var commands = client.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = settings.prefixes,
                EnableDms = true,
                CaseSensitive = false,
                DmHelp = true
            });
            
            if (!string.IsNullOrWhiteSpace(settings.status))
                client.Ready += async e =>
                {
                    await client.UpdateStatusAsync(new DSharpPlus.Entities.DiscordActivity(settings.status, settings.statusmode), settings.userstatus);
                };

            if (!string.IsNullOrWhiteSpace(settings.token))
                await client.ConnectAsync();
            else throw new Exception("Token is empty");
            await Task.Delay(-1);
        }
    }
}
