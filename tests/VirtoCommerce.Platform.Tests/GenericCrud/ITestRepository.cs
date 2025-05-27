using System.Linq;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Tests.GenericCrud;

public interface ITestRepository : IRepository
{
    IQueryable<TestEntity> Entities { get; }
}
