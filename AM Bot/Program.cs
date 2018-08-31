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
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync()
        {
            DiscordSocketClient client = new DiscordSocketClient();
            string token = Config.token;
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
            if (message.Content.Equals("!am help") || message.Content.Equals("!am"))
            {
                await message.Channel.SendMessageAsync("도움말\n" +
                    "!am open - 익명 메시지를 보낼 수 있는 개인 메시지 창을 띄웁니다.");
            }
            else if (message.Content.Equals("!am open"))
            {
                await message.Author.SendMessageAsync("도움말\n" +
                    "이 곳에 치는 명령어 이외의 메시지들은 익명으로 지정된 채널에 전송됩니다." +
                    "처음 사용시 !am set을 통해서 설정을 해주세요!\n" +
                    "!am guild - 가입된 Guild의 코드들을 보여줍니다.\n" +
                    "!am channel [guild code] - 그 길드에 있는 채널 코드들을 보여줍니다.\n" +
                    "!am set [guild code] [channel code] - 익명 메시지를 보낼 길드와 채널을 설정합니다.");
            }
            else if (message.Content.Equals("!am guild"))
            {

            }
            else if (message.Content.IndexOf("!am channel ").Equals(0))
            {
            }
            else if (message.Content.IndexOf("!am set ").Equals(0))
            {
                await message.Author.SendMessageAsync("설정되었습니다.");
            }
            else if (!message.Content.IndexOf("!am").Equals(0))
            {
                // 익명 메시지
            }
            else
            {
                await message.Author.SendMessageAsync("없는 명령어 입니다.");
            }
        }
    }
}
