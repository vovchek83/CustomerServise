using System;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using Caliburn.Micro;
using CustomersService.DataModel.Models;
using CustomersService.DataModel.Repositories.Interfaces;
using CustomersService.Infrastructure.Presentation.MVVM;

namespace Client.ViewModels.Login
{

    [Export(typeof(LoginViewModel))]
    public class LoginViewModel : ValidatableBindableBase
    {
        public event EventHandler LoginSuccess = delegate { };

        #region Data Members

        private readonly IUsersRepository _userRepo;
        private string _password = "Admin";
        private string _userName = "Admin";
        private string _errorText;

        private static readonly ILog Logger = LogManager.GetLog(typeof(LoginViewModel));

        #endregion

        #region Properties

        public override string DisplayName { get; set; } = "Customer Service";

        [Required]
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                SetProperty(ref _password, value);
                RiseCanLogin();
            }
        }

        [Required]
        public string UserName
        {
            get { return _userName; }
            set
            {
                SetProperty(ref _userName, value);
                RiseCanLogin();
            }
        }

        public string ErrorText
        {
            get { return _errorText; }
            set
            {
                _errorText = value;
                NotifyOfPropertyChange(() => ErrorText);
            }
        }

        #endregion

        #region Constructor

        [ImportingConstructor]
        public LoginViewModel(IUsersRepository userRepo)
        {
            _userRepo = userRepo;
        }

        #endregion

        #region Commands Handles

        public async void Login()
        {
            try
            {
                IsBusy = true;

                var user = await _userRepo.GetUserAsync(UserName);

                if (VarifyUserAccess(user))
                {
                    OnLoginSuccess();
                }

            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
            finally
            {
                IsBusy = false;
            }
        }


        public bool CanLogin
        {
            get
            {
                return !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password);
            } 
        }

        public void Cancel()
        {
            TryClose();
        }

        #endregion

        #region Private Methods
        private void OnLoginSuccess()
        {
            Logger.Info("Login Success");
            LoginSuccess(this, EventArgs.Empty);
            TryClose();
        }

        private bool VarifyUserAccess(User user)
        {
            if (user == null)
            {
                ErrorText = "User Not Exist";
                return false;
            }
            if (user.Password != Password)
            {
                ErrorText = "Incorrect Password";
                return false;
            }

            return true;
        }

        private void RiseCanLogin()
        {
            NotifyOfPropertyChange("CanLogin");
        }

        #endregion

    }
}
