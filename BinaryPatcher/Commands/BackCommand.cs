using System;
using System.IO;

namespace BinaryPatcher.Commands
{
    public class BackCommand : MoveCommand
    {
        public override void ApplyToFileStream(Stream file) => file.Position = Math.Clamp(file.Position - offset, 0, file.Length);
    }
}