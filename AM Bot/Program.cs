using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace AM_Bot
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync()
        {
            DiscordSocketClient client = new DiscordSocketClient();

            // Write your bot token
            string token = Config.Token;

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            MessageReceiver receiver = new MessageReceiver(client);
            client.MessageReceived += receiver.MessageReceived;
            client.Ready += Ready;
            await Task.Delay(-1);
        }

        private Task Ready()
        {
            return Task.CompletedTask;
        }
    }
}
