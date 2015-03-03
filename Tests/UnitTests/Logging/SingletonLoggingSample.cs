using System;
using VirtoCommerce.Foundation.PlatformTools;
using Xunit;

namespace CommerceFoundation.UnitTests.Logging
{
	public class SingletonLoggingSample
	{
		[Fact]
		public void Method()
		{
			try
			{
				DoBusiness();
			}
			catch (Exception ex)
			{
				Logger.Error(ex.ToString()); // note: there are no addtional event sources created. insted it is collecting some user identity information
			}
		}

		private void DoBusiness()
		{
			//throw new ApplicationException("test");
		}
	}
}