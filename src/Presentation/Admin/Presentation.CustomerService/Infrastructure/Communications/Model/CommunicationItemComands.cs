using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Communications
{
    public class CommunicationItemComands : ViewModelBase, INotifyPropertyChanged
    {
        public string Icon {get; set;}
        public string ToolTip { get; set; }
        public string Header { get; set; }
        public object CommandParametr { get; set; }
        public DelegateCommand<object> Command { get; set; }
        public Func<bool> SetVisible { get; set; }
        private bool isVisible;
        public bool IsVisible 
        { 
            get { return isVisible;} 
            set
            {
                isVisible = value;
   				OnPropertyChanged("IsVisible");
            }
        }

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                OnPropertyChanged();
            }
        }

        private string _commandGroupName;
        public string CommandGroupName
        {
            get { return _commandGroupName; }
            set { _commandGroupName = value;
            OnPropertyChanged();}
        }
    }
}
