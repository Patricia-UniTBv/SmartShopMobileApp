using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DTO;
using SmartShopMobileApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartShopMobileApp.ViewModels
{
    public partial class SignUpViewModel : ObservableObject
    {
        public SignUpViewModel()
        {
            _manageData = new ManageData();
        }

        private IManageData _manageData;
        public IManageData ManageData
        {
            get { return _manageData; }
            set { _manageData = value; }
        }

        [ObservableProperty]
        private string _firstName;

        [ObservableProperty]
        private string _lastName;

        [ObservableProperty]
        private string _emailAddress;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private DateTime birthDate;

        [RelayCommand]
        private async Task SignUp()
        {
            try
            {
                _manageData.SetStrategy(new CreateData());

                string hashedPassword = HashPassword(Password);

                await _manageData.GetDataAndDeserializeIt<UserDTO>($"User/AddNewUser?emailAddress={EmailAddress}&password={hashedPassword}&firstName={FirstName}&lastName={LastName}&birthdate={BirthDate}", "");

                Application.Current.MainPage = new NavigationPage(new LogInView());

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

    }
}
