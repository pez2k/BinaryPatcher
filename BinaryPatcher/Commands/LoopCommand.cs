using System;
using System.IO;

namespace BinaryPatcher.Commands
{
    using Exceptions;

    public class LoopCommand : Command
    {
        public int Count { get; set; }

        public override void ParseArguments(string[] arguments)
        {
            if (arguments.Length < 1)
            {
                throw new MissingArgumentsException();
            }

            int count;
            try
            {
                count = int.Parse(arguments[0], System.Globalization.NumberStyles.HexNumber);
            }
            catch (FormatException)
            {
                throw new InvalidArgumentException($"Could not parse argument value.");
            }

            if (count < 1)
            {
                throw new InvalidArgumentException($"Count must be greater than zero.");
            }

            Count = count;
        }

        public override void ApplyToFileStream(Stream file)
        {
            // Special case handled by command runner, so no-op
        }
    }
}