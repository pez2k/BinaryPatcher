using System.IO;

namespace BinaryPatcher.Commands
{
    public abstract class Command
    {
        public abstract void ParseArguments(string[] arguments);
        public abstract void ApplyToFileStream(Stream file);
    }
}