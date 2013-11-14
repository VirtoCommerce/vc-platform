using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace VirtoCommerce.ManagementClient.Core.Controls
{
	[TemplatePart]
	public class MetroButton : Button
	{

		public MetroButton()
			: base()
		{

		}


		#region Dependency Propreties

		public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string), typeof(MetroButton), new PropertyMetadata(string.Empty));

		public static readonly DependencyProperty EllipseHeightProperty = DependencyProperty.Register("EllipseHeight", typeof(double), typeof(MetroButton));

		#endregion


		#region Public Properties

		public string Header
		{
			get { return (string)GetValue(HeaderProperty); }
			set
			{
				SetValue(HeaderProperty, value);
			}
		}

		public double EllipseHeight
		{
			get { return (double)GetValue(EllipseHeightProperty); }
			set
			{
				SetValue(EllipseHeightProperty, value);
			}
		}

		#endregion



	}
}
