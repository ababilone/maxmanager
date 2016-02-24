using System;

namespace MaxManager.Services.Settings
{
	public delegate void SettingUpdatedEventHandler(ISettingService settingService, EventArgs eventArgs);
}