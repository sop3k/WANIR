using System;
using System.Windows.Input;

namespace WANIRPartners.Utils
{
    public class NamedCommand
    {
        public NamedCommand(string name, ICommand command)
        {
            Name = name;
            Command = command;
        }

        public string Name { get; private set; }
        public ICommand Command { get; private set; }
    }
}
