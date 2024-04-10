using System;
using System.IO;

namespace BinaryPatcher.Commands
{
    using Exceptions;

    public class MoveCommand : Command
    {
        protected int offset;

        public override void ParseArguments(string[] arguments)
        {
            if (arguments.Length < 1)
            {
                throw new MissingArgumentsException();
            }

            try
            {
                offset = int.Parse(arguments[0], System.Globalization.NumberStyles.HexNumber);
            }
            catch (FormatException)
            {
                throw new InvalidArgumentException($"Could not parse argument value.");
            }

            if (offset < 0)
            {
                throw new InvalidArgumentException($"Offset must be a positive value.");
            }
        }

        public override void ApplyToFileStream(Stream file) => file.Position = Math.Clamp(file.Position + offset, 0, file.Length);
    }
}