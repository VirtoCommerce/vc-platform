using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
	/// <summary>
	/// Interaction logic for CreatedModifiedStatusBar.xaml
	/// </summary>
	public partial class CreatedModifiedViewer : UserControl
	{
		#region .ctor
		public CreatedModifiedViewer()
		{
			InitializeComponent();
		}
		#endregion

		#region Properties

		public DateTime Created
		{
			get { return DateTime.SpecifyKind(Convert.ToDateTime(GetValue(CreatedProperty)), DateTimeKind.Local); }
			set { SetValue(CreatedProperty, value); }
		}
		
		public string CreatedBy
		{
			get { return (string)GetValue(CreatedByProperty); }
			set { SetValue(CreatedByProperty, value); }
		}

		public DateTime Modified
		{
			get { return DateTime.SpecifyKind(Convert.ToDateTime(GetValue(ModifiedProperty)), DateTimeKind.Local); }
			set { SetValue(ModifiedProperty, value); }
		}

		public string ModifiedBy
		{
			get { return (string)GetValue(ModifiedByProperty); }
			set { SetValue(ModifiedByProperty, value); }
		}

		#endregion

		#region Dependency Properties
		public static readonly DependencyProperty CreatedProperty =
				DependencyProperty.Register("Created",
				typeof(DateTime),
				typeof(CreatedModifiedViewer),
				new PropertyMetadata());

		public static readonly DependencyProperty CreatedByProperty =
				DependencyProperty.Register("CreatedBy",
				typeof(string),
				typeof(CreatedModifiedViewer),
				new PropertyMetadata());

		public static readonly DependencyProperty ModifiedProperty =
				DependencyProperty.Register("Modified",
				typeof(DateTime),
				typeof(CreatedModifiedViewer),
				new PropertyMetadata());

		public static readonly DependencyProperty ModifiedByProperty =
				DependencyProperty.Register("ModifiedBy",
				typeof(string),
				typeof(CreatedModifiedViewer),
				new PropertyMetadata());

		#endregion

		# region VisibilityProperties
		public bool CreatedExists
		{
			get { return (bool)(Created != DateTime.MinValue); }
		}
		public bool CreatedByExists
		{
			get { return (bool)!String.IsNullOrEmpty(CreatedBy); }
		}
		public bool ModifiedExists
		{
			get { return (bool)(Modified != DateTime.MinValue); }
		}
		public bool ModifiedByExists
		{
			get { return (bool)!String.IsNullOrEmpty(ModifiedBy); }
		}
		#endregion

	}
}
