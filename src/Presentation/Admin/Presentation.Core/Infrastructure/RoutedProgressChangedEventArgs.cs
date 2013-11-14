using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
	public delegate void RoutedProgressChangedEventHandler(object sender, RoutedProgressChangedEventArgs args);

	public class RoutedProgressChangedEventArgs : RoutedEventArgs
	{
		public int ProgressPercentage 
		{ 
			get; 
			private set;
		}
		public object UserState 
		{ 
			get; 
			private set;
		}

        public RoutedProgressChangedEventArgs(RoutedEvent routedEvent, object source, ProgressChangedEventArgs args)
            : base(routedEvent, source)
        {
			if(args != null)
			{
				ProgressPercentage = args.ProgressPercentage;
				UserState = args.UserState;
			}
        }
	}
}
