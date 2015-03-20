using System.ComponentModel;
using System.Dynamic;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;

namespace CommerceFoundation.UI.FunctionalTests.Catalogs.Extensions
{
	public class LocalViewModel<TModel> : DynamicObject, INotifyPropertyChanged where TModel : new()
	{
		public event PropertyChangedEventHandler PropertyChanged;

		// Create the OnPropertyChanged method to raise the event 
		protected void OnPropertyChanged(string name)
		{
			var handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(name));
			}
		}

		private TModel _entity = default(TModel);
		private TModel _original = default(TModel);
		/// <summary>
		/// Initializes a new instance of the <see cref="LocalViewModel{TModel}" /> class.
		/// </summary>
		/// <param name="original">The original.</param>
		/// <param name="entity">The entity.</param>
		public LocalViewModel(TModel original, TModel entity)
		{
			_original = original;
			_entity = entity;
		}

		/// <summary>
		/// Gets the entity.
		/// </summary>
		/// <value>
		/// The entity.
		/// </value>
		public TModel Entity
		{
			get { return _entity; }
		}

		// If you try to get a value of a property  
		// not defined in the class, this method is called. 
		public override bool TryGetMember(
			GetMemberBinder binder, out object result)
		{
			result = _entity.GetType().GetProperty(binder.Name).GetValue(_entity, null);
			return true;
		}

		// If you try to set a value of a property that is 
		// not defined in the class, this method is called. 
		public override bool TrySetMember(
			SetMemberBinder binder, object value)
		{
			_entity.GetType().GetProperty(binder.Name).SetValue(_entity, value, null);
			OnPropertyChanged(binder.Name);
			return true;
		}

		/// <summary>
		/// Saves the changes.
		/// </summary>
		public void AcceptChanges()
		{
			_original.InjectFrom<CloneInjection>(_entity);
		}
	}

	public static class ViewModelExtensions
	{
		/// <summary>
		/// Locals the specified entity.
		/// </summary>
		/// <typeparam name="TModel">The type of the model.</typeparam>
		/// <param name="entity">The entity.</param>
		/// <returns></returns>
		public static LocalViewModel<TModel> Local<TModel>(this TModel entity) where TModel : new()
		{
			var newEntity = new TModel();
			newEntity.InjectFrom<CloneInjection>(entity);

			return new LocalViewModel<TModel>(entity, newEntity);
		}
	}
}
