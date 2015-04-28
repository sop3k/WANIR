using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using MvvmFoundation.Wpf;
using WANIRPartners.Utils;

namespace WANIRPartners.ViewModels
{
    public class ApplicationViewModel : ObservableObject, IViewController
    {
        #region Fields

        private ICommand _changePageCommand;

        private PageViewModel _currentPageViewModel;
        private ObservableCollection<PageViewModel> _pageViewModels;

        #endregion

        public ApplicationViewModel()
        {
            // Add available pages
            PageViewModels.Add(new PartnersViewModel(this));
            PageViewModels.Add(new ProjectsViewModel(this));
            PageViewModels.Add(new SettingsViewModel(this));

            // Set starting page
            CurrentPageViewModel = PageViewModels[0];
        }

        #region Properties / Commands

        public ICommand ChangePageCommand
        {
            get
            {
                if (_changePageCommand == null)
                {
                    _changePageCommand = new RelayCommand<PageViewModel>(
                        p => ChangeViewModel((PageViewModel)p),
                        p => p is PageViewModel);
                }

                return _changePageCommand;
            }
        }

        public ObservableCollection<PageViewModel> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new ObservableCollection<PageViewModel>();

                return _pageViewModels;
            }
        }

        public ObservableCollection<NamedCommand> CurrentPageCommands
        {
            get { return new ObservableCollection<NamedCommand>(CurrentPageViewModel.Commands); }
        }

        public PageViewModel CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;

                    RaisePropertyChanged("CurrentPageViewModel");
                    RaisePropertyChanged("CurrentPageCommands");
                }
            }
        }

        #endregion

        #region Methods

        private void ChangeViewModel(PageViewModel viewModel)
        {
            CurrentPageViewModel = viewModel;
        }

        #endregion
    }
}
