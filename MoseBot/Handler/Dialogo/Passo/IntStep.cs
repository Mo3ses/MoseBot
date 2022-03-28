using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using MoseBot.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoseBot.Handler.Dialogo.Passo
{
    internal class IntStep : DialogueStepBase
    {

        private readonly int? _minValue;
        private readonly int? _maxValue;
        private IDialogueStep _nextStep;

        public IntStep(
            string content,
            IDialogueStep nextStep,
            int? minValue = null,
            int? maxValue = null) : base(content)
        {
            _nextStep = nextStep;
            _minValue = minValue;
            _maxValue = maxValue;
        }

        public Action<int> OnValidResult { get; set; } = delegate { };

        public override IDialogueStep NextStep => _nextStep;

        public void SetNextStep(IDialogueStep nextStep)
        {
            _nextStep = nextStep;
        }

        public override async Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user)
        {
            var embedBuilder = new DiscordEmbedBuilder
            {
                Title = $"Por favor responda abaixo",
                Description = $"{user.Mention}, {_content}",
            };

            embedBuilder.AddField("Para parar o dialogo", "Use o comando ?cancel ");

            if (_minValue.HasValue)
            {
                embedBuilder.AddField("Valor minimo: ", $"{_minValue.Value}");
            }
            if (_maxValue.HasValue)
            {
                embedBuilder.AddField("Valor Maximo: ", $"{_maxValue.Value}");
            }

            var interactivity = client.GetInteractivity();

            while (true)
            {
                var embed = await channel.SendMessageAsync(embed: embedBuilder).ConfigureAwait(false);

                OnMessageAdded(embed);

                var messageResult = await interactivity.WaitForMessageAsync(
                    x => x.ChannelId == channel.Id && x.Author.Id == user.Id).ConfigureAwait(false);

                OnMessageAdded(messageResult.Result);

                if (messageResult.Result.Content.Equals("?cancel", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                if(!int.TryParse(messageResult.Result.Content, out int inputValue))
                {
                    if (inputValue < _minValue.Value)
                    {
                        await TryAgain(channel, $"Seu valor não é um inteiro").ConfigureAwait(false);
                        continue;
                    }
                }

                if (_minValue.HasValue)
                {
                    if (inputValue < _minValue.Value)
                    {
                        await TryAgain(channel, $"O valor digitado {inputValue} é menor que: {_minValue} tente novamente").ConfigureAwait(false);
                        continue;
                    }
                }
                if (_maxValue.HasValue)
                {
                    if (inputValue > _maxValue.Value)
                    {
                        await TryAgain(channel, $"O valor digitado {inputValue} é maior que: {_maxValue} tente novamente").ConfigureAwait(false);
                        continue;
                    }
                }

                OnValidResult(inputValue);

                return false;
            }
        }
    }
}