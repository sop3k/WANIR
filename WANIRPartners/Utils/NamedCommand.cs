using System;

namespace WANIRPartners.Utils
{
    public class NamedCommand
    {
        public NamedCommand(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}
