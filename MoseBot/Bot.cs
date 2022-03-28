using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.Logging;
using MoseBot.Comandos;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MoseBot
{
    internal class Bot
    {
        public DiscordClient Client { get; private set; }
        public InteractivityExtension Interatividade { get; private set; }
        public CommandsNextExtension Comandos { get; private set; }
        public async Task RunAsync()
        {
            string json = string.Empty;

            using(Stream fs = File.OpenRead("config.json"))
            using(StreamReader sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);

            ConfigJson configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            DiscordConfiguration config = new DiscordConfiguration
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug
            };

            Client = new DiscordClient(config);

            Client.Ready += OnClientReady;

            Client.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromMinutes(3),

            });

            CommandsNextConfiguration ComandosConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] {configJson.Prefix},
                EnableMentionPrefix = true,
                EnableDms = false,
                DmHelp = true
            };

            Comandos = Client.UseCommandsNext(ComandosConfig);

            Comandos.RegisterCommands<ComandoFun>();
            Comandos.RegisterCommands<ComandoRoles>();

            await Client.ConnectAsync();

            await Task.Delay(-1);
        }

        private Task OnClientReady(object sender, ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }


    }
}
