using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using MaxManager.Views;
using MaxManager.Web.Lan;
using MaxManager.Web.Lan.Discovery;
using MaxManager.Web.Lan.Merger;
using MaxManager.Web.Lan.Parser;
using Microsoft.Practices.ServiceLocation;

namespace MaxManager.ViewModels
{
	public class ViewModelLocator
	{
		private INavigationService CreateNavigationService()
		{
			var navigationService = new Services.NavigationService();
			
			navigationService.Configure(NavigationKeys.Home, typeof(MainPage));
			navigationService.Configure(NavigationKeys.Discover, typeof(DiscoverPage));

			return navigationService;
		}

		private IMaxConnector CreateMaxConnector()
		{
			var maxParser = new MaxParser();
			return new MaxConnector(maxParser);
		}

		private IMaxStateAnalyzer CreateMaxStateAnalyzer(IMaxConnector maxConnector)
		{
			var maxMerger = new MaxMerger();
			return new MaxStateAnalyzer(maxConnector, maxMerger);
		}

		public ViewModelLocator()
		{
			ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

			SimpleIoc.Default.Register(CreateNavigationService);

			var maxConnector = CreateMaxConnector();
			SimpleIoc.Default.Register(() => maxConnector);

			var maxStateAnalyzer = CreateMaxStateAnalyzer(maxConnector);
			SimpleIoc.Default.Register(() => maxStateAnalyzer);

			if (ViewModelBase.IsInDesignModeStatic)
			{
				//SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
			}
			else
			{
				//SimpleIoc.Default.Register<IDataService, DataService>();
			}

			SimpleIoc.Default.Register<IMaxCubeDiscoverer, MaxCubeDiscoverer>();
			SimpleIoc.Default.Register<DiscoverViewModel>();
			SimpleIoc.Default.Register<MainViewModel>();
			SimpleIoc.Default.Register<SettingsViewModel>();
		}

		public MainViewModel Main => SimpleIoc.Default.GetInstance<MainViewModel>();
		public DiscoverViewModel Discover => SimpleIoc.Default.GetInstance<DiscoverViewModel>();
		public SettingsViewModel Settings => SimpleIoc.Default.GetInstance<SettingsViewModel>();
	}
}
