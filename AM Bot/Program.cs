using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace AM_Bot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        public async Task MainAsync()
        {
            DiscordSocketClient client = new DiscordSocketClient();
            string token = "";
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            client.MessageReceived += MessageReceived;
            client.Ready += Ready;
            await Task.Delay(-1);
        }

        private Task Ready()
        {
            return Task.CompletedTask;
        }

        private async Task MessageReceived(SocketMessage message)
        {
        }
    }
}
