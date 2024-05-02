using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DTO;
using SmartShopMobileApp.Helpers;
using SmartShopMobileApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        private bool _isEmailValid;
        public bool IsEmailValid
        {
            get { return _isEmailValid; }
            set
            {
                if (_isEmailValid != value)
                {
                    _isEmailValid = value;
                    OnPropertyChanged(nameof(IsEmailValid));
                    UpdateSignUpEnabled();
                }
            }
        }

        private bool _isPasswordValid;
        public bool IsPasswordValid
        {
            get { return _isPasswordValid; }
            set
            {
                if (_isPasswordValid != value)
                {
                    _isPasswordValid = value;
                    OnPropertyChanged(nameof(IsPasswordValid));
                    UpdateSignUpEnabled();
                }
            }
        }

        private void UpdateSignUpEnabled()
        {
            var a = IsEmailValid;
            var b = IsPasswordValid;

            IsSignUpEnabled = !string.IsNullOrWhiteSpace(FirstName) &&
                              !string.IsNullOrWhiteSpace(LastName) &&
                              !string.IsNullOrWhiteSpace(EmailAddress) &&
                              !string.IsNullOrWhiteSpace(Password) &&
                              IsEmailValid &&
                              IsPasswordValid;
        }

        private bool _isSignUpEnabled;
        public bool IsSignUpEnabled
        {
            get { return _isSignUpEnabled; }
            set
            {
                if (_isSignUpEnabled != value)
                {
                    _isSignUpEnabled = value;
                    OnPropertyChanged(nameof(IsSignUpEnabled));
                }
            }
        }

        [RelayCommand]
        private async Task SignUp()
        {
            try
            {
                _manageData.SetStrategy(new CreateData());

                string hashedPassword = HashPassword(Password);

                await _manageData.GetDataAndDeserializeIt<UserDTO>($"User/AddNewUser?emailAddress={EmailAddress}" +
                    $"&password={hashedPassword}&firstName={FirstName}&lastName={LastName}&birthdate={BirthDate}", "");

                await Shell.Current.GoToAsync("//LogInView");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static string HashPassword(string password)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }

    }
}
