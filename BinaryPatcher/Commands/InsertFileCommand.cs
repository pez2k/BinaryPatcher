using System.IO;

namespace BinaryPatcher.Commands
{
    using Exceptions;

    public class InsertFileCommand : Command
    {
        private string filename = "";
        private int maxLength = 0;

        public override void ParseArguments(string[] arguments)
        {
            if (arguments.Length < 1)
            {
                throw new MissingArgumentsException();
            }

            filename = arguments[0];

            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new InvalidArgumentException($"A file to insert must be provided.");
            }
            if (!File.Exists(filename))
            {
                throw new InvalidArgumentException($"The specified file could not be found: {filename}.");
            }

            if (arguments.Length > 1)
            {
                if (int.TryParse(arguments[1], System.Globalization.NumberStyles.HexNumber, null, out maxLength) && maxLength <= 0)
                {
                    throw new InvalidArgumentException($"Length to copy must be greater than zero.");
                }
            }
        }

        public override void ApplyToFileStream(Stream file)
        {
            if (string.IsNullOrWhiteSpace(filename) || !File.Exists(filename))
            {
                return;
            }

            using (FileStream insertedFile = new(filename, FileMode.Open, FileAccess.Read))
            {
                if (maxLength <= 0)
                {
                    insertedFile.CopyTo(file);
                }
                else
                {
                    byte[] buffer = new byte[maxLength];
                    insertedFile.Read(buffer);
                    file.Write(buffer);
                }
            }
        }
    }
}