using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Common;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Dialogs;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
	/// <summary>
	/// Base class for all ViewModel classes in the application.
	/// It provides support for property change notifications 
	/// and has a DisplayName property.  This class is abstract.
	/// </summary>
	public abstract class ViewModelBase : NotifyPropertyChanged, IViewModel
	{
		#region Protected Fields

		protected static TaskScheduler UITaskScheduler;

		#endregion

		#region Private Fields

		private bool _isInitializing;

		#endregion

		#region Constructor

		static ViewModelBase()
		{
			OnUIThread(() => UITaskScheduler = TaskScheduler.FromCurrentSynchronizationContext());
		}

		protected ViewModelBase(bool throwOnInvalidPropertyName = false)
		{
			ThrowOnInvalidPropertyName = throwOnInvalidPropertyName;
		}

		#endregion // Constructor

		#region Properties

		/// <summary>
		/// indicates if data processing is in progress (loading, saving)
		/// </summary>
		public bool IsInitializing
		{
			get { return _isInitializing; }
			set { _isInitializing = value; OnPropertyChanged(); }
		}

		private bool _showLoadingAnimation;
		public bool ShowLoadingAnimation
		{
			get { return _showLoadingAnimation; }
			set
			{
				_showLoadingAnimation = value;
				OnPropertyChanged();
			}
		}

		private string _animationText;
		public string AnimationText
		{
			get { return _animationText; }
			set
			{
				_animationText = value;
				OnPropertyChanged();
			}
		}
		/// <summary>
		/// Returns the user-friendly name of this object.
		/// Child classes can set this property to a new value,
		/// or override it to determine the value on-demand.
		/// </summary>
		public virtual string DisplayName
		{
			get
			{
				return "Undefined";
			}
		}

		public virtual string IconSource
		{
			get
			{
				return null;
			}
		}

		private ViewTitleBase _viewTitle;
		public ViewTitleBase ViewTitle
		{
			get { return _viewTitle; }
			set
			{
				_viewTitle = value;
				OnPropertyChanged();
			}
		}
		public virtual bool IsBackTrackingDisabled
		{
			get { return false; }
		}

		public int MenuOrder { get; set; }

		/// <summary>
		/// set the color of the menuTab in horizontal and vertical top menu
		/// </summary>
		public virtual Brush ShellDetailItemMenuBrush
		{
			get { return new SolidColorBrush(Color.FromRgb(51, 51, 51)); }
		}

		#endregion

		#region Thread Methods

		protected static T OnUIThread<T>(Func<T> callback)
		{

			if (Application.Current != null)
			{
				return (T)Application.Current.Dispatcher.Invoke(DispatcherPriority.Send, new Func<object>(() => callback()));
			}
			else
			{
				return (T)Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Send, new Func<object>(() => callback()));
			}

		}

		protected static void OnUIThread(Action callback)
		{
			if (Application.Current != null)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Send, callback);
			}
			else
			{
				Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Send, callback);
			}
		}

		public static void PerformTasksInBackgroundWorker(Action onDoWork, Action onRunWorkerCompleted = null)
		{
			using (var backgroundWorker = new BackgroundWorker())
			{
				backgroundWorker.DoWork += (sender, args) => onDoWork();
			    if (onRunWorkerCompleted != null)
			    {
			        backgroundWorker.RunWorkerCompleted += (sender, args) => onRunWorkerCompleted();
			    }
				backgroundWorker.RunWorkerAsync();
			}
		}

		public static Task ExecuteAsync(Action onDoWork, Action onWorkCompleted = null, Action onBeforeHandleException = null)
		{
			var task = new Task(onDoWork, TaskCreationOptions.LongRunning);
			task.ContinueWith(resultTask =>
			{
				if (onBeforeHandleException != null)
					onBeforeHandleException();

				if (HandleException(resultTask))
					return;

				if (onWorkCompleted != null)
					onWorkCompleted();
			}, UITaskScheduler);

			task.Start();
			return task;
		}

		public static Task ExecuteAsync<T>(Func<T> onDoWork, Action<T> onWorkCompleted = null, Action onBeforeHandleException = null, Action onExceptionHandled = null) where T : class
		{
			var task = new Task<T>(onDoWork, TaskCreationOptions.LongRunning);
			task.ContinueWith(resultTask =>
			{
				if (onBeforeHandleException != null)
					onBeforeHandleException();

				if (HandleException(resultTask))
				{
					if (onExceptionHandled != null)
						onExceptionHandled();
					return null;
				}

				if (onWorkCompleted != null)
					onWorkCompleted(resultTask.Result);

				return resultTask.Result;
			}, UITaskScheduler);

			task.Start();
			return task;
		}

		public static bool HandleException(Task task)
		{
			if (task.Exception == null)
				return false;

			var mess = task.Exception.Message;
			var clipboardText = task.Exception.ToString();
			if (task.Exception.InnerExceptions != null && task.Exception.InnerExceptions.Count > 0)
			{
				mess = task.Exception.InnerExceptions[0].Message;
				clipboardText = task.Exception.InnerExceptions[0].ToString();
			}
			ErrorDialog.ShowErrorDialog(string.Format("Error message: {0}", mess), task.Exception.ToString(), clipboardText,
										false);
			return true;
		}

		#endregion

		#region Debugging Aides

		/// <summary>
		/// Warns the developer if this object does not have
		/// a public property with the specified name. This 
		/// method does not exist in a Release build.
		/// </summary>
		[Conditional("DEBUG")]
		[DebuggerStepThrough]
		public void VerifyPropertyName(string propertyName)
		{
			// Verify that the property name matches a real,  
			// public, instance property on this object.
			if (TypeDescriptor.GetProperties(this)[propertyName] == null)
			{
				var msg = "Invalid property name: " + propertyName;

				if (ThrowOnInvalidPropertyName)
					throw new Exception(msg);

				Debug.Fail(msg);
			}
		}

		/// <summary>
		/// Returns whether an exception is thrown, or if a Debug.Fail() is used
		/// when an invalid property name is passed to the VerifyPropertyName method.
		/// The default value is false, but subclasses used by unit tests might 
		/// override this property's getter to return true.
		/// </summary>
		protected virtual bool ThrowOnInvalidPropertyName { get; private set; }

		#endregion // Debugging Aides

		public static IOrderedQueryable<T> ApplySortDescriptions<T>(IQueryable<T> query, SortDescriptionCollection sortDescriptions)
		{
			var orderedItems = sortDescriptions[0].Direction == ListSortDirection.Ascending ?
												 query.OrderBy(sortDescriptions[0].PropertyName) :
												 query.OrderByDescending(sortDescriptions[0].PropertyName);
			return orderedItems;
		}

		#region IDisposable Members

		/// <summary>
		/// Invoked when this object is being removed from the application
		/// and will be subject to garbage collection.
		/// </summary>
		public void Dispose()
		{
			OnDispose();
		}

		/// <summary>
		/// Child classes can override this method to perform 
		/// clean-up logic, such as removing event handlers.
		/// </summary>
		protected virtual void OnDispose()
		{
		}

#if DEBUG
		/// <summary>
		/// Useful for ensuring that ViewModel objects are properly garbage collected.
		/// </summary>
		~ViewModelBase()
		{
			var msg = string.Format("{0} ({1}) ({2}) Finalized", GetType().Name, DisplayName, GetHashCode());
			Debug.WriteLine(msg);
		}
#endif

		#endregion // IDisposable Members

		#region Auxiliary Methods
		public void RaiseOnPropertyChanged(string propertyName) { OnPropertyChanged(propertyName); }
		#endregion

		/// <summary>
		/// Show given message in Error dialog
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="msg">Error message</param>
		protected void ShowErrorDialog(Exception ex, string msg)
		{
			ErrorDialog.ShowErrorDialog(string.Format("{0}\n{1}", msg, ex.Message), ex.ToString(), string.Format("{0}\n{1}", msg, ex), false);
		}

		// private InputBindings _inputBindings;

		protected virtual IEnumerable<ActionBinding> GetActionBindings()
		{
			yield break;
		}

		public void InitializeGestures()
		{
			InputBindings.RegisterCommands(GetActionBindings(), this.GetHashCode());
		}
		//public void InitializeGestures(InputBindingCollection inputBindings, ActionBinding[] entries)
		//{
		//	if (_inputBindings == null)
		//	{
		//		_inputBindings = new InputBindings(inputBindings);
		//		_inputBindings.RegisterCommands(GetActionBindings(), entries);
		//	}
		//}
	}
}