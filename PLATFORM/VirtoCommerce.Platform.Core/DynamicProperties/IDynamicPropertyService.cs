using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
    public interface IDynamicPropertyService
    {
        string[] GetObjectTypes();
        DynamicProperty[] GetTypeProperties(string objectType);
        void SaveTypeProperties(DynamicProperty[] properties);

        DynamicProperty[] GetObjectProperties(string objectType, string objectId);
        void SaveObjectProperties(DynamicProperty[] properties);
        void DeleteObjectValues(string objectType, string objectId);
        //void DeleteProperty(string propertyId);
        //void DeleteProperties(string objectType);
    }
}
