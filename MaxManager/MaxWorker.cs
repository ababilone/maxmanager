using System;
using Windows.System.Threading;
using GalaSoft.MvvmLight.Ioc;
using MaxManager.Web.Lan;
using MaxManager.Web.Lan.Commands;

namespace MaxManager
{
	class MaxWorker
	{
		private readonly TimeSpan _interval;
		private ThreadPoolTimer _threadPoolTimer;

		public MaxWorker(TimeSpan interval)
		{
			_interval = interval;
		}

		public void Start()
		{
			ProcessMaxMessages();

			if (_threadPoolTimer == null)
				_threadPoolTimer = ThreadPoolTimer.CreatePeriodicTimer(Handler, _interval, Destroyed);
		}

		private void Handler(ThreadPoolTimer timer)
		{
			ProcessMaxMessages();
		}

		private static void ProcessMaxMessages()
		{
			var maxConnector = SimpleIoc.Default.GetInstance<IMaxConnector>();
			maxConnector?.Process();
			maxConnector?.Send(MaxCommands.L);
		}

		private void Destroyed(ThreadPoolTimer timer)
		{
		}

		public void Stop()
		{
			_threadPoolTimer?.Cancel();
			_threadPoolTimer = null;
		}
	}
}