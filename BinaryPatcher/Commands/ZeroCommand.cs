using System;
using System.IO;

namespace BinaryPatcher.Commands
{
    using Exceptions;

    public class ZeroCommand : Command
    {
        private int count;

        public override void ParseArguments(string[] arguments)
        {
            if (arguments.Length < 1)
            {
                throw new MissingArgumentsException();
            }

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
        }

        public override void ApplyToFileStream(Stream file)
        {
            for (int i = 0; i < count; i++)
            {
                file.WriteByte(0);
            }
        }
    }
}