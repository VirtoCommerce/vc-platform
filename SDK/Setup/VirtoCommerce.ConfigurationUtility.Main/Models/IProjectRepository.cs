using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.ConfigurationUtility.Main.Models
{
    public interface IProjectRepository : IRepository
    {
        IQueryable<Project> Projects { get; }
    }
}
