using System;
using Windows.UI.Xaml;

namespace MaxManager.Utils.Timer
{
	public delegate void CountdownSecondElaspedEventHandler(object sender, int remainingSeconds);
	public delegate void CountdownFinishedEventHandler(object sender);

	public class CountdownTimer
	{
		private DispatcherTimer _dispatcherTimer;
		private int _remainingSeconds;

		public event CountdownSecondElaspedEventHandler CountdownSecondElasped;
		public event CountdownFinishedEventHandler CountdownFinished;

		public void Start(int seconds)
		{
			_remainingSeconds = seconds;
			_dispatcherTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
			_dispatcherTimer.Tick += DispatcherTimer_Tick;
			_dispatcherTimer.Start();
			CountdownSecondElasped?.Invoke(this, _remainingSeconds);
		}

		public void Stop()
		{
			_dispatcherTimer?.Stop();
		}

		private void DispatcherTimer_Tick(object sender, object e)
		{
			_remainingSeconds--;

			if (_remainingSeconds == 0)
			{
				_dispatcherTimer.Stop();
				CountdownFinished?.Invoke(this);
			}
			else
			{
				CountdownSecondElasped?.Invoke(this, _remainingSeconds);
			}
		}
	}
}