using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;

namespace Presentation.CustomerService.Infrastructure.Communications
{
    public enum CommunicationType
    {
        [Description("Email")]
        Email,
        [Description("Входящий телефонный звонок")]
        InboundCall,
        [Description("Изходящий телефонный звонок")]
        OutboundCall,
        [Description("Заметка")]
        Note
    }

}
