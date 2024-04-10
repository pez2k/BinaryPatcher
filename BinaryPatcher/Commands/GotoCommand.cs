using System;
using System.IO;

namespace BinaryPatcher.Commands
{
    using Exceptions;

    public class GotoCommand : Command
    {
        private int target;

        public override void ParseArguments(string[] arguments)
        {
            if (arguments.Length < 1)
            {
                throw new MissingArgumentsException();
            }

            try
            {
                target = int.Parse(arguments[0], System.Globalization.NumberStyles.HexNumber);
            }
            catch (FormatException)
            {
                throw new InvalidArgumentException($"Could not parse argument value.");
            }

            if (target < 0)
            {
                throw new InvalidArgumentException($"Target must be a positive value.");
            }
        }

        public override void ApplyToFileStream(Stream file) => file.Position = Math.Clamp(target, 0, file.Length);
    }
}