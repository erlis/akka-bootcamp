﻿using Akka.Actor;

namespace WinTail
{
    public class ValidationActor : UntypedActor
    {
        private readonly IActorRef _consoleWriteActor;

        public ValidationActor(IActorRef consoleWriteActor)
        {
            _consoleWriteActor = consoleWriteActor;
        }

        protected override void OnReceive(object message)
        {
            var msg = message as string;
            if (string.IsNullOrEmpty(msg))
            {
                _consoleWriteActor.Tell(new Messages.NullInputError("No input received."));
            }
            else
            {
                var valid = IsValid(msg);
                if (valid)
                {
                    _consoleWriteActor.Tell(new Messages.InputSuccess("Thank you Message was valid."));
                }
                else
                {
                    _consoleWriteActor.Tell(new Messages.ValidationError("Invalid input had odd number of characters."));
                }
            }

            Sender.Tell(new Messages.ContinueProcessing());
        }

        private static bool IsValid(string message)
        {
            return message.Length % 2 == 0;
        }

    }
}