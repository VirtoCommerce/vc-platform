#region
using System.Collections.Generic;

#endregion

namespace VirtoCommerce.Web.Views
{
    public interface ITemplateEngine
    {
        #region Public Methods and Operators
        bool CanProcess(string inputType, string outputType);

        string Process(string contents, IDictionary<string, dynamic> attributes);
        #endregion
    }
}