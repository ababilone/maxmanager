using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Threading;
using MaxManager.ViewModels;

namespace MaxManager
{
    sealed partial class App
    {
	    private readonly MaxWorker _maxWorker;

	    public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;

		    _maxWorker = new MaxWorker(TimeSpan.FromSeconds(15));
        }

	    public ViewModelLocator Locator => Resources["Locator"] as ViewModelLocator;

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
			var rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                Window.Current.Content = new Views.Shell(rootFrame);
            }

			if (rootFrame.Content == null)
            {
                rootFrame.Navigate(typeof(Views.DiscoverPage), e.Arguments);
            }
            Window.Current.Activate();

			DispatcherHelper.Initialize();

			_maxWorker.Start();
		}

		void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

			_maxWorker.Stop();

			//TODO: Save application state and stop any background activity
			deferral.Complete();
        }
    }
}
