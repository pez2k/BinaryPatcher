using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BinaryPatcher
{
    using Commands;
    using Exceptions;

    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: BinaryPatcher <bpatch script> <target file>");
                return;
            }

            try
            {
                ApplyPatch(args[1], ReadPatchFile(args[0]));
            }
            catch (MissingArgumentsException exception)
            {
                Console.WriteLine($"{GetExceptionSource(exception)}: Missing arguments for command.");
            }
            catch (InvalidArgumentException exception)
            {
                Console.WriteLine($"{GetExceptionSource(exception)}: {exception.Message}");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"An error occurred:\r\n{exception.Message}\r\n{exception.InnerException?.Message}");
            }
        }

        private static Command[] ReadPatchFile(string filename)
        {
            List<Command> commands = new();
            using (FileStream patchFile = new(filename, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new(patchFile))
                {
                    while (!reader.EndOfStream)
                    {
                        Command? command = ParseCommand(reader.ReadLine());
                        if (command != null)
                        {
                            commands.Add(command);
                        }
                    }
                }
            }
            return commands.ToArray();
        }

        private static Command? ParseCommand(string? line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return null;
            }

            line = line.Replace('\t', ' ').Trim();
            string[] parts = line.Split(" ").Where(part => !string.IsNullOrWhiteSpace(part)).ToArray();

            if (parts.Length == 0)
            {
                return null;
            }

            Command? command = parts[0] switch
            {
                "g" => new GotoCommand(),         // Go to a specific address
                "m" => new MoveCommand(),         // Move cursor forwards by n bytes
                "b" => new BackCommand(),         // Move cursor backwards by n bytes
                "p" => new PatternMatchCommand(), // Find the specified pattern and move to the start of it
                "w" => new WriteCommand(),        // Overwrite bytes at cursor position
                "z" => new ZeroCommand(),         // Zero a given number of bytes at cursor position
                "l" => new LoopCommand(),         // Execute previous command n times
                "i" => new InsertFileCommand(),   // Insert the contents of a file at cursor position, overwriting
                _ => null
            };

            command?.ParseArguments(parts.Skip(1).ToArray());
            return command;
        }

        private static void ApplyPatch(string filename, Command[] commands)
        {
            using (FileStream targetFile = new(filename, FileMode.Open, FileAccess.ReadWrite))
            {
                Command? previousCommand = null;
                foreach (Command command in commands)
                {
                    if (command is LoopCommand loopCommand)
                    {
                        if (loopCommand.Count > 1 && previousCommand != null && previousCommand is not LoopCommand)
                        {
                            for (int i = 1; i < loopCommand.Count; i++)
                            {
                                previousCommand.ApplyToFileStream(targetFile);
                            }
                        }
                    }
                    else
                    {
                        command.ApplyToFileStream(targetFile);
                    }

                    previousCommand = command;
                }
            }
        }

        private static string GetExceptionSource(Exception exception) => exception.TargetSite?.DeclaringType?.Name ?? "Unknown";
    }
}