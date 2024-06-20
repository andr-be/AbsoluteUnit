namespace AbsoluteUnit.Program.Structures
{
    public record CommandGroup(
        Command CommandType,
        Dictionary<Flag, int> Flags,
        List<string> CommandArguments
    )
    {
        public override string ToString()
        {
            string commandString = $"Command:\t{CommandType}";

            if (CommandArguments.Count > 0)
            {
                commandString += "\nArguments:\t";
                for (int argNum = 0; argNum < CommandArguments.Count; argNum++)
                    commandString += $"[{argNum}] {CommandArguments[argNum]}, ";
            }

            if (Flags.Count > 0)
            {
                commandString += "\nFlags:\t\t";
                for (int flagNum = 0; flagNum < Flags.Count; flagNum++)
                    commandString += $"[{flagNum}] {Flags.Keys.ElementAt(flagNum)} : {Flags.Values.ElementAt(flagNum)}, ";
            }

            return commandString;
        }
    }
}
