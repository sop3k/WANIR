using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

using NHibernate;

using MvvmFoundation.Wpf;
using WANIRPartners.Models;
using WANIRPartners.Utils;

namespace WANIRPartners.ViewModels
{
    public abstract class PageViewModel : ObservableObject, IDisposable
    {
        #region Ctor
        public PageViewModel(IViewController controller)
        {
            ViewController = controller;
        }
        #endregion

        #region Session
        public virtual ISessionFactory SessionFactory
        {
            get { return Boostrapper.SessionFactory; }
        }

        public virtual ISession Session
        {
            get
            {
                if (session == null)
                    session = SessionFactory.OpenSession();
                return session;
            }
        }
        #endregion

        #region Properties
        abstract public String ViewName { get; }

        public bool Active { get; set; }
        public bool NotActive { get { return !Active; } }

        abstract public ObservableCollection<NamedCommand> Commands { get; }

        virtual public ObservableCollection<NamedCommand> SpecificCommands
        {
            get;
            private set;
        }

        public IViewController ViewController { get; private set;}
        #endregion 

        #region Methods
        
        public void ShowView(PageViewModel view)
        {
            ICommand  changeCommand = ViewController.ChangePageCommand;
            changeCommand.Execute(view);
        }

        public void ShowView()
        {
            ICommand changeCommand = ViewController.ChangePageCommand;
            changeCommand.Execute(null);
        }

        public virtual void Dispose()
		{
			if(session!=null)
				session.Dispose();
        }
        #endregion

        #region Fields
        private ISession session;
        #endregion 

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
                RaisePropertyChanged("Type");
            }
        }

        public string Province
        {
            get { return _province; }
            set
            {
                _province = value;
                RaisePropertyChanged("Province");
                RaisePropertyChanged("Districts");
            }
        }

        public string District
        {
            get { return _district; }
            set
            {
                _district = value;
                RaisePropertyChanged("District");
            }
        }

        public string FullText
        {
            get { return _fulltext; }
            set
            {
                _fulltext = value;
                RaisePropertyChanged("FullText");
            }
        }

        public string Region
        {
            get { return _region; }
            set
            {
                _region = value;
                RaisePropertyChanged("Region");
            }
        }

        public IEnumerable<string> Districts
        {
            get
            {
                if (Province != null)
                {
                    foreach (var province in Province.Split(','))
                        foreach(var district in Const.Provinces[province])
                            yield return district;
                }
            }
        }
        public IEnumerable<string> Provinces
        {
            get { return Const.Provinces.Keys; }
        }
        public IEnumerable<string> Types
        {
            get { return Const.Types; }
        }
        public IEnumerable<string> Regions
        {
            get { return Const.Regions; }
        }
        public IEnumerable<int> Years
        {
            get { return Const.Years; }
        }
        public IEnumerable<string> YesNoNotSetChoices
        {
            get { return new List<string> { Const.NOT_SET, Const.YES_CAPTION, Const.NO_CAPTION }; }
        }

        private string _province;
        private string _district;
        private string _type;
        private string _name;
        private string _fulltext;
        private string _region;

        public void Activate()
        {
            Active = true;
            RaisePropertyChanged("Active");
            RaisePropertyChanged("NotActive");
        }
        public void Deactivate()
        {
            Active = false;
            RaisePropertyChanged("Active");
            RaisePropertyChanged("NotActive");
        }
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

        public override ISession Session
        {
            get {  return Parent.Session; }
        }
    }

    public abstract class CommonChildPageViewModel<T> : ChildPageViewModel
        where T: ObjectPopulator<T>, new()
    {
        #region Ctor

        public CommonChildPageViewModel(PageViewModel parent)
            : base(parent)
        {}

        #endregion
        public virtual void Save()
        {
            using(var tx = Session.BeginTransaction())
            {
                T obj = new T();
                obj.Populate(this);
                Session.SaveOrUpdate(obj);
                tx.Commit();
            }

            Close();
        }

        public void Cancel()
        {
            Close();
        }
    }
}
