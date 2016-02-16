using System;
using System.Windows.Input;
using MaxManager.Web.Lan;
using MaxManager.Web.Lan.Commands;

namespace MaxManager.Commands
{
	class SetRoomControlModeCommand : ICommand
	{
		private readonly IMaxConnector _maxConnector;

		public SetRoomControlModeCommand(IMaxConnector maxConnector)
		{
			_maxConnector = maxConnector;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public async void Execute(object parameter)
		{
			if (parameter == null)
				return;

			var maxCommand = new SMaxCommand();
			//await _maxConnector.Send(maxCommand);
		}

		public event EventHandler CanExecuteChanged;
	}
}
