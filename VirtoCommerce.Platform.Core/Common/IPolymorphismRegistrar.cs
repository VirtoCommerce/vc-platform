namespace VirtoCommerce.Platform.Core.Common
{
    public interface IPolymorphismRegistrar
    {
        IPolymorphicBaseTypeInfo[] GetPolymorphicBaseTypes(string moduleName);
        void RegisterPolymorphicBaseType(string moduleName, IPolymorphicBaseTypeInfo polymorphismBaseTypeInfo);
    }
}
