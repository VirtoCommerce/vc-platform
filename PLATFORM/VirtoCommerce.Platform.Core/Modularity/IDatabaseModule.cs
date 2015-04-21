namespace VirtoCommerce.Platform.Core.Modularity
{
	/// <summary>
	/// Defines the contract for the modules using database.
	/// </summary>
	public interface IDatabaseModule
	{
		/// <summary>
		/// Allows module to configure database.
		/// </summary>
		/// <param name="sampleDataLevel">Defines the amount of sample data that should be inserted, if any.</param>
		void SetupDatabase(SampleDataLevel sampleDataLevel);
	}
}
