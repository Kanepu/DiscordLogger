

# DiscordLogger
DiscordLogger is a discord bot that listens to messages sent in chosen guilds and stores them in a database

Things it stores:

 - date
 - author's name
 - author id
 - is the user a bot
 - message id
 - channel's name
 - channel id
 - guild's name
 - guild id
 - amount of attachments
 - the attachment's extension (e.g. .png)
 - attachment (in base64 format)
 - amount of embeds
 - is the messaged edited
 - is the message deleted
 - message's contents after editing
 - message's contents

## DiscordLoggerConsole
This is the console version of the bot

### HOW TO SETUP
**Setting the token**
have your token ready (refer to [this](https://github.com/reactiflux/discord-irc/wiki/Creating-a-discord-bot-&-getting-a-token) guide to find it)
navigate to [config.json](https://github.com/Kanepu/DiscordLogger/blob/master/DiscordLoggerConsole/config.json) in the project and set the [token](https://github.com/Kanepu/DiscordLogger/blob/master/DiscordLoggerConsole/config.json#L8) to your token like this

BEFORE:
![BEFORE](https://i.ibb.co/FxrtX7Z/Screenshot-2020-11-07-143234.png)

AFTER (token was covered):
![AFTER](https://i.ibb.co/SNTgSkK/Screenshot-2020-11-07-144307.png)

**Setting the owner**
have your discord user id ready (refer to [this](https://support.discord.com/hc/en-us/articles/206346498-Where-can-I-find-my-User-Server-Message-ID-) guide to find it)
navigate to [config.json](https://github.com/Kanepu/DiscordLogger/blob/master/DiscordLoggerConsole/config.json) in the project and set the [owner](https://github.com/Kanepu/DiscordLogger/blob/master/DiscordLoggerConsole/config.json#L3) to your user id like this

BEFORE:
![BEFORE](https://i.ibb.co/kg38ZQz/Screenshot-2020-11-07-145001.png)

AFTER:
![AFTER](https://i.ibb.co/ct6L4pd/Screenshot-2020-11-07-150350.png)

Build and run your bot and it should now function properly

### HOW TO USE THE BOT
The prefix is set to ! by default all commands must be placed after that or the bot will not detect them 
for example the command "rows" will work if you place ! before it like this "!rows"

Commands that are in **bold** can only be executed by the owner

**Commands:**

 - save: This saves the configuration currently stored by the application (this will overwrite any changes made to the config file that were not updated in the bot itself (not refreshed))
 - backup: This creates a new folder with the name "backups" and copies the current configuration file along with the database there in this format "(config file name).(timestamp).json" and "(database file name).(timestamp).db"
 - refresh: This refreshes the current loaded config in the bot (used to update the bot after any changes have been done to the .json file however do note that changes in the token, status, statusmode, userstatus, databasename, and prefixes will not be updated without restarting the bot (this might change in future updates)
 - rows: This fetches the amount of records stored by the bot in the database
 - recreateattachment: This will check if the given message is stored and has a valid attachment then will recreate the attachment and send it to the user (this only accept message ids you can refer to [this](https://support.discord.com/hc/en-us/articles/206346498-Where-can-I-find-my-User-Server-Message-ID-) guide to find message ids)
 - filterbyauthor: This fetches records by a given user id (refer to [this](https://support.discord.com/hc/en-us/articles/206346498-Where-can-I-find-my-User-Server-Message-ID-) guide to find user ids) and sends them as a .gz file that can be extracted by the user to read the .txt file with the records
 - filterbydate: This fetches records by a given date (the format is DD/MM/YYYY) and sends them as a .gz file that can be extracted by the user to read the .txt file with the records
 - filterbymessage: This fetches a record by a given message id (refer to [this](https://support.discord.com/hc/en-us/articles/206346498-Where-can-I-find-my-User-Server-Message-ID-) guide to find message ids) and sends it as a .gz file that can be extracted by the user to read the .txt file with the record (this would only fetch one record as more than one record with the same message id can not exist)
 - filterbycontents: This fetches records by a given keyword and sends them as a .gz file that can be extracted by the user to read the .txt file with the records
 - addguilds: This adds a guild id to the list of guilds that the bot would listen to (this does not make the bot join a server it must be added)
 - listguilds: This lists all of the guild ids for the guilds it is listening to
 - removeguilds: This removes a guild id from the list of guilds that the bot would listen to (this does not make the bot leave a server)
 - **addadmin**: This adds admins to the admin list (users that can use commands)
 - **listadmins**: This lists all of the admin ids present in the list
 - **removeadmin**: This removes an admin by their id

### Configuration
Default config:
```json
 {
  "admins": [],
  "owner": 0,
  "guildstostalk": [],
  "prefixes": [
    "!"
  ],
  "token": "",
  "databasename": "main",
  "status": "",
  "statusmode": 0,
  "userstatus": "online"
}
```

**admins**: list of admins (its easier to use the commands to edit that)

**owner**: this should be set to your discord user id before launching the bot (some commands will only allow the owner to execute them)
#### [How to get my user id](https://support.discord.com/hc/en-us/articles/206346498-Where-can-I-find-my-User-Server-Message-ID-)

**guildstostalk**: list of guilds that the bot will be listening to for messages (its easier to use the commands to edit that)

**prefixes**: this contains the list of prefixes (characters placed before command names that the bot would listen to)

**token**: must be set to your bot's token ([how to get my bot's token](https://github.com/reactiflux/discord-irc/wiki/Creating-a-discord-bot-&-getting-a-token))

**databasename**: this controls the name of the .db file that the bot creates (changing this will make the bot start from a new database with no records)

**status**: this is what is displayed after your status mode for example if your statusmode was 0 and you set this to "discord" the bot's discord status would be "Playing discord"

**statusmode**: this controls what discord puts before your status (e.g. "Playing") refer to list for the values
``` c#
        //
        // Summary:
        //     Indicates the user is playing a game.
        Playing = 0,
        //
        // Summary:
        //     Indicates the user is streaming a game.
        Streaming = 1,
        //
        // Summary:
        //     Indicates the user is listening to something.
        ListeningTo = 2,
        //
        // Summary:
        //     Indicates the user is watching something.
        Watching = 3
```
**userstatus**: controls the discord status of the bot (refer to [this](https://dsharpplus.emzi0767.com/api/DSharpPlus.Entities.UserStatus.html) for the names)
## DiscordLoggerWPF (NOT FINISHED)
This is the WPF version of the bot 




