using System;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using Client.Navigation.Messages;
using Client.ViewModels.Login;
using Client.ViewModels.Shell;

namespace Client
{

    public interface ILoginConductor : IHandle<LoginMessage>
    {

    }

    [Export(typeof(ILoginConductor))]
    public class LoginConductor : ILoginConductor
    {

        #region Fields

        private readonly IWindowManager _windowManager;

        private static readonly ILog Logger = LogManager.GetLog(typeof(LoginConductor));


        #endregion Fields

        #region Constructors

        [ImportingConstructor]
        public LoginConductor(IEventAggregator events, IWindowManager windowManager)
        {
            _windowManager = windowManager;

            events.Subscribe(this);
        }

        #endregion Constructors

        #region Methods

        public void Handle(LoginMessage message)
        {
            try
            {
                LoginViewModel loginWindow = IoC.Get<LoginViewModel>();
                loginWindow.LoginSuccess += LoginWindow_LoginSuccess;

                _windowManager.ShowDialog(loginWindow);

            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        private void LoginWindow_LoginSuccess(object sender, EventArgs args)
        {
            try
            {
                IShellViewModel mainWindow = IoC.Get<IShellViewModel>();
                _windowManager.ShowWindow(mainWindow);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        #endregion Methods
    }
}