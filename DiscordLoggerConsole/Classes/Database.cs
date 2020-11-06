using SQLite;
using System;

namespace DiscordLoggerConsole.Classes
{
    public class Database
    {
        [Table("MessageData")]
        public class MessageData
        {
            [Column("date")]
            public string date { get; set; }

            [PrimaryKey]
            [Column("datetime")]
            public DateTime datetime { get; set; }

            [Column("author")]
            public string author { get; set; }

            [Column("authorid")]
            public string authorid { get; set; }

            [Column("isbot")]
            public bool isbot { get; set; }

            [Column("messageid")]
            public string messageid { get; set; }

            [Column("channel")]
            public string? channel { get; set; }

            [Column("channelid")]
            public string? channelid { get; set; }

            [Column("guild")]
            public string? guild { get; set; }

            [Column("guildid")]
            public string? guildid { get; set; }

            [Column("attachmentscount")]
            public int attachmentscount { get; set; }

            [Column("attachmentsextension")]
            public string? attachmentsextension { get; set; }

            [Column("attachmentsbase")]
            public string? attachmentsbase { get; set; }

            [Column("embedcount")]
            public int embedcount { get; set; }

            [Column("isedited")]
            public bool isedited { get; set; }

            [Column("isdeleted")]
            public bool isdeleted { get; set; }

            [Column("afteredit")]
            public string? afteredit { get; set; }

            [Column("content")]
            public string? content { get; set; }
        }
    }
}
