using System.Windows;
using Microsoft.Practices.ServiceLocation;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Behaviors.Common;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Behaviors
{
	public static class PermissionBehavior
	{
	    private static IAuthenticationContext _authContext;
	    private static IAuthenticationContext AuthContext
	    {
	        get { return _authContext ?? (_authContext = ServiceLocator.Current.GetInstance<IAuthenticationContext>()); }
	    }

        #region Permission Property

        public static readonly DependencyProperty PermissionIdProperty = DependencyProperty.RegisterAttached(
            "PermissionId",
            typeof(string),
            typeof(PermissionBehavior),
            new PropertyMetadata("", OnVisibilityChanged));

        public static void SetPermissionId(UIElement uiElement, string value)
        {
            uiElement.SetValue(PermissionIdProperty, value);
        }

        public static string GetPermissionId(UIElement uiElement)
        {
            return (string)uiElement.GetValue(PermissionIdProperty);
        }

        #endregion

        #region DenyVisibility Property

        public static readonly DependencyProperty DenyVisibilityProperty = DependencyProperty.RegisterAttached(
            "DenyVisibilityProperty",
			typeof(Visibility),
			typeof(PermissionBehavior),
			new PropertyMetadata(Visibility.Hidden, OnVisibilityChanged));

        public static void SetDenyVisibility(UIElement uiElement, Visibility visibility)
		{
            uiElement.SetValue(DenyVisibilityProperty, visibility);
		}

		public static Visibility GetDenyVisibility(UIElement uiElement)
		{
			return (Visibility) uiElement.GetValue(DenyVisibilityProperty);
		}
	    #endregion

        #region DenyEnabled Property

        public static readonly DependencyProperty DenyEnabledProperty = DependencyProperty.RegisterAttached(
            "DenyEnabledProperty",
            typeof(bool),
            typeof(PermissionBehavior),
            new PropertyMetadata(false, OnVisibilityChanged));

        public static void SetDenyEnabled(UIElement uiElement, bool value)
        {
            uiElement.SetValue(DenyEnabledProperty, value);
        }

        public static bool GetDenyEnabled(UIElement uiElement)
        {
            return (bool)uiElement.GetValue(DenyEnabledProperty);
        }

        #endregion

		private static readonly AttachedBehavior Behavior =
			AttachedBehavior.Register(host => new PermissionVisibilityBehavior(host));

		private static void OnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Behavior.Update(d);
		}

		private sealed class PermissionVisibilityBehavior : Behavior<UIElement>
		{
			internal PermissionVisibilityBehavior(DependencyObject host) : base(host)
			{
			}

			protected override void Update(UIElement host)
			{
				//check permission by Permission property
                bool hasPermition = AuthContext.CheckPermission(GetPermissionId(host));
                host.Visibility = hasPermition ? Visibility.Visible : GetDenyVisibility(host);
                host.IsEnabled = hasPermition || GetDenyEnabled(host);
			}
		}
	}
}