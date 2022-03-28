using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System.Threading.Tasks;

namespace MoseBot
{
    internal class ComandoRoles : BaseCommandModule
    {
        [Command("membro")]
        public async Task Join(CommandContext ctx)
        {
            var joinEmbed = new DiscordEmbedBuilder
            {
                Title = "Você quer ser membro?",
                Color = DiscordColor.Green,
                ImageUrl = ctx.Client.CurrentUser.AvatarUrl
            };

            var joinMessage = await ctx.Channel.SendMessageAsync(embed: joinEmbed).ConfigureAwait(false);

            var thumbsUpEmoji = DiscordEmoji.FromName(ctx.Client, ":+1:");
            var thumbsDownEmoji = DiscordEmoji.FromName(ctx.Client, ":-1:");

            await joinMessage.CreateReactionAsync(thumbsUpEmoji).ConfigureAwait(false);
            await joinMessage.CreateReactionAsync(thumbsDownEmoji).ConfigureAwait(false);

            var interatividade = ctx.Client.GetInteractivity();

            var resultado = await interatividade.WaitForReactionAsync(
            x => x.Message == joinMessage &&
            x.User == ctx.User &&
            (x.Emoji == thumbsUpEmoji || x.Emoji == thumbsDownEmoji)).ConfigureAwait(false);

            if (resultado.Result.Emoji == thumbsUpEmoji)
            {
                var role = ctx.Guild.GetRole(958030617340182538);
                await ctx.Member.GrantRoleAsync(role).ConfigureAwait(false);
            }

            await joinMessage.DeleteAsync().ConfigureAwait(false);
        }
    }
}