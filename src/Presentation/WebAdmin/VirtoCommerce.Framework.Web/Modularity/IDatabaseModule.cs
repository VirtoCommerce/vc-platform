using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Framework.Web.Modularity
{
	/// <summary>
	/// Defines the contract for the modules using database.
	/// </summary>
	public interface IDatabaseModule
	{
		/// <summary>
		/// Allows module to configure database.
		/// </summary>
		/// <param name="insertSampleData">If true, insert sample data.</param>
		/// <param name="reducedSampleData">If true, insert reduced sample data.</param>
		void SetupDatabase(bool insertSampleData, bool reducedSampleData);
	}
}
