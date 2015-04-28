using System;
using System.Windows.Input;

namespace WANIRPartners.ViewModels
{
    public interface IViewController
    {
        ICommand ChangePageCommand { get; }
    }
}
