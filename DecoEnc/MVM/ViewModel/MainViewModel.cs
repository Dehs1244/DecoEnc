using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DecoEnc.Core;

namespace DecoEnc.MVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand CryptViewCommand { get; set; }
        public RelayCommand CryptFileViewCommand { get; set; }

        public HomeViewModel HomeVm { get; set; }
        public CryptViewModel CryptModel { get; set; }
        public CryptFileViewModel CryptFileModel { get; set; }
        public DecryptViewModel DecryptModel { get; set; }
        public DecryptFileViewModel DecryptFileModel { get; set; }

        private object _CurrentView;

        public object CurrentView
        {
            get => _CurrentView;
            set
            {
                _CurrentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            HomeVm = new HomeViewModel();
            CryptModel = new CryptViewModel();
            CryptFileModel = new CryptFileViewModel();
            DecryptModel = new DecryptViewModel();
            DecryptFileModel = new DecryptFileViewModel();

            HomeViewCommand = new RelayCommand(o =>
             {
                 CurrentView = HomeVm;
             });
            CryptViewCommand = new RelayCommand(o =>
            {
                CurrentView = CryptModel;
            });
            CryptFileViewCommand = new RelayCommand((o) =>
            {
                CurrentView = CryptFileModel;
            });
            DecryptModel.Command = new RelayCommand((o) =>{
                CurrentView = DecryptModel;
            });
            DecryptFileModel.Command = new RelayCommand((o) =>
            {
                CurrentView = DecryptFileModel;
            });
        }
    }
}
