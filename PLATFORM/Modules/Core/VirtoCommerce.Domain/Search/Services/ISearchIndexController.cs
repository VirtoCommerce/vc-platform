using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Domain.Search.Services
{
    public interface ISearchIndexController
    {
		/// <summary>
		/// Processes the staged indexes.
		/// </summary>
		void Process(string scope, string documentType = "", bool rebuild = false);
    }
}
