using GSApp.Core;


namespace GSApp.MVVM.ViewModel
{
    class MainViewModel : ObservebleObject
    {

        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand DiscoveryViewCommand { get; set; }  
        public RelayCommand AudioMixerViewCommand { get; set; }


        public HomeViewModel HomeVm { get; set; }
        public DiscoveryViewModel DiscoveryVm { get; set; }
        public AudiomixerViewModel AudiomixerVm { get; set; }
        
        
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

        }
    }
}
