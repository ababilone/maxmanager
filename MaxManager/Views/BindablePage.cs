using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MaxManager.ViewModels;

namespace MaxManager.Views
{
	public class BindablePage : Page
	{
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			var navigableViewModel = DataContext as INavigable;
			navigableViewModel?.Activate(e.Parameter);
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);

			var navigableViewModel = DataContext as INavigable;
			navigableViewModel?.Deactivate(e.Parameter);
		}
	}
}
