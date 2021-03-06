﻿using System;
using System.Windows.Input;
using MaxManager.ViewModels;
using MaxManager.Web.Lan;
using MaxManager.Web.Lan.Commands;
using MaxManager.Web.Lan.Parser;

namespace MaxManager.Commands
{
	class SetRoomControlModeCommand : ICommand
	{
		private readonly IMaxConnector _maxConnector;
		private readonly MaxRoomControlMode _roomControlMode;

		public SetRoomControlModeCommand(IMaxConnector maxConnector, MaxRoomControlMode roomControlMode)
		{
			_maxConnector = maxConnector;
			_roomControlMode = roomControlMode;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public async void Execute(object parameter)
		{
			var roomViewModel = parameter as RoomViewModel;
			if (roomViewModel == null)
				return;

			var maxCommand = new STemperatureAndModeMaxCommand
			{
				Mode = _roomControlMode,
				Temperature = (int)roomViewModel.SetPointTemperature * 2,
				RoomId = roomViewModel.MaxRoom.Id,
				RfAddress = roomViewModel.MaxRoom.GroupRfAddress
			};
		
			await _maxConnector.SendAsync(maxCommand);
		}

		public event EventHandler CanExecuteChanged;
	}
}
