using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.EventHandling;
using DSharpPlus.Interactivity.Extensions;
using MoseBot.Atributos;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MoseBot.Comandos
{
    internal class ComandoFun : BaseCommandModule
    {
        
        [Command("ping")]
        [Description("Pong")]
        [CategoriaNecessaria(ChannelCheckMode.Any, "Text Channels")]
        public async Task Ping(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Pong!").ConfigureAwait(false);
        }
        [Command("soma")]
        [Description("Soma de dois numeros")]
        public async Task Roll(CommandContext ctx,
            [Description("Primeiro numero")] int min,
            [Description("Segundo numero")] int max)
        {
            await ctx.Channel
                .SendMessageAsync($"A soma é: {min + max}")
                .ConfigureAwait(false);
        }
        [Command("resposta")]
        [Description("Devolve a respota com um Joinha")]
        public async Task Resposta(CommandContext ctx)
        {
            var interatividade = ctx.Client.GetInteractivity();

            var mensagem = await interatividade
                .WaitForMessageAsync(x => x.Channel == ctx.Channel)
                .ConfigureAwait(false);

            await ctx.Channel.SendMessageAsync(mensagem.Result.Content + " :thumbsup:");
        }
        [Command("enquete")]
        [Description("Cria uma enquete")]
        public async Task Poll(CommandContext ctx, TimeSpan duracao, params DiscordEmoji[] EmojiOptions)
        {
            var interatividade = ctx.Client.GetInteractivity();
            var options = EmojiOptions.Select(x => x.ToString());

            var PollEmbed = new DiscordEmbedBuilder
            {
                Title = "Enquete",
                Description = string.Join("", options)
            };

            var PollMesage = await ctx.Channel.SendMessageAsync(embed: PollEmbed).ConfigureAwait(false);
            
            foreach(var option in EmojiOptions)
            {
                await PollMesage.CreateReactionAsync(option).ConfigureAwait(false);
            }

            var result = await interatividade.CollectReactionsAsync(PollMesage, duracao).ConfigureAwait(false);
            var distincResult = result.Distinct();

            var results = distincResult.Select(x => $"{x.Emoji}: {x.Total}");

            await ctx.Channel.SendMessageAsync(string.Join("\n", results)).ConfigureAwait(false);
        }
    }
}
