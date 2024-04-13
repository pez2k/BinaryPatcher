using System;
using System.IO;

namespace BinaryPatcher.Commands
{
    using Exceptions;

    public class AddCommand : Command
    {
        protected byte value;

        public override void ParseArguments(string[] arguments)
        {
            if (arguments.Length < 1)
            {
                value = 1;
                return;
            }

            try
            {
                value = byte.Parse(arguments[0], System.Globalization.NumberStyles.HexNumber);
            }
            catch (FormatException)
            {
                throw new InvalidArgumentException($"Could not parse argument value.");
            }

            if (value <= 0)
            {
                throw new InvalidArgumentException($"Value must be greater than zero.");
            }
        }

        public override void ApplyToFileStream(Stream file)
        {
            byte currentByte = (byte)file.ReadByte();
            if (currentByte + value > byte.MaxValue)
            {
                Console.WriteLine("Warning: Incrementing byte would cause overflow, skipping.");
                return;
            }
            currentByte += value;
            file.Position--;
            file.WriteByte(currentByte);
        }
    }
}