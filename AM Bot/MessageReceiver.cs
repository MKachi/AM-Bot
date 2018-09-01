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
                await message.Channel.SendMessageAsync(Messages.Get("help"));
            }
            else if (message.Content.Equals("!am open"))
            {
                await message.Author.SendMessageAsync(Messages.Get("open"));
            }
            else if (message.Content.IndexOf("!am").Equals(0))
            {
                await message.Author.SendMessageAsync(Messages.Get("missingCommand"));
            }
        }

        private async void DirectMessage(SocketMessage message)
        {
            if (message.Content.Equals("!am help") || message.Content.Equals("!am"))
            {
                await message.Author.SendMessageAsync(Messages.Get("DM help"));
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
                        await message.Author.SendMessageAsync(Messages.Get("set"));
                        return;
                    }
                }
                await message.Author.SendMessageAsync(Messages.Get("setError"));
            }
            else if (!message.Content.IndexOf("!am").Equals(0))
            {
                if (_userSetting.ContainsKey(message.Author.Id))
                {
                    Console.WriteLine(message.Channel.Name);
                    Console.WriteLine("@" + message.Author.Username + "#" + message.Author.Discriminator);
                    SocketChannel channel = _client.GetChannel(_userSetting[message.Author.Id]);
                    var messageChannel = channel as IMessageChannel;
                    await messageChannel.SendMessageAsync(message.Content);
                }
                else
                {
                    await message.Author.SendMessageAsync(Messages.Get("setPlz"));
                }
            }
            else
            {
                await message.Author.SendMessageAsync(Messages.Get("missingCommand"));
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
