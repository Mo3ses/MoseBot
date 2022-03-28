using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;

namespace MoseBot.Comandos
{
    internal class ComandoFun : BaseCommandModule
    {
        
        [Command("ping")]
        [Description("Pong")]
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
    }
}
