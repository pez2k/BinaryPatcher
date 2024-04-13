using System;
using System.Collections.Generic;
using System.IO;

namespace BinaryPatcher.Commands
{
    using Exceptions;

    public class PatternMatchCommand : Command
    {
        private byte?[] pattern = Array.Empty<byte?>();

        public override void ParseArguments(string[] arguments)
        {
            if (arguments.Length < 1)
            {
                throw new MissingArgumentsException();
            }

            List<byte?> parsedData = new();
            foreach (string argument in arguments)
            {
                if (byte.TryParse(argument, System.Globalization.NumberStyles.HexNumber, null, out byte parsedValue))
                {
                    parsedData.Add(parsedValue);
                }
                else if (argument == "?" || argument == "??" || argument == "*" || argument == "**")
                {
                    parsedData.Add(null);
                }
                else
                {
                    break;
                }
            }

            if (parsedData.Count <= 0)
            {
                throw new MissingArgumentsException();
            }

            pattern = parsedData.ToArray();
        }

        public override void ApplyToFileStream(Stream file)
        {
            long position = file.Position;
            long originalPosition = position;
            int patternLength = pattern.Length;
            while (file.Position + patternLength <= file.Length)
            {
                bool matching = true;

                foreach (byte? item in pattern)
                {
                    byte currentByte = (byte)file.ReadByte();
                    if (item != null && currentByte != item)
                    {
                        matching = false;
                        break;
                    }
                }

                if (matching)
                {
                    file.Position = position;
                    return;
                }

                position++;
                file.Position = position;
            }

            Console.WriteLine("Warning: Pattern not matched.");
            file.Position = originalPosition;
        }
    }
}