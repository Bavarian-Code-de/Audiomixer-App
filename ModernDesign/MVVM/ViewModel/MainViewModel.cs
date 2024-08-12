using GSApp.Core;


namespace GSApp.MVVM.ViewModel
{
    class MainViewModel : ObservebleObject
    {

        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand DiscoveryViewCommand { get; set; }
        public RelayCommand AudioMixerViewCommand { get; set; }
        public RelayCommand SettingsViewCommand { get; set; }
        public RelayCommand HelpViewCommand { get; set; }


        public HomeViewModel HomeVm { get; set; }
        public DiscoveryViewModel DiscoveryVm { get; set; }
        public AudiomixerViewModel AudiomixerVm { get; set; }
        public SettingsViewModel SettingsVm {get;set;}
        public HelpViewModel HelpVm { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value;
                onPropertyChanged();
            }
        }

        public MainViewModel()
        {
            HomeVm =  new HomeViewModel();
            DiscoveryVm = new DiscoveryViewModel();
            AudiomixerVm = new AudiomixerViewModel();
            SettingsVm = new SettingsViewModel();
            HelpVm = new HelpViewModel();

            CurrentView = HomeVm;

            HomeViewCommand = new RelayCommand(x => 
            {
                CurrentView = HomeVm;
            });
            DiscoveryViewCommand = new RelayCommand(x =>
            {
                CurrentView = DiscoveryVm;
            });
            AudioMixerViewCommand = new RelayCommand(x =>
            {
                CurrentView = AudiomixerVm;
            });
            SettingsViewCommand = new RelayCommand(x =>
            {
                CurrentView = SettingsVm;
            });
            HelpViewCommand = new RelayCommand(x =>
            {
                CurrentView = HelpVm;
            });
        }
    }
}
