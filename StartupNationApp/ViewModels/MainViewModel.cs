using Common.Model;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSVReader;
using GalaSoft.MvvmLight.Command;
using Common.Contracts;
using System.Windows;
using GalaSoft.MvvmLight.Threading;
using System.Threading;
using System.Reflection;
using System.IO;
using StartupNationApp.Utils;
using StartupNationApp.Utils.FIlter;
using System.Diagnostics;

namespace StartupNationApp.ViewModels
{
    public class MainViewModel: ViewModelBase
    {
        #region Ui Properties

        public ObservableCollection<Company> AllCompanies { get; set; }
        public ObservableCollection<Company> DisplayedCompanies { get; set; }
        public ObservableCollection<string> Messages { get; set; }
        private Company _selectedCompany;
        public Company SelectedCompany
        {
            get { return _selectedCompany; }
            set { Set(ref _selectedCompany, value); }
        }

        public string InputFile { get; set; }
        private int _numOfObjectsInConteiner;
        public int NumOfObjectsInConteiner
        {
            get
            {
                return _numOfObjectsInConteiner;
            }
            set
            {
                Set(ref _numOfObjectsInConteiner, value);
            }
        }
        private DateTime _loadCompaniesStartTime;
        public DateTime LoadCompaniesStartTime
        {
            get
            {
                return _loadCompaniesStartTime;
            }
            set
            {
                Set(ref _loadCompaniesStartTime, value);
            }
        }
        private TimeSpan _timeFormStart;
        private string _loadDuration;
        public string LoadDuration
        {
            get
            {
                return _loadDuration;
            }
            set
            {
                Set(ref _loadDuration, value);
            }
        }
        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                Set(ref _title, value);
            }
        }

        public FilterObject Filter { get; set; }
        private int _numOfFilteredObjects;
        public int NumOfFilteredObjects
        {
            get
            {
                return _numOfFilteredObjects;
            }
            set
            {
                Set(ref _numOfFilteredObjects, value);
            }
        }
        #endregion

        private IDataRetriver CompanyRetriver { get; set; }

        public MainViewModel(IMessageService messageService)
        {
            DispatcherHelper.Initialize();
            AllCompanies = new ObservableCollection<Company>();
            DisplayedCompanies = new ObservableCollection<Company>();
            Messages = new ObservableCollection<string>();

            IsRetriveInProgress = false;
            IsFilterSet = false;
            _messageService = messageService;
            _retriverFactory = new DataRetriverFactory(_messageService);

            Title = $"Mila's Startup nation! Version: {Assembly.GetExecutingAssembly().GetName().Version.ToString(3)}";

            Filter = new FilterObject()
            {
                LastFundedBeforeMonths = 6,
                Stages = FilterStage.CreateAll(),
                DealFlows = FilterDealFlow.CreateAll(),
                GotAtLeast  = 1.5f
            };

            RegisterEvents();
        }

        private void RegisterEvents()
        {
            _messageService.NewMessage += (s, message) =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() => Messages.Add(message));
            };
        }

        private void RegisterRetriveEvents()
        {
            CompanyRetriver.CompanyAdded += (s, company) =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    AllCompanies.Add(company);
                    if (IsFilterSet && CompanyFilter(company) || !IsFilterSet)
                    {
                        DisplayedCompanies.Add(company);
                        NumOfFilteredObjects++;
                    }
                });
                NumOfObjectsInConteiner++;
            };
        }

        #region Commands

        public RelayCommand OpenHomepageCommand
        {
            get
            {
                return new RelayCommand(() => Process.Start("chrome.exe", SelectedCompany.LinkToHomepage),
                    () => true);
            }
        }

        public RelayCommand OpenFinderCommand
        {
            get
            {
                return new RelayCommand(() => Process.Start("chrome.exe", SelectedCompany.LinkToFinder),
                    () => true);
            }
        }
        

        private RelayCommand _getCompaniesCommand;

        public RelayCommand GetCompaniesCommand
        {
            get
            {
                if (_getCompaniesCommand != null)
                {
                    return _getCompaniesCommand;
                }

                _getCompaniesCommand = new RelayCommand(
                    () =>
                    {
                        Task.Run(async () =>
                        {

                            if (string.IsNullOrEmpty(InputFile))
                            {
                                MessageBox.Show("Input file is empty");
                                return;
                            }

                            CompanyRetriver = _retriverFactory.GetRetriver(InputFile);
                            RegisterRetriveEvents();
                            using (var timer = StartGetMonitor())
                            {
                                await CompanyRetriver.StartGetCompanies(AllCompanies);
                            }

                            IsRetriveInProgress = false;
                            var message = string.Format("Finished!\nGot {0} startups.", NumOfObjectsInConteiner);
                            MessageBox.Show(message, "Info");
                        });
                    },
                    () => !IsRetriveInProgress);
                return _getCompaniesCommand;
            }
        }

        private Timer StartGetMonitor()
        {
            LoadCompaniesStartTime = DateTime.Now;
            IsRetriveInProgress = true;
            var timer = new Timer(_loadCompaniesStartTime =>
            {
                _timeFormStart = _timeFormStart.Add(TimeSpan.FromSeconds(1));
                LoadDuration = _timeFormStart.ToString();
            }, null, 0, 1000);
            return timer;
        }

        public RelayCommand Stop
        {
            get
            {
                return new RelayCommand(() => CompanyRetriver.StopGetCompanies(), () => true);
            }
        } 
        
        public RelayCommand<string> SaveToFileCommand
        {
            get
            {
                return new RelayCommand<string>(pathToOutputFile =>
                {
                    var pathToOutputFileWithSufix = FileSufix.CreateSufix(pathToOutputFile, SufixType.Startups);
                    if (IsRetriveInProgress)
                    {
                        MessageBox.Show("Can't save while retriving data. Stop retrive first!", "Error");
                        return;
                    }

                    if (File.Exists(pathToOutputFileWithSufix))
                    {
                        var result = MessageBox.Show("File already exists, replase it?", "Question", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.No)
                        {
                            return;
                        }
                    }
                    var writer = new CompanyWriter(pathToOutputFile);
                    writer.Write(AllCompanies);
                    MessageBox.Show("Finished writting file!", "Info");
                }, s => true);
            }
        }

        public RelayCommand ClearFilterCommand
        {
            get
            {
                return new RelayCommand(() => 
                {
                    if (!IsFilterSet)
                    {
                        return;
                    }

                    IsFilterSet = false;
                    DisplayedCompanies.Clear();
                    foreach (var f in AllCompanies)
                    {
                        DisplayedCompanies.Add(f);
                    }

                    CountFilteredObjects();
                }, () => true);
            }
        }

        private void CountFilteredObjects()
        {
            NumOfFilteredObjects = DisplayedCompanies.Count();
        }

        public RelayCommand FilterCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    IsFilterSet = true;
                    var filtered = AllCompanies.Where(c => CompanyFilter(c));

                    DisplayedCompanies.Clear();
                    foreach (var f in filtered)
                    {
                        DisplayedCompanies.Add(f);
                    }
                    CountFilteredObjects();
                }, () => true);
            }
        }

        private bool CompanyFilter(Company company)
        {
            return company.AmountRaisedInMilUsd > Filter.GotAtLeast &&
                Filter.Stages.Any(s=> s.Stage == company.Stage && s.IsSelected) &&
                Filter.DealFlows.Any(d => d.DealFlow == company.DealFlow && d.IsSelected) && 
                company.LastFunding.AddMonths(Filter.LastFundedBeforeMonths).CompareTo(DateTime.Now) < 0;
        }

        #endregion

        #region Fields

        public bool IsRetriveInProgress { get; private set; }
        private IMessageService _messageService;
        private DataRetriverFactory _retriverFactory;
        private bool _isFilterSet;
        public bool IsFilterSet
        {
            get
            {
                return _isFilterSet;
            }
            set
            {
                Set(ref _isFilterSet, value);
            }
        }

        #endregion
    }
}
