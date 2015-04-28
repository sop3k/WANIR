using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

using WANIRPartners.Utils;

namespace WANIRPartners.ViewModels
{
    public abstract class PageViewModel
    {
        public PageViewModel(IViewController controller)
        {
            ViewController = controller;
        }

        abstract public String Name{ get; }

        abstract public IEnumerable<NamedCommand> Commands { get; }

        public IViewController ViewController { get; private set;}
    }

    public abstract class ChildPageViewModel : PageViewModel
    {
        public ChildPageViewModel(PageViewModel parent)
                : base(null)
        {
            Parent = parent;
        }

        public void Close()
        {
            ICommand change = Parent.ViewController.ChangePageCommand;
            change.Execute(Parent);
        }

        public PageViewModel Parent { get; private set; }
    }
}
