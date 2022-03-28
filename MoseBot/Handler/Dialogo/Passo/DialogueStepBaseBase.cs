using MoseBot.Interfaces;
using System;

namespace MoseBot.Handler.Dialogo
{
    public abstract class DialogueStepBaseBase
    {

        public IDialogueStep NextStep => throw new NotImplementedException();
    }
}