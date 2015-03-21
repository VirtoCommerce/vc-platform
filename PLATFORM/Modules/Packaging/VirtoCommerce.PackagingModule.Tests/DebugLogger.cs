using System;
using System.Diagnostics;
using NuGet;

namespace VirtoCommerce.PackagingModule.Tests
{
	public class DebugLogger : ILogger
	{
		public void Log(MessageLevel level, string message, params object[] args)
		{
			Debug.WriteLine(level + ": " + message, args);
		}

		public FileConflictResolution ResolveFileConflict(string message)
		{
			Debug.WriteLine(message);
			throw new NotImplementedException();
		}
	}
}
