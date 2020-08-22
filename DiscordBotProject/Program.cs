using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;

namespace DiscordBotProject
{
    public class Class1
    {
        public static void Main(string[] args)
        => new Class1().MainAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;

        private CommandHandler _handler;
        public async Task MainAsync() //loggin
        {
            _client = new DiscordSocketClient();
            _client.Log += Log;
            await _client.LoginAsync(TokenType.Bot,Environment.GetEnvironmentVariable("DiscordToken"));
            await _client.StartAsync();

            _handler = new CommandHandler(_client);

            // prevent from closing app
            await Task.Delay(-1);
        }
        private Task Log(LogMessage msg) //loggin msg
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

    }
}
