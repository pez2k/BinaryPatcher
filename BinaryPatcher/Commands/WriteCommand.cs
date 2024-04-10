using System;
using System.Collections.Generic;
using System.IO;

namespace BinaryPatcher.Commands
{
    using Exceptions;

    public class WriteCommand : Command
    {
        private byte[] data = Array.Empty<byte>();

        public override void ParseArguments(string[] arguments)
        {
            if (arguments.Length < 1)
            {
                throw new MissingArgumentsException();
            }

            List<byte> parsedData = new();
            foreach (string argument in arguments)
            {
                if (!byte.TryParse(argument, System.Globalization.NumberStyles.HexNumber, null, out byte parsedValue))
                {
                    break;
                }
                parsedData.Add(parsedValue);
            }

            if (parsedData.Count < 0)
            {
                throw new MissingArgumentsException();
            }

            data = parsedData.ToArray();
        }

        public override void ApplyToFileStream(Stream file) => file.Write(data);
    }
}