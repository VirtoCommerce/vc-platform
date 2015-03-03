using System;
using FunctionalTests.TestHelpers;

namespace UI.FunctionalTests.Helpers.Common
{
	public abstract class FunctionalUITestBase : FunctionalTestBase, IDisposable
	{
		protected TestServiceManager ServManager;

		protected FunctionalUITestBase()
		{
			ServManager = new TestServiceManager();
			DefService();
			ServManager.CreateDb(EnsureDatabaseInitialized);
		}

		public void Dispose()
		{
			ServManager.Clean();
		}

		public override void Init(RepositoryProvider provider)
		{
			if (provider == RepositoryProvider.DataService)
			{
				ServManager.InitService();
			}

			base.Init(provider);
		}

		public abstract void DefService();
	}
}
