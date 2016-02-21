using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using MaxManager.Commands;
using MaxManager.Model;

namespace MaxManager.Views
{
	public sealed partial class Shell : Page
	{
		private readonly Frame _contentFrame;

		public Shell(Frame frame)
		{
			_contentFrame = frame;
			InitializeComponent();
			ShellSplitView.Content = frame;

			var update = new Action(() =>
			{
				// update radiobuttons after frame navigates
				var type = frame.CurrentSourcePageType;
				foreach (var radioButton in AllRadioButtons(this))
				{
					var target = radioButton.CommandParameter as NavType;
					if (target == null)
						continue;
					radioButton.IsChecked = target.Type == type;
				}
				ShellSplitView.IsPaneOpen = false;
				BackCommand.RaiseCanExecuteChanged();
			});
			frame.Navigated += (s, e) => update();
			Loaded += (s, e) => update();
			DataContext = this;
		}

		private Command _backCommand;
		public Command BackCommand => _backCommand ?? (_backCommand = new Command(ExecuteBack, CanBack));

		private bool CanBack()
		{
			return _contentFrame.CanGoBack;
		}

		private void ExecuteBack()
		{
			_contentFrame.GoBack();
		}

		private Command _menuCommand;
		public Command MenuCommand => _menuCommand ?? (_menuCommand = new Command(ExecuteMenu));

		private void ExecuteMenu()
		{
			ShellSplitView.IsPaneOpen = !ShellSplitView.IsPaneOpen;
		}

		Command<NavType> _navCommand;
		public Command<NavType> NavCommand => _navCommand ?? (_navCommand = new Command<NavType>(ExecuteNav));

		public Frame ContentFrame => _contentFrame;

		private void ExecuteNav(NavType navType)
		{
			var type = navType.Type;

			_contentFrame.Navigate(navType.Type);

			if (type == typeof(MainPage))
			{
				_contentFrame.BackStack.Clear();
				BackCommand?.RaiseCanExecuteChanged();
			}
		}

		public List<RadioButton> AllRadioButtons(DependencyObject parent)
		{
			var list = new List<RadioButton>();
			for (var i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
			{
				var child = VisualTreeHelper.GetChild(parent, i);
				if (child is RadioButton)
				{
					list.Add(child as RadioButton);
					continue;
				}
				list.AddRange(AllRadioButtons(child));
			}
			return list;
		}

		private void DontCheck(object s, RoutedEventArgs e)
		{
			var radioButton = s as RadioButton;
			if (radioButton != null)
				radioButton.IsChecked = false;
		}
	}
}
