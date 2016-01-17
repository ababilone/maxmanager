using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using MaxManager.Views;
using MaxManager.Web.Lan.Discovery;
using Microsoft.Practices.ServiceLocation;

namespace MaxManager.ViewModels
{
	public class ViewModelLocator
	{
		private INavigationService CreateNavigationService()
		{
			var navigationService = new NavigationService();

			navigationService.Configure(NavigationKeys.Home, typeof(MainPage));
			navigationService.Configure(NavigationKeys.Discover, typeof(DiscoverPage));

			return navigationService;
		}

		public ViewModelLocator()
		{
			ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

			SimpleIoc.Default.Register(CreateNavigationService);

			if (ViewModelBase.IsInDesignModeStatic)
			{
				//SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
			}
			else
			{
				//SimpleIoc.Default.Register<IDataService, DataService>();
			}

			SimpleIoc.Default.Register<MaxCubeDiscoverer>();
			SimpleIoc.Default.Register<DiscoverViewModel>();
			SimpleIoc.Default.Register<MainViewModel>();
		}

		public MainViewModel Main => SimpleIoc.Default.GetInstance<MainViewModel>();
		public DiscoverViewModel Discover => SimpleIoc.Default.GetInstance<DiscoverViewModel>();
	}
}
