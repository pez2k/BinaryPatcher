using System;
using System.IO;

namespace BinaryPatcher.Commands
{
    public class SubtractCommand : AddCommand
    {
        public override void ApplyToFileStream(Stream file)
        {
            byte currentByte = (byte)file.ReadByte();
            if (currentByte - value < byte.MinValue)
            {
                Console.WriteLine("Warning: Decrementing byte would cause underflow, skipping.");
                return;
            }
            currentByte -= value;
            file.Position--;
            file.WriteByte(currentByte);
        }
    }
}