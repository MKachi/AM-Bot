using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace AM_Bot
{
    public class MessageReceiver
    {
        private DiscordSocketClient _client = null;
        private Dictionary<ulong, ulong> _userSetting;

        public MessageReceiver(DiscordSocketClient client)
        {
            _client = client;
            _userSetting = new Dictionary<ulong, ulong>();
        }

        public async Task MessageReceived(SocketMessage message)
        {
            if (IsDM(message))
            {
                DirectMessage(message);
            }
            else if (message.Content.Equals("!am help") || message.Content.Equals("!am"))
            {
                await message.Channel.SendMessageAsync(
                    "도움말\n" +
                    "!am open - 익명 메시지를 보낼 수 있는 개인 메시지 창을 띄웁니다."
                );
            }
            else if (message.Content.Equals("!am open"))
            {
                await message.Author.SendMessageAsync(
                    "도움말\n" +
                    "이 곳에 치는 명령어 이외의 메시지들은 익명으로 지정된 채널에 전송됩니다.\n" +
                    "처음 사용시 !am set을 통해서 설정을 해주세요!\n" +
                    "!am code - 가입된 Guild의 channel 코드들을 보여줍니다.\n" +
                    "!am set [channel code] - 익명 메시지를 보낼 길드와 채널을 설정합니다.");
            }
            else if (message.Content.IndexOf("!am").Equals(0))
            {
                await message.Author.SendMessageAsync("없는 명령어 입니다.");
            }
        }

        private async void DirectMessage(SocketMessage message)
        {
            if (message.Content.Equals("!am help") || message.Content.Equals("!am"))
            {
                await message.Author.SendMessageAsync(
                    "도움말\n" +
                    "이 곳에 치는 명령어 이외의 메시지들은 익명으로 지정된 채널에 전송됩니다.\n" +
                    "처음 사용시 !am set을 통해서 설정을 해주세요!\n" +
                    "!am code - 가입된 Guild의 channel 코드들을 보여줍니다.\n" +
                    "!am set [channel code] - 익명 메시지를 보낼 길드와 채널을 설정합니다.");
            }
            else if (message.Content.Equals("!am code"))
            {
                StringBuilder sendText = new StringBuilder();
                foreach (var guild in _client.Guilds)
                {
                    sendText.Append("Guild : ").Append(guild.Name).Append("\n");
                    foreach (IChannel channel in guild.Channels)
                    {
                        if (!ReferenceEquals(channel.Name, null) && channel.GetType().Equals(typeof(SocketTextChannel)))
                        {
                            sendText.Append("   ").Append(channel.Name).Append(" - ").Append(channel.Id).Append("\n");
                        }
                    }
                }
                await message.Author.SendMessageAsync(sendText.ToString());
            }
            else if (message.Content.IndexOf("!am set ").Equals(0))
            {
                ulong id;
                if (ulong.TryParse(message.Content.Replace("!am set ", ""), out id))
                {
                    if (!ReferenceEquals(_client.GetChannel(id), null))
                    {
                        if (_userSetting.ContainsKey(message.Author.Id))
                        {
                            _userSetting[message.Author.Id] = id;
                        }
                        else
                        {
                            _userSetting.Add(message.Author.Id, id);
                        }
                        await message.Author.SendMessageAsync("설정되었습니다.");
                        return;
                    }
                }
                await message.Author.SendMessageAsync("코드를 다시 확인해주세요.");
            }
            else if (!message.Content.IndexOf("!am").Equals(0))
            {
                if (_userSetting.ContainsKey(message.Author.Id))
                {
                    Console.WriteLine(message.Channel.Name);
                    Console.WriteLine("@" + message.Author.Username + "#" + message.Author.Discriminator);
                    SocketChannel channel = _client.GetChannel(_userSetting[message.Author.Id]);
                    var messageChannel = channel as IMessageChannel;
                    await messageChannel.SendMessageAsync(Config.Name + message.Content);
                }
                else
                {
                    await message.Author.SendMessageAsync("!am set 명령어를 통해 채널을 설정해주세요");
                }
            }
            else
            {
                await message.Author.SendMessageAsync("없는 명령어 입니다.");
            }
        }

        private bool IsDM(SocketMessage message)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("@").Append(message.Author.Username).Append("#").Append(message.Author.Discriminator);
            return message.Channel.Name.Equals(sb.ToString());
        }
    }
}
