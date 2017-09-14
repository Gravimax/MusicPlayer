/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:MVVMTest"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace MusicPlayer.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<ApplicationViewModel>();
            SimpleIoc.Default.Register<PlayerControlViewModel>();
            SimpleIoc.Default.Register<PlayListControlViewModel>();
        }

        public ApplicationViewModel ApplicationVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ApplicationViewModel>();
            }
        }

        public PlayerControlViewModel PlayerControlVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PlayerControlViewModel>();
            }
        }

        public PlayListControlViewModel PlayListControlVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PlayListControlViewModel>();
            }
        }

        public static void Cleanup()
        {
            ServiceLocator.Current.GetInstance<ApplicationViewModel>().Cleanup();
            ServiceLocator.Current.GetInstance<PlayerControlViewModel>().Cleanup();
            ServiceLocator.Current.GetInstance<PlayListControlViewModel>().Cleanup();
        }
    }
}