using Client.ServiceProxy;
using Client.WpfApp.ViewModels;
using Client.WpfApp.Views;
using Common.Contract;
using Common.Contract.BookMgr;
using Microsoft.Practices.Unity;
using Prism.Unity;
using ServiceProxy;
using System.Windows;

namespace Client.WpfApp
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override void ConfigureServiceLocator()
        {
            base.ConfigureServiceLocator();

            Container.RegisterType<IFileService, FileServiceProxy>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IBookService, BookServiceProxy>(new ContainerControlledLifetimeManager());
        }

        protected override DependencyObject CreateShell()
        {
            var shellViewModel = Container.Resolve<ShellViewModel>();
            var shellView = Container.Resolve<ShellView>();
            shellView.DataContext = shellViewModel;

            return shellView;
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        private void TestGit()
        {
            // Add something from git branch: TestGit01
        }
    }
}
