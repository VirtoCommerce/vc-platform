using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Search
{
    public interface ISearchIndexController
    {
        //void StartIndexing(string scope, string documentType = "", bool rebuild = false);
        /// <summary>
        /// Stages indexes. Should run only using one process.
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="documentType"></param>
        /// <param name="rebuild"></param>
        void Prepare(string scope, string documentType = "", bool rebuild = false);

        /// <summary>
        /// Processes the staged indexes.
        /// </summary>
        void Process(string scope, string documentType = "");
    }
}
