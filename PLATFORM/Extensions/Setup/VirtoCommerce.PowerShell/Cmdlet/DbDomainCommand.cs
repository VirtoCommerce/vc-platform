using System;
using System.Diagnostics;
using System.Management.Automation;

namespace VirtoCommerce.PowerShell.Cmdlet
{
	[CLSCompliant(false)]
	public abstract class DomainCommand : PSCmdlet
	{
		private readonly AppDomain _domain;

		protected DomainCommand()
		{
			_domain = AppDomain.CurrentDomain;
		}

		protected AppDomain Domain
		{
			get { return _domain; }
		}

		/// <summary>
		/// Wrap the base Cmdlet's WriteError call so that it will not throw
		/// a NotSupportedException when called without a CommandRuntime (i.e.,
		/// when not called from within Powershell).
		/// </summary>
		/// <param name="errorRecord">The error to write.</param>
		protected void SafeWriteError(ErrorRecord errorRecord)
		{
			Debug.Assert(errorRecord != null, "errorRecord cannot be null.");

			// If the exception is an Azure Service Management error, pull the
			// Azure message out to the front instead of the generic response.
			// errorRecord = AzureServiceManagementException.WrapExistingError(errorRecord);

			if (CommandRuntime != null)
			{
				WriteError(errorRecord);
			}
			else
			{
				Trace.WriteLine(errorRecord);
			}
		}

		/// <summary>
		/// Write an error message for a given exception.
		/// </summary>
		/// <param name="ex">The exception resulting from the error.</param>
		protected void SafeWriteError(Exception ex)
		{
			Debug.Assert(ex != null, "ex cannot be null or empty.");
			SafeWriteError(new ErrorRecord(ex, string.Empty, ErrorCategory.CloseError, null));
		}

		protected void SafeThrowError(ErrorRecord errorRecord)
		{
			Debug.Assert(errorRecord != null, "errorRecord cannot be null.");

			// If the exception is an Azure Service Management error, pull the
			// Azure message out to the front instead of the generic response.
			// errorRecord = AzureServiceManagementException.WrapExistingError(errorRecord);

			if (CommandRuntime != null)
			{
				this.ThrowTerminatingError(errorRecord);
			}
			else
			{
				Trace.WriteLine(errorRecord);
			}
		}

		protected void SafeThrowError(Exception ex)
		{
			Debug.Assert(ex != null, "ex cannot be null or empty.");

			if (CommandRuntime != null)
			{
				this.ThrowTerminatingError(new ErrorRecord(ex, string.Empty, ErrorCategory.CloseError, null));
			}
			else
			{
				throw ex;
			}
		}

		protected void SafeWriteVerbose(string text)
		{
			// If the exception is an Azure Service Management error, pull the
			// Azure message out to the front instead of the generic response.
			// errorRecord = AzureServiceManagementException.WrapExistingError(errorRecord);
			if (CommandRuntime != null)
			{
				try
				{
					WriteVerbose(text);
				}
				catch
				{
					SafeWriteDebug(text);
				}
			}
			else
			{
				Trace.WriteLine(text);
			}
		}

		protected void SafeWriteDebug(string text)
		{
			if (CommandRuntime != null)
			{
				WriteDebug(text);
			}
			else
			{
				Trace.WriteLine(text);
			}
		}

		public void SafeWriteProgress(ProgressRecord progress)
		{
			if (CommandRuntime != null)
			{
				WriteProgress(progress);
			}
			else
			{
				Trace.WriteLine(progress.StatusDescription);
			}
		}


	}
}