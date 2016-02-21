using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight.Views;
using MaxManager.Views;

namespace MaxManager.Services
{
	class NavigationService : INavigationService
	{
		private Frame RootFrame => (Window.Current.Content as Shell)?.ContentFrame;
		private readonly Dictionary<string, Type> _pageTypes;
		private readonly Dictionary<Type, string> _pageKeys; 
		 
		public NavigationService()
		{
			_pageTypes = new Dictionary<string, Type>();
			_pageKeys = new Dictionary<Type, string>();
		}

		public string CurrentPageKey => _pageKeys[RootFrame.Content?.GetType()];

		public void Configure(string pageKey, Type pageType)
		{
			_pageTypes.Add(pageKey, pageType);
			_pageKeys.Add(pageType, pageKey);
		}

		public void GoBack()
		{
			RootFrame.GoBack();
		}

		public void NavigateTo(string pageKey)
		{
			RootFrame.Navigate(_pageTypes[pageKey]);
		}

		public void NavigateTo(string pageKey, object parameter)
		{
			RootFrame.Navigate(_pageTypes[pageKey], parameter);
		}
	}
}
