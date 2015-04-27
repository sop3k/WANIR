using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WANIRPartners.Utils;

namespace WANIRPartners.ViewModels
{
    public interface IPageViewModel
    {
        String Name{ get; }

        IEnumerable<NamedCommand> Commands { get; }
    }
}
