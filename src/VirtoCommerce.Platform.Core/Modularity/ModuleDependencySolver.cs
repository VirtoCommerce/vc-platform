using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity.Exceptions;

namespace VirtoCommerce.Platform.Core.Modularity
{
    /// <summary>
    /// Used by ModuleInitializer to get the load sequence
    /// for the modules to load according to their dependencies.
    /// </summary>
    public class ModuleDependencySolver
    {
        private readonly ListDictionary<string, string> _dependencyMatrix = [];
        private readonly List<string> _knownModules = [];

        private readonly List<string> _boostedModules;
        private readonly ListDictionary<string, string> _boostedDependencyMatrix = [];

        public ModuleDependencySolver(ModuleSequenceBoostOptions boostOptions)
        {
            _boostedModules = boostOptions.ModuleSequenceBoost.ToList();
        }

        /// <summary>
        /// Adds a module to the solver.
        /// </summary>
        /// <param name="name">The name that uniquely identifies the module.</param>
        public void AddModule(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            AddToDependencyMatrix(name);
            AddToKnownModules(name);

            if (_boostedModules.Contains(name))
            {
                AddToBoostedDependencyMatrix(name);
            }
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
            if (string.IsNullOrEmpty(dependingModule))
            {
                throw new ArgumentNullException(nameof(dependingModule));
            }

            if (string.IsNullOrEmpty(dependentModule))
            {
                throw new ArgumentNullException(nameof(dependentModule));
            }

            if (!_knownModules.Contains(dependingModule))
            {
                throw new ArgumentException($"Cannot add dependency for unknown module {dependingModule}");
            }

            AddToDependencyMatrix(dependentModule);
            _dependencyMatrix.Add(dependentModule, dependingModule);

            if (_boostedModules.Contains(dependingModule))
            {
                var index = _boostedModules.IndexOf(dependingModule);
                _boostedModules.Insert(index, dependentModule);

                _boostedDependencyMatrix.Add(dependentModule, dependingModule);
            }
        }

        private void AddToDependencyMatrix(string module)
        {
            if (!_dependencyMatrix.ContainsKey(module))
            {
                _dependencyMatrix.Add(module);
            }
        }

        private void AddToKnownModules(string module)
        {
            if (!_knownModules.Contains(module))
            {
                _knownModules.Add(module);
            }
        }

        private void AddToBoostedDependencyMatrix(string module)
        {
            if (!_boostedDependencyMatrix.ContainsKey(module))
            {
                _boostedDependencyMatrix.Add(module);
            }
        }

        /// <summary>
        /// Calculates an ordered vector according to the defined dependencies.
        /// Non-dependant modules appears at the beginning of the resulting array.
        /// </summary>
        /// <returns>The resulting ordered list of modules.</returns>
        /// <exception cref="CyclicDependencyFoundException">This exception is thrown
        /// when a cycle is found in the defined dependency graph.</exception>
        public string[] Solve()
        {
            var skip = new List<string>();
            while (skip.Count < _dependencyMatrix.Count)
            {
                var leaves = FindLeaves(skip, _dependencyMatrix);
                if (leaves.Count == 0 && skip.Count < _dependencyMatrix.Count)
                {
                    throw new CyclicDependencyFoundException($"At least one cyclic dependency has been found in the module catalog. Cycles in the module dependencies must be avoided.");
                }
                skip.AddRange(leaves);
            }
            skip.Reverse();

            if (_boostedDependencyMatrix.Count > 0)
            {
                var boostedModules = GetBoostedSortedModules();

                // Remove boosted modules and add them to the start of the list
                skip.RemoveAll(boostedModules.Contains);
                skip = boostedModules.Concat(skip).ToList();
            }

            if (skip.Count > _knownModules.Count)
            {
                var missedDependencies = skip.Except(_knownModules).ToList();
                // Create missed module matrix (key: missed module, value: module that miss it) and reverse it (keys to values, values to keys; key: module that miss other module, value: missed module)
                var missedDependenciesMatrix = missedDependencies.ToDictionary(md => md, md => _dependencyMatrix[md])
                    .SelectMany(p => p.Value.Select(m => new KeyValuePair<string, string>(m, p.Key)))
                    .GroupBy(p => p.Key)
                    .ToDictionary(g => g.Key, g => g.Select(p => p.Value));
                throw new MissedModuleException(missedDependenciesMatrix, $"A module declared a dependency on another module which is not declared to be loaded. Missing module(s): {string.Join(", ", missedDependencies)}");
            }

            return skip.ToArray();
        }

        private List<string> GetBoostedSortedModules()
        {
            var result = new List<string>();
            while (result.Count < _boostedDependencyMatrix.Count)
            {
                var leaves = FindLeaves(result, _boostedDependencyMatrix);
                result.AddRange(leaves);
            }
            result.Reverse();
            return result;
        }

        /// <summary>
        /// Gets the number of modules added to the solver.
        /// </summary>
        /// <value>The number of modules.</value>
        public int ModuleCount
        {
            get { return _dependencyMatrix.Count; }
        }

        private static List<string> FindLeaves(List<string> skip, ListDictionary<string, string> dependencies)
        {
            var result = new List<string>();

            foreach (var precedent in dependencies.Keys)
            {
                if (skip.Contains(precedent))
                {
                    continue;
                }

                var count = 0;
                foreach (var dependent in dependencies[precedent])
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
