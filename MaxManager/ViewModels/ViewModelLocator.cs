using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using MaxManager.Web.Lan.Discovery;
using Microsoft.Practices.ServiceLocation;

namespace MaxManager.ViewModels
{
	public class ViewModelLocator
	{
		public ViewModelLocator()
		{
			ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

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
