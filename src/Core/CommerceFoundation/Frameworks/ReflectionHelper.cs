using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Reflection;
using System.IO;

namespace VirtoCommerce.Foundation.Frameworks
{
    public class ReflectionHelper
    {
        protected static List<Type> TypeList = new List<Type>();
        /// <summary>
        /// Loads the types.
        /// </summary>
        private static void LoadTypes()
        {
            lock (TypeList)
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in assemblies)
                {
                    Type[] types = null;
                    try
                    {
                        types = assembly.GetTypes();
                    }
                    catch (ReflectionTypeLoadException)
                    {
                        throw;
                    }

                    foreach (Type loadedType in types)
                    {
                        TypeList.Add(loadedType);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the subclasses.
        /// </summary>
        /// <param name="baseClass">The base class.</param>
        /// <returns></returns>
        public static string[] GetSubclasses(string baseClass)
        {
            var subclassList = new List<string>();

            lock (TypeList)
            {
                // Only load types once
                if (TypeList.Count == 0)
                {
                    LoadTypes();
                }

                try
                {
                    foreach (var pluginType in TypeList)
                    {
                        if (pluginType != null && pluginType.GetInterface(baseClass, true) != null)
                        {
                            if (!pluginType.IsAbstract)
                            {
                                subclassList.Add(pluginType.AssemblyQualifiedName);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Added following line to suppress warning about
                    //  ex not being used
                    ex.ToString();
                    throw;
                }
            }

            return subclassList.ToArray();
        }

        /// <summary>
        /// Gets the subclasses.
        /// </summary>
        /// <param name="baseClass">The base class.</param>
        /// <returns></returns>
        public static Type[] GetSubclassesTypes(string baseClass)
        {
            var subclassList = new List<Type>();

            lock (TypeList)
            {
                // Only load types once
                if (TypeList.Count == 0)
                {
                    LoadTypes();
                }

                try
                {
                    foreach (var pluginType in TypeList)
                    {
                        if (pluginType != null && pluginType.GetInterface(baseClass, true) != null)
                        {
                            if (!pluginType.IsAbstract)
                            {
                                subclassList.Add(pluginType);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Added following line to suppress warning about
                    //  ex not being used
                    ex.ToString();
                    throw;
                }
            }

            return subclassList.ToArray();
        }
    }
}
