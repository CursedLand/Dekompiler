using System.Windows;
using System.Windows.Controls;
using Dekompiler.Core;
using Dekompiler.Core.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace Dekompiler.Desktop
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			var serviceCollection = new ServiceCollection();
			serviceCollection.AddWpfBlazorWebView();
			serviceCollection.AddSingleton(ThemeConfiguration.Default); // change to .Dark to get dark-theme.
			serviceCollection.AddSingleton<RendererService>();
			Resources.Add("services", serviceCollection.BuildServiceProvider());
		}
	}
}