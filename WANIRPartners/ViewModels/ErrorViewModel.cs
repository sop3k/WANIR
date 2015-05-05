using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using MvvmFoundation.Wpf;

using WANIRPartners.Models;
using WANIRPartners.Utils;

using NHibernate.Linq;

namespace WANIRPartners.ViewModels
{
    public class ErrorViewModel : ObservableObject
    {
        public ErrorViewModel(Exception ex)
        {
            _exception = ex;
        }

        public String ExceptionMessage
        {
            get
            {
                return _exception.InnerException != null ? _exception.InnerException.Message : _exception.Message;
            }
        }

        public String ExceptionStackTrace
        {
            get
            {
                return _exception.InnerException != null ? _exception.InnerException.StackTrace : _exception.StackTrace;
            }
        }


        public ICommand Close
        {
            get
            {
                return new RelayCommand(() => Environment.Exit(0));
            }
        }

        private Exception _exception;
    }
}
