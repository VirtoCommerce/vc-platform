using System;
using System.Windows;
using System.Data;
using System.Xml;
using System.Configuration;
using System.IO;

namespace Virtoway.WPF.State.Demo
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : System.Windows.Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			using (Stream stream = File.Open("ElementStateDemo.xml", FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
			{
				ElementStateOperations.Load(stream);
			}
			base.OnStartup(e);
		}

		protected override void OnExit(ExitEventArgs e)
		{
			using (Stream stream = File.Open("ElementStateDemo.xml", FileMode.Create, FileAccess.Write, FileShare.None))
			{
				ElementStateOperations.Save(stream);
			}
			base.OnExit(e);
		}
	}
}