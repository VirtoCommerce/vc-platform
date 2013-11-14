using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
    /// <summary>
    /// base class that implements INotifyPropertyChanged
    /// </summary>
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

		// rp: added to debug subscriptions; now I can put a breakpoint and see who and when subscribes to the events
	    private event PropertyChangedEventHandler PropertyChangedImpl;
        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged
        {
			remove
			{
				PropertyChangedImpl -= value;
			}
			add
			{
				PropertyChangedImpl += value;
			}
        }

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
			OnSpecifiedPropertyChanged(propertyName);
        }

		protected void OnSpecifiedPropertyChanged(string propertyName)
		{
			if (PropertyChangedImpl != null)
			{
				var e = new PropertyChangedEventArgs(propertyName);
				PropertyChangedImpl(this, e);
			}
		}
        #endregion
    }
}
