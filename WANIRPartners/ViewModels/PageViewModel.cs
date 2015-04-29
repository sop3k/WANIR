using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

using WANIRPartners.Utils;

namespace WANIRPartners.ViewModels
{
    public abstract class PageViewModel
    {
        #region Ctor
        public PageViewModel(IViewController controller)
        {
            ViewController = controller;
        }
        #endregion

        #region Properties
        abstract public String Name{ get; }

        abstract public IEnumerable<NamedCommand> Commands { get; }

        public IViewController ViewController { get; private set;}
        #endregion 

        #region Methods
        public void ShowView(PageViewModel view)
        {
            ICommand  changeCommand = ViewController.ChangePageCommand;
            changeCommand.Execute(view);
        }
        #endregion
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
