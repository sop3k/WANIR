using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

using MvvmFoundation.Wpf;

using WANIRPartners.Utils;

namespace WANIRPartners.ViewModels
{
    public abstract class PageViewModel : ObservableObject
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
        #region Ctor
        public ChildPageViewModel(PageViewModel parent)
                : base(parent.ViewController)
        {
            Parent = parent;
        }

        #endregion

        #region Methods
        public void Close()
        {
            ICommand change = Parent.ViewController.ChangePageCommand;
            change.Execute(Parent);
        }
        #endregion

        #region Properties
        public PageViewModel Parent { get; private set; }
        #endregion
    }
}
