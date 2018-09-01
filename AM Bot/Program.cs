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
            try
            {
                Config.Load();
                new Program().MainAsync().GetAwaiter().GetResult();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        public async Task MainAsync()
        {
            DiscordSocketClient client = new DiscordSocketClient();
            await client.LoginAsync(TokenType.Bot, Config.Token);
            await client.StartAsync();

            MessageReceiver receiver = new MessageReceiver(client);
            client.MessageReceived += receiver.MessageReceived;
            client.Ready += Ready;
            await Task.Delay(-1);
        }

        private Task Ready()
        {
            Console.WriteLine("[System] AM Bot is Ready!");
            return Task.CompletedTask;
        }
    }
}
