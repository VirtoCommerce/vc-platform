using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Microsoft.Web.Administration;
using Microsoft.Web.RuntimeStatus;

namespace VirtoCommerce.ConfigurationUtility.Main.Infrastructure
{
	internal class NativeMethods
	{
		// Methods
		[DllImport("user32.dll", SetLastError = true)]
		internal static extern IntPtr GetTopWindow(IntPtr hWnd);
		[DllImport("user32.dll", SetLastError = true)]
		internal static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);
		[DllImport("user32.dll", SetLastError = true)]
		internal static extern uint GetWindowThreadProcessId(IntPtr hwnd, out uint lpdwProcessId);
		[DllImport("user32.dll", SetLastError = true)]
		internal static extern bool PostMessage(HandleRef hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
	}

	public static class IisExpress
	{
		private static readonly List<string> Paths = new List<string>();
		private static readonly List<string> Sites = new List<string>();

		public static readonly string ConfigurationPath = @"%userprofile%\documents\iisexpress\config\applicationhost.config";

		public static void Start(string site, int port = 7329)
		{
			Sites.Clear();

			// check if process is already running
			var processes = Process.GetProcessesByName("iisexpress", ".");
			if (processes.Length > 0)
			{
				foreach (var pr in processes)
				{
					PopulateProcessInformation(pr.Id);
				}

				if (!Sites.Contains(site.ToLower()))
				{
					Sites.Add(site.ToLower());
				}
				else
				{
					return;
				}
			}
			else
			{
				Sites.Add(site.ToLower());
			}

			var processInfo = new ProcessStartInfo()
			{
				FileName = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\IIS Express\\iisexpress.exe",
				RedirectStandardOutput = true,
				UseShellExecute = false,
				CreateNoWindow = true
			};

			if (!String.IsNullOrEmpty(site))
			{
				processInfo.Arguments = string.Format("/site:\"{0}\"", site);
			}

			Process.Start(processInfo);
			Thread.Sleep(5000);
		}

		public static void StartFromPath(string path, int port = 7329)
		{
			if (!Paths.Contains(path.ToLower()))
				Paths.Add(path.ToLower());
			else
				return;

			var arguments = new StringBuilder();
			arguments.Append(String.Format(@"/path:""{0}""", path));
			arguments.Append(String.Format(@"/config:""{0}""", ConfigurationPath));
			//arguments.Append(@" /Port:" + port);
			var process = new Process()
			{
				StartInfo = new ProcessStartInfo()
					{
						FileName = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\IIS Express\\iisexpress.exe",
						Arguments = arguments.ToString(),
						RedirectStandardOutput = true,
						UseShellExecute = false,
						CreateNoWindow = true
					}
			};

			process.Start();


			while (!process.StandardOutput.EndOfStream)
			{
				string line = process.StandardOutput.ReadToEnd();
				Trace.WriteLine(line);
			}


			//Thread.Sleep(10000);
		}

		public static void ListServers()
		{
			using (var mgr = new ServerManager(ConfigurationPath))
			{
				foreach (var site in mgr.Sites)
				{
					Console.WriteLine(site.Name);
				}
			}
		}

		private static void PopulateProcessInformation(int processId)
		{
			Microsoft.Web.RuntimeStatus.WorkerProcess workerProcess = null;
			try
			{
				using (var client = new RuntimeStatusClient())
				{
					workerProcess = client.GetWorkerProcess(processId);
					foreach (string str in workerProcess.RegisteredUrlsInfo)
					{
						string[] strArray = str.Split(new char[] { '|' });
						if (strArray.Length == 3)
						{
							string siteName = strArray[0];
							string physicalPath = strArray[1];
							string url = strArray[2];
							url = url.Replace("://*:", "://localhost:");
							Sites.Add(siteName.ToLower());
							//this.Applications.Add(new IisExpressApplication(siteName, url, IisExpressHelper.GetPortNumberFromUrl(str), physicalPath, this));
						}
					}
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				if (workerProcess != null)
				{
					workerProcess.Dispose();
				}
			}
		}
	}
}
