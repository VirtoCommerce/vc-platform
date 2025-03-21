using System;
using System.Collections.Generic;
using System.IO;

namespace VirtoCommerce.Platform.Core.Modularity
{
    /// <summary>
    /// Loads modules from an arbitrary location on the filesystem. This type loader is only called if 
    /// <see cref="ModuleInfo"/> classes have a Ref parameter that starts with "file://". 
    /// This class is only used on the Desktop version of the Prism Library.
    /// </summary>
    public class FileModuleTypeLoader : IModuleTypeLoader, IDisposable
    {
        private const string _fileSchema = "file://";

        private readonly IAssemblyResolver _assemblyResolver;
        private readonly HashSet<Uri> _downloadedUris = [];


        /// <summary>
        /// Initializes a new instance of the <see cref="FileModuleTypeLoader"/> class.
        /// </summary>
        /// <param name="assemblyResolver">The assembly resolver.</param>
        public FileModuleTypeLoader(IAssemblyResolver assemblyResolver)
        {
            _assemblyResolver = assemblyResolver;
        }

        /// <summary>
        /// Raised repeatedly to provide progress as modules are loaded in the background.
        /// </summary>
        public event EventHandler<ModuleDownloadProgressChangedEventArgs> ModuleDownloadProgressChanged;

        private void RaiseModuleDownloadProgressChanged(ModuleInfo moduleInfo, long bytesReceived, long totalBytesToReceive)
        {
            RaiseModuleDownloadProgressChanged(new ModuleDownloadProgressChangedEventArgs(moduleInfo, bytesReceived, totalBytesToReceive));
        }

        private void RaiseModuleDownloadProgressChanged(ModuleDownloadProgressChangedEventArgs e)
        {
            if (ModuleDownloadProgressChanged != null)
            {
                ModuleDownloadProgressChanged(this, e);
            }
        }

        /// <summary>
        /// Raised when a module is loaded or fails to load.
        /// </summary>
        public event EventHandler<LoadModuleCompletedEventArgs> LoadModuleCompleted;

        private void RaiseLoadModuleCompleted(ModuleInfo moduleInfo, Exception error)
        {
            RaiseLoadModuleCompleted(new LoadModuleCompletedEventArgs(moduleInfo, error));
        }

        private void RaiseLoadModuleCompleted(LoadModuleCompletedEventArgs e)
        {
            if (LoadModuleCompleted != null)
            {
                LoadModuleCompleted(this, e);
            }
        }

        /// <summary>
        /// Evaluates the <see cref="ModuleInfo.Ref"/> property to see if the current type loader will be able to retrieve the <paramref name="moduleInfo"/>.
        /// Returns true if the <see cref="ModuleInfo.Ref"/> property starts with "file://", because this indicates that the file
        /// is a local file. 
        /// </summary>
        /// <param name="moduleInfo">Module that should have its type loaded.</param>
        /// <returns>
        /// 	<see langword="true"/> if the current type loader is able to retrieve the module, otherwise <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">An <see cref="ArgumentNullException"/> is thrown if <paramref name="moduleInfo"/> is null.</exception>
        public bool CanLoadModuleType(ModuleInfo moduleInfo)
        {
            ArgumentNullException.ThrowIfNull(moduleInfo);

            return moduleInfo.Ref != null && moduleInfo.Ref.StartsWith(_fileSchema, StringComparison.Ordinal);
        }


        /// <summary>
        /// Retrieves the <paramref name="moduleInfo"/>.
        /// </summary>
        /// <param name="moduleInfo">Module that should have its type loaded.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Exception is rethrown as part of a completion event")]
        public void LoadModuleType(ModuleInfo moduleInfo)
        {
            ArgumentNullException.ThrowIfNull(moduleInfo);

            try
            {
                var uri = new Uri(moduleInfo.Ref, UriKind.RelativeOrAbsolute);

                // If this module has already been downloaded, I fire the completed event.
                if (IsSuccessfullyDownloaded(uri))
                {
                    RaiseLoadModuleCompleted(moduleInfo, null);
                }
                else
                {
                    var path = moduleInfo.Ref.StartsWith(_fileSchema + "/", StringComparison.Ordinal)
                        ? moduleInfo.Ref.Substring(_fileSchema.Length + 1)
                        : moduleInfo.Ref.Substring(_fileSchema.Length);

                    var fileSize = -1L;
                    if (File.Exists(path))
                    {
                        var fileInfo = new FileInfo(path);
                        fileSize = fileInfo.Length;
                    }

                    // Although this isn't asynchronous, nor expected to take very long, I raise progress changed for consistency.
                    RaiseModuleDownloadProgressChanged(moduleInfo, 0, fileSize);

                    moduleInfo.Assembly = _assemblyResolver.LoadAssemblyFrom(moduleInfo.Ref);

                    // Although this isn't asynchronous, nor expected to take very long, I raise progress changed for consistency.
                    RaiseModuleDownloadProgressChanged(moduleInfo, fileSize, fileSize);

                    // I remember the downloaded URI.
                    RecordDownloadSuccess(uri);

                    RaiseLoadModuleCompleted(moduleInfo, null);
                }
            }
            catch (Exception ex)
            {
                RaiseLoadModuleCompleted(moduleInfo, ex);
            }
        }

        private bool IsSuccessfullyDownloaded(Uri uri)
        {
            lock (_downloadedUris)
            {
                return _downloadedUris.Contains(uri);
            }
        }

        private void RecordDownloadSuccess(Uri uri)
        {
            lock (_downloadedUris)
            {
                _downloadedUris.Add(uri);
            }
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <remarks>Calls <see cref="Dispose(bool)"/></remarks>.
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the associated <see cref="_assemblyResolver"/>.
        /// </summary>
        /// <param name="disposing">When <see langword="true"/>, it is being called from the Dispose method.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_assemblyResolver is IDisposable disposableResolver)
            {
                disposableResolver.Dispose();
            }
        }

        #endregion
    }
}
