using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Xml.XPath;
using NuGet;

namespace VirtoCommerce.PackagingModule.Data.Repositories
{
	public class WebsiteProjectSystem : IProjectSystem
	{
		public WebsiteProjectSystem(string rootPath)
		{
			Root = rootPath;
			ProjectName = Path.GetFileName(rootPath);
			TargetFramework = new FrameworkName(".NETFramework", GetTargetFrameworkVersion());
			Logger = NullLogger.Instance;
			IsBindingRedirectSupported = false;
		}

		#region IProjectSystem Members

		public string ProjectName { get; private set; }
		public FrameworkName TargetFramework { get; private set; }
		public bool IsBindingRedirectSupported { get; private set; }

		public void AddFrameworkReference(string name)
		{
			throw new NotImplementedException();
		}

		public void AddImport(string targetFullPath, ProjectImportLocation location)
		{
			throw new NotImplementedException();
		}

		public void AddReference(string referencePath, Stream stream)
		{
		}

		public bool FileExistsInProject(string path)
		{
			return FileExists(path);
		}

		public bool IsSupportedFile(string path)
		{
			return true;
		}

		public bool ReferenceExists(string name)
		{
			return false;
		}

		public void RemoveImport(string targetFullPath)
		{
			throw new NotImplementedException();
		}

		public void RemoveReference(string name)
		{
		}

		public string ResolvePath(string path)
		{
			return GetFullPath(path);
		}

		#endregion

		#region IFileSystem Members

		public string Root { get; private set; }
		public ILogger Logger { get; set; }

		public void AddFile(string path, Action<Stream> writeToStream)
		{
			AddFileCore(path, writeToStream);
		}

		public void AddFile(string path, Stream stream)
		{
			AddFileCore(path, stream.CopyTo);
		}

		public void AddFiles(IEnumerable<IPackageFile> files, string rootDir)
		{
			throw new NotImplementedException();
		}

		public Stream CreateFile(string path)
		{
			throw new NotImplementedException();
		}

		public void DeleteDirectory(string path, bool recursive)
		{
			if (DirectoryExists(path))
			{
				try
				{
					var fullPath = GetFullPath(path);
					Directory.Delete(fullPath, recursive);
				}
				catch (DirectoryNotFoundException) { }
			}
		}

		public void DeleteFile(string path)
		{
			if (FileExists(path))
			{
				try
				{
					var fullPath = GetFullPath(path);
					File.Delete(fullPath);
				}
				catch (FileNotFoundException) { }
			}
		}

		public void DeleteFiles(IEnumerable<IPackageFile> files, string rootDir)
		{
			throw new NotImplementedException();
		}

		public bool DirectoryExists(string path)
		{
			var fullPath = GetFullPath(path);
			return Directory.Exists(fullPath);
		}

		public bool FileExists(string path)
		{
			var fullPath = GetFullPath(path);
			return File.Exists(fullPath);
		}

		public DateTimeOffset GetCreated(string path)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<string> GetDirectories(string path)
		{
			if (DirectoryExists(path))
			{
				var fullPath = GetFullPath(path);
				return Directory.EnumerateDirectories(fullPath);//.Select(MakeRelativePath);
			}

			return Enumerable.Empty<string>();
		}

		public IEnumerable<string> GetFiles(string path, string filter, bool recursive)
		{
			if (DirectoryExists(path))
			{
				var fullPath = GetFullPath(path);
				return Directory.EnumerateFiles(fullPath, filter, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);//.Select(MakeRelativePath);
			}

			return Enumerable.Empty<string>();
		}

		public string GetFullPath(string path)
		{
			return Path.Combine(Root, path);
		}

		public DateTimeOffset GetLastAccessed(string path)
		{
			throw new NotImplementedException();
		}

		public DateTimeOffset GetLastModified(string path)
		{
			throw new NotImplementedException();
		}

		public void MakeFileWritable(string path)
		{
			throw new NotImplementedException();
		}

		public void MoveFile(string source, string destination)
		{
			throw new NotImplementedException();
		}

		public Stream OpenFile(string path)
		{
			var fullPath = GetFullPath(path);
			return File.OpenRead(fullPath);
		}

		#endregion

		#region IPropertyProvider Members

		public dynamic GetPropertyValue(string propertyName)
		{
			throw new NotImplementedException();
		}

		#endregion



		protected virtual void EnsureDirectory(string path)
		{
			var fullPath = GetFullPath(path);
			Directory.CreateDirectory(fullPath);
		}


		private void AddFileCore(string path, Action<Stream> writeToStream)
		{
			EnsureDirectory(Path.GetDirectoryName(path));

			using (var stream = File.Create(GetFullPath(path)))
			{
				writeToStream(stream);
			}
		}

		private Version GetTargetFrameworkVersion()
		{
			string result = null;

			const string defaultVersion = "4.5.1";
			const string webConfigPath = "web.config";

			if (FileExists(webConfigPath))
			{
				using (var stream = OpenFile(webConfigPath))
				{
					var webConfig = XmlUtility.LoadSafe(stream);
					var httpRuntime = webConfig.XPathSelectElement("/configuration/system.web/httpRuntime");

					if (httpRuntime != null)
						result = httpRuntime.GetOptionalAttributeValue("targetFramework");
				}
			}

			if (string.IsNullOrEmpty(result))
				result = defaultVersion;

			return Version.Parse(result);
		}
	}
}
