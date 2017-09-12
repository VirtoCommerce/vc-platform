using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity.Exceptions;
using VirtoCommerce.Platform.Core.Properties;

namespace VirtoCommerce.Platform.Core.Modularity
{
    /// <summary>
    /// Used by ModuleInitializer to get the load sequence
    /// for the modules to load according to their dependencies.
    /// </summary>
    public class ModuleDependencySolver
    {
        private readonly ListDictionary<string, string> dependencyMatrix = new ListDictionary<string, string>();
        private readonly List<string> knownModules = new List<string>();

        /// <summary>
        /// Adds a module to the solver.
        /// </summary>
        /// <param name="name">The name that uniquely identifies the module.</param>
        public void AddModule(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.StringCannotBeNullOrEmpty, "name"));

            AddToDependencyMatrix(name);
            AddToKnownModules(name);
        }

        /// <summary>
        /// Adds a module dependency between the modules specified by dependingModule and
        /// dependentModule.
        /// </summary>
        /// <param name="dependingModule">The name of the module with the dependency.</param>
        /// <param name="dependentModule">The name of the module dependingModule
        /// depends on.</param>
        public void AddDependency(string dependingModule, string dependentModule)
        {
            if (String.IsNullOrEmpty(dependingModule))
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.StringCannotBeNullOrEmpty, "dependingModule"));

            if (String.IsNullOrEmpty(dependentModule))
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.StringCannotBeNullOrEmpty, "dependentModule"));

            if (!knownModules.Contains(dependingModule))
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.DependencyForUnknownModule, dependingModule));

            AddToDependencyMatrix(dependentModule);
            dependencyMatrix.Add(dependentModule, dependingModule);
        }

        private void AddToDependencyMatrix(string module)
        {
            if (!dependencyMatrix.ContainsKey(module))
            {
                dependencyMatrix.Add(module);
            }
        }

        private void AddToKnownModules(string module)
        {
            if (!knownModules.Contains(module))
            {
                knownModules.Add(module);
            }
        }

        /// <summary>
        /// Calculates an ordered vector according to the defined dependencies.
        /// Non-dependant modules appears at the beginning of the resulting array.
        /// </summary>
        /// <returns>The resulting ordered list of modules.</returns>
        /// <exception cref="CyclicDependencyFoundException">This exception is thrown
        /// when a cycle is found in the defined depedency graph.</exception>
        public string[] Solve()
        {
            List<string> skip = new List<string>();
            while (skip.Count < dependencyMatrix.Count)
            {
                List<string> leaves = this.FindLeaves(skip);
                if (leaves.Count == 0 && skip.Count < dependencyMatrix.Count)
                {
                    throw new CyclicDependencyFoundException(Resources.CyclicDependencyFound);
                }
                skip.AddRange(leaves);
            }
            skip.Reverse();

            if (skip.Count > knownModules.Count)
            {
                var missedDependencies = skip.Except(knownModules).ToList();
                // Create missed module matrix (key: missed module, value: module that miss it) and reverse it (keys to values, values to keys; key: module that miss other module, value: missed module)
                var missedDependenciesMatrix = missedDependencies.ToDictionary(md => md, md => dependencyMatrix[md])
                    .SelectMany(p => p.Value.Select(m => new KeyValuePair<string, string>(m, p.Key)))
                    .GroupBy(p => p.Key)
                    .ToDictionary(g => g.Key, g => g.Select(p => p.Value));
                throw new MissedModuleException(missedDependenciesMatrix, string.Format(CultureInfo.CurrentCulture, Resources.DependencyOnMissingModule, string.Join(", ", missedDependencies)));
            }

            return skip.ToArray();
        }

        /// <summary>
        /// Gets the number of modules added to the solver.
        /// </summary>
        /// <value>The number of modules.</value>
        public int ModuleCount
        {
            get { return dependencyMatrix.Count; }
        }

        private List<string> FindLeaves(List<string> skip)
        {
            List<string> result = new List<string>();

            foreach (string precedent in dependencyMatrix.Keys)
            {
                if (skip.Contains(precedent))
                {
                    continue;
                }

                int count = 0;
                foreach (string dependent in dependencyMatrix[precedent])
                {
                    if (skip.Contains(dependent))
                    {
                        continue;
                    }
                    count++;
                }
                if (count == 0)
                {
                    result.Add(precedent);
                }
            }
            return result;
        }
    }
}