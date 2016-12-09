using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
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

        public ICommand SignInEmailAndPassword
        {
            get
            {
                return new CommandWrapper(
                    (o) =>
                    {
                        GetUserEmail();
                        GetUserPassword(o);
                        CheckUser();
                    }, o => true);
            }
        }

        private void CheckUser()
        {
            if (_user.PasswordHash.ToString() == _userPassword)
            {
                //go to next page logging sucseful
                UserEmail = "Success!";
            }
        }

        private void GetUserEmail()
        {
            _user = _httpClient.GetUserByEmail(UserEmail);
            UserId = _user.Id;
        }

        private void GetUserPassword(object param)
        {
            var param1 = param as PasswordBox;
            if (param1 != null)
            {
                UserPassword = param1.Password;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
