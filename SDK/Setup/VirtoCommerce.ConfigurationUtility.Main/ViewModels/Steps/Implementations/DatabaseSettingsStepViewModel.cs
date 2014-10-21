using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.ConfigurationUtility.Main.Infrastructure;
using VirtoCommerce.ConfigurationUtility.Main.Properties;
using VirtoCommerce.ConfigurationUtility.Main.ViewModels.Steps.Interfaces;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.PowerShell.DatabaseSetup.Cmdlet;

namespace VirtoCommerce.ConfigurationUtility.Main.ViewModels.Steps.Implementations
{
	public sealed class DatabaseSettingsStepViewModel : WizardStepViewModelBase, IDatabaseSettingsStepViewModel
	{
		#region Dependencies
		private readonly IConfirmationStepViewModel _confirmationViewModel;
		#endregion

		#region Constants

		private const string ConnectionStringFormat = @"Server={0};Database={1};{2};Connection Timeout=30;MultipleActiveResultSets=True";
		private const string TestConnectionStringFormat = @"Server={0};Database={1};{2};Connection Timeout=5";
		private const string WindowsAuthenticationFormat = "Integrated Security=True";
		private const string SqlServerAuthenticationFormat = "User ID={0};Password={1}";

		#endregion

		#region Constructors

		public DatabaseSettingsStepViewModel(IConfirmationStepViewModel confirmationViewModel)
		{
			_confirmationViewModel = confirmationViewModel;

			OnIsValidChanged();
			Initialize();
		}

		private async void Initialize()
		{
			TestConnectionCommand = new DelegateCommand(ValidateDatabaseConnection);
			IsInitializing = true;
			InstallSamples = true;
			IsNotTestedConnection = true;
			ServerAuthentication = SqlServerAuthentication.WindowsAuthentication;

			await Task.Run(() =>
			{
				if (availableServersCached == null)
				{
					availableServersCached = new List<string>();
					var servers = SqlDataSourceEnumerator.Instance.GetDataSources();
					foreach (var server in servers.Rows.Cast<DataRow>().Where(server => (string)server["ServerName"] == Environment.MachineName))
					{
						if ((server["InstanceName"] as string) != null)
						{
							availableServersCached.Add(server["ServerName"] + @"\" + server["InstanceName"]);
						}
						else
						{
							availableServersCached.Add(server["ServerName"].ToString());
						}
					}	
				}
				
				AvailableServers = availableServersCached;

				if (!IsConnectionValid && string.IsNullOrEmpty(Server))
				{
					Server = availableServersCached.Count > 0 ? availableServersCached[0] : string.Empty;	
				}
				
				IsInitializing = false;
				OnIsValidChanged();
			});
		}

		#endregion

		#region validators

		private bool _isConnectionValid;
		public bool IsConnectionValid
		{
			get { return _isConnectionValid; }
			set
			{
				_isConnectionValid = value;
				OnPropertyChanged();
			}
		}

		private async void ValidateDatabaseConnection()
		{
			IsTestConnection = true;
			if (!IsConnectionOneTimesTested)
				IsConnectionOneTimesTested = true;
			if (string.IsNullOrEmpty(DatabaseName))
			{
				IsConnectionValid = false;
				IsTestConnection = false;
				return;
			}
			await Task.Run(() =>
			{
				using (var con = new SqlConnection(_testConnectionString))
				{
					try
					{
						con.Open();
						var cmdText = "select count(*) from master.dbo.sysdatabases where name=\'" + DatabaseName + "\'";
						var sqlCmd = new SqlCommand(cmdText, con);
						var countFound = (int)sqlCmd.ExecuteScalar();
						bool doesExist = countFound > 0;
						if (doesExist)
						{
							sqlCmd.CommandText = string.Format("select count(*) from {0}.INFORMATION_SCHEMA.TABLES where table_name=\'Setting\'", DatabaseName);
							countFound = (int)sqlCmd.ExecuteScalar();
							doesExist = countFound > 0;
						}

						con.Close();

						if (doesExist)
						{
							OnPropertyChanged("DatabaseName");
							MessageBox.Show(String.Format(Resources.TestConnectionDBExists, DatabaseName), Resources.Failed, MessageBoxButtons.OK, MessageBoxIcon.Error);
							IsConnectionValid = false;
						}
						else
						{
							MessageBox.Show(Resources.TestConnectionPassExclamation, Resources.Successful, MessageBoxButtons.OK, MessageBoxIcon.Information);
							IsConnectionValid = true;
						}
					}
					catch (SqlException e)
					{
						MessageBox.Show(String.Format(Resources.TestConnectionFailExclamation, e.ErrorCode, e.Message), Resources.Failed, MessageBoxButtons.OK, MessageBoxIcon.Error);
						IsConnectionValid = false;
					}
				}

				IsNotTestedConnection = false;
				OnIsValidChanged();
			});
			IsTestConnection = false;
		}
		#endregion

		#region Overrides of WizardStepViewModelBase

		public override string Description
		{
			get { return Resources.DatabaseSettings; }
		}

		public override bool IsValid
		{
			get
			{
				return IsConnectionValid;
			}
		}

		public override bool IsLast
		{
			get { return false; }
		}

		#endregion

		#region Implementation of IConfigureStep

		public string Action
		{
			get { return Resources.DatabaseSettingsAction; }
		}

		public string Message { get; private set; }

		public void Configure(CancellationToken ct)
		{
			ct.ThrowIfCancellationRequested();

			Result = null; // sets result to InProgress
			try
			{
				var configurationFolder = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(ConfigurationWizardViewModel).Assembly.Location), @"..\Resources\Database"));
				var connectionString = _confirmationViewModel.DatabaseConnectionString;
				
				// Configure database
				new PublishAppConfigDatabase().PublishWithScope(connectionString, null, InstallSamples, InstallSamples, _confirmationViewModel.SearchConnection.Scope); // publish AppConfig first as it contains system tables
				ct.ThrowIfCancellationRequested();
				new PublishStoreDatabase().Publish(connectionString, null, InstallSamples, InstallSamples);
				ct.ThrowIfCancellationRequested();
				new PublishCatalogDatabase().Publish(connectionString, configurationFolder, InstallSamples, InstallSamples);
				ct.ThrowIfCancellationRequested();
				new PublishImportDatabase().Publish(connectionString, configurationFolder, InstallSamples, InstallSamples);
				ct.ThrowIfCancellationRequested();
				new PublishCustomerDatabase().Publish(connectionString, null, InstallSamples, InstallSamples);
				ct.ThrowIfCancellationRequested();
				new PublishInventoryDatabase().Publish(connectionString, null, InstallSamples, InstallSamples);
				ct.ThrowIfCancellationRequested();
				new PublishLogDatabase().Publish(connectionString, null, InstallSamples, InstallSamples);
				ct.ThrowIfCancellationRequested();
				new PublishMarketingDatabase().Publish(connectionString, null, InstallSamples, InstallSamples);
				ct.ThrowIfCancellationRequested();
				new PublishOrderDatabase().Publish(connectionString, null, InstallSamples, InstallSamples);
				ct.ThrowIfCancellationRequested();
				new PublishReviewDatabase().Publish(connectionString, null, InstallSamples, InstallSamples);
				ct.ThrowIfCancellationRequested();
				new PublishSearchDatabase().Publish(connectionString, null, InstallSamples, InstallSamples);
				ct.ThrowIfCancellationRequested();
				new PublishSecurityDatabase().Publish(connectionString, configurationFolder, InstallSamples, InstallSamples);
				ct.ThrowIfCancellationRequested();

				Result = OperationResult.Successful;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception e)
			{
				Message = string.Format("{0} {1}: {2}", Resources.DatabaseSettingsAction, Resources.Failed, e.ExpandExceptionMessage());
				Result = OperationResult.Failed;
				throw;
			}
		}

		public void Cancel()
		{
			Result = OperationResult.Cancelling;

			// TODO: implement cancellation
			Result = OperationResult.Cancelled;
		}

		public OperationResult? Result
		{
			get { return _result; }
			private set
			{
				_result = value;
				OnUIThread(() => _result.UpdateState(Configuration, Resources.DatabaseSettingsAction));
			}
		}

		private OperationResult? _result;

		#endregion

		#region Implementation of IDatabaseSettingsStepViewModel

		public IConfigurationViewModel Configuration { get; set; }

		public string Server
		{
			get { return _server; }
			set
			{
				_server = value;
				OnPropertyChanged();
				OnConnectionStringChanged();
				OnIsValidChanged();
			}
		}
		private string _server;

		public IEnumerable<string> AvailableServers
		{
			get { return _availableServers; }
			private set
			{
				_availableServers = value;
				OnPropertyChanged();
			}
		}
		private IEnumerable<string> _availableServers;
		private static List<string> availableServersCached;

		public string DatabaseName
		{
			get { return _databaseName; }
			set
			{
				_databaseName = value;
				OnPropertyChanged();
				OnConnectionStringChanged();
				OnIsValidChanged();
			}
		}
		private string _databaseName;

		public SqlServerAuthentication ServerAuthentication
		{
			get { return _serverAuthentication; }
			set
			{
				_serverAuthentication = value;
				_confirmationViewModel.SqlServerAuthentication = _serverAuthentication == SqlServerAuthentication.WindowsAuthentication
																							  ? Resources.WindowsAuthentication
																							  : Resources.SqlServerAuthentication;
				OnPropertyChanged();
				OnPropertyChanged("Login");
				OnPropertyChanged("Password");
				OnConnectionStringChanged();
				OnIsValidChanged();
			}
		}
		private SqlServerAuthentication _serverAuthentication;

		public IEnumerable<SqlServerAuthentication> ServerAuthenticationMethods
		{
			get
			{
				yield return SqlServerAuthentication.WindowsAuthentication;
				yield return SqlServerAuthentication.SqlServerAuthentication;
			}
		}

		private string _login;
		public string Login
		{
			get { return _login; }
			set
			{
				_login = value;
				OnPropertyChanged();
				OnConnectionStringChanged();
				OnIsValidChanged();
			}
		}

		private string _password;
		public string Password
		{
			get { return _password; }
			set
			{
				_password = value;
				OnPropertyChanged();
				OnConnectionStringChanged();
				OnIsValidChanged();
			}
		}

		private bool _installSamples;
		public bool InstallSamples
		{
			get { return _installSamples; }
			set
			{
				_installSamples = value;
				OnPropertyChanged();
			}
		}


		private string _testConnectionString = string.Empty;

		private void OnConnectionStringChanged()
		{
			var authenticationString = ServerAuthentication == SqlServerAuthentication.WindowsAuthentication ? WindowsAuthenticationFormat : string.Format(SqlServerAuthenticationFormat, Login, Password);
			var connectionString = string.Format(ConnectionStringFormat, Server, DatabaseName, authenticationString);
			_testConnectionString = string.Format(TestConnectionStringFormat, Server, string.Empty, authenticationString);
			_confirmationViewModel.DatabaseConnectionString = connectionString;
			IsConnectionValid = false;
			IsNotTestedConnection = true;
		}

		public DelegateCommand TestConnectionCommand { get; private set; }

		private bool _isNotTestedConnection;
		public bool IsNotTestedConnection
		{
			get { return _isNotTestedConnection; }
			set
			{
				_isNotTestedConnection = value;
				OnPropertyChanged();
				OnPropertyChanged("IsTestButtonAvailable");
			}
		}
		#endregion

		#region Implementation of IDataErrorInfo

		public string this[string name]
		{
			get
			{
				string result = null;

				switch (name)
				{
					case "Server":
						if (string.IsNullOrWhiteSpace(Server))
						{
							result = Resources.InvalidSqlServerName;
						}
						break;
					case "DatabaseName":
						if (string.IsNullOrWhiteSpace(DatabaseName) || IsTestConnection)
						{
							result = Resources.InvalidDatabaseName;
						}
						break;
					case "Login":
						if (ServerAuthentication == SqlServerAuthentication.SqlServerAuthentication && string.IsNullOrWhiteSpace(Login))
						{
							result = Resources.InvalidSqlServerAuthenticationLogin;
						}
						break;
					//case "Password":
					//	if (ServerAuthentication == SqlServerAuthentication.SqlServerAuthentication && string.IsNullOrWhiteSpace(Password))
					//	{
					//		result = Resources.InvalidSqlServerAuthenticationPassword;
					//	}
					//	break;
				}

				return result;
			}
		}

		public string Error
		{
			get { return null; }
		}

		#endregion

		#region ViewModel properties

		private bool _isTestConnection;
		public bool IsTestConnection
		{
			get
			{
				return _isTestConnection;
			}
			set
			{
				_isTestConnection = value;
				OnPropertyChanged();
				OnPropertyChanged("IsTestButtonAvailable");
				OnPropertyChanged("IsComboBoxEnabled");
			}
		}

		public bool IsComboBoxEnabled
		{
			get { return !IsTestConnection; }
		}

		public bool IsTestButtonAvailable
		{
			get
			{
				return !IsTestConnection && IsNotTestedConnection;
			}
		}

		private bool _isConnectionOneTimesTested;
		public bool IsConnectionOneTimesTested
		{
			get { return _isConnectionOneTimesTested; }
			set
			{
				_isConnectionOneTimesTested = value;
				OnPropertyChanged();
			}
		}

		#endregion

	}
}