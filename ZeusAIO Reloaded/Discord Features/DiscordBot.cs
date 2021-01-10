using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZeusAIO.IO;

namespace ZeusAIO
{
    class DiscordBot
    {
        static List<string> pickedModulesNames1 = new List<string>();

        private DiscordSocketClient _client;
        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _client.MessageReceived += CommandHandler;
            var token = "tokenhereforbot";
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            await Task.Delay(-1);
        }

        private static Task CommandHandler(SocketMessage message)
        {
            string command = "";
            int lengthOfCommand = -1;


            if (!message.Content.StartsWith("!")) //This is your prefix
                return Task.CompletedTask;

            if (message.Author.IsBot) //This ignores all commands from bots
                return Task.CompletedTask;

            if (message.Content.Contains(" "))
                lengthOfCommand = message.Content.IndexOf(' ');
            else
                lengthOfCommand = message.Content.Length;

            command = message.Content.Substring(1, lengthOfCommand - 1).ToLower();

            //Commands begin here
            if (command.Equals("s"))  //Displaysstats
            {
                string DiscordREALID = message.Author.Id.ToString();
                if (Config.config.DiscordID == DiscordREALID)
                {
                    EmbedBuilder builder = new EmbedBuilder();

                    builder.WithTitle("ZeusAIO Reloaded");
                    builder.AddField("Hits", ZeusAIO.mainmenu.hits, true);
                    builder.AddField("Frees", ZeusAIO.mainmenu.frees, true);
                    builder.AddField("Bads", (ZeusAIO.mainmenu.checks - ZeusAIO.mainmenu.hits - ZeusAIO.mainmenu.frees), true);
                    builder.AddField("Checked", ZeusAIO.mainmenu.checks + "/" + ZeusAIO.mainmenu.comboTotal, true);
                    builder.AddField("Error", ZeusAIO.mainmenu.errors, true);
                    builder.AddField("Retries", ZeusAIO.mainmenu.realretries, true);
                    builder.AddField("Cpm", ZeusAIO.mainmenu.cpm * 60, true);
                    builder.AddField("Modules Selected", string.Join(", ", ZeusAIO.mainmenu.pickedModulesNames), true);
                    builder.WithColor(Color.Green);
                    message.Channel.SendMessageAsync("", false, builder.Build());
                }

            }
            return Task.CompletedTask;
        }
    }
}



