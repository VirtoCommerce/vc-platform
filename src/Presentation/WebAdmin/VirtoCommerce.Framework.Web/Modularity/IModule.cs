namespace VirtoCommerce.Framework.Web.Modularity
{
	/// <summary>
	/// Defines the contract for the modules deployed in the application.
	/// </summary>
	public interface IModule
	{
		/// <summary>
		/// Allows module to configure database.
		/// </summary>
		/// <param name="insertSampleData">If true, insert sample data.</param>
		/// <param name="reducedSampleData">If true, insert reduced sample data.</param>
		void SetupDatabase(bool insertSampleData, bool reducedSampleData);

		/// <summary>
		/// Notifies the module that it has be initialized.
		/// </summary>
		void Initialize();
	}
}
