using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using InstaKiller.Model;

namespace InstaKiller.Wpf
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly HttpClientWrapper _httpClient = new HttpClientWrapper("http://localhost:50870/");
        private string _userEmail;
        private string _userPassword;
        private Guid _userId;
        private Person _user;

        public Guid UserId
        {
            get { return _userId; }
            set { _userId = value; OnPropertyChanged();}
        }

        public string UserEmail
        {
            get { return _userEmail; }
            set { _userEmail = value; OnPropertyChanged();}
        }

        public string UserPassword
        {
            get { return _userPassword; }
            set { _userPassword = value; OnPropertyChanged();}
        }

        public ICommand GetUserByEmail
        {
            get
            {
                return new CommandWrapper((o) =>
                {
                    _user = _httpClient.GetUserByEmail(UserEmail);
                    UserId = _user.Id;
                    //_userId = _user.Id;
                }, o => true);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
