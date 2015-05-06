namespace VirtoCommerce.Domain.Search.Model
{
    public class SearchIndexEventArgs : SearchEventArgs
    {
        private double _CompletePercentage = 0;

        /// <summary>
        /// Gets or sets the completed percentage.
        /// </summary>
        /// <value>The completed percentage.</value>
        public double CompletedPercentage
        {
            get { return _CompletePercentage; }
            set { _CompletePercentage = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchIndexEventArgs"/> class.
        /// </summary>
        public SearchIndexEventArgs()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchIndexEventArgs"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="percentage">The percentage.</param>
        public SearchIndexEventArgs(string message, double percentage)
            : base()
        {
            this.Message = message;
            this.CompletedPercentage = percentage;
        }
    }    
}
