using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace MaxManager.Views
{
	[ContentProperty(Name = "HeaderContent")]
	public sealed partial class MaxHeader
	{
		public MaxHeader()
		{
			InitializeComponent();
		}

		public FrameworkElement HeaderContent { get; set; }
	}
}
