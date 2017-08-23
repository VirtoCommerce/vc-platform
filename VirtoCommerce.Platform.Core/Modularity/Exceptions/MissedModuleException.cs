using System;
using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.Platform.Core.Modularity.Exceptions
{
    /// <summary>
    /// Represents the exception that is thrown when at least one dependency was missed.
    /// </summary>
    public class MissedModuleException : ModularityException
    {
        public IDictionary<string, IEnumerable<string>> MissedDependenciesMatrix { get; set; } = new Dictionary<string, IEnumerable<string>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MissedModuleException"/> class.
        /// </summary>
        public MissedModuleException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissedModuleException"/> class
        /// with the specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MissedModuleException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissedModuleException"/> class
        /// with the specified error message and inner exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public MissedModuleException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Initializes the exception with a particular modules and error message.
        /// </summary>
        /// <param name="missedDependenciesMatrix">The dependency matrix of the missed modules.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public MissedModuleException(IDictionary<string, IEnumerable<string>> missedDependenciesMatrix, string message)
            : base(string.Join(", ", missedDependenciesMatrix.Values.SelectMany(m => m).Distinct()), message)
        {
            MissedDependenciesMatrix = missedDependenciesMatrix;
        }

        /// <summary>
        /// Initializes the exception with a particular modules, error message and inner exception that happened.
        /// </summary>
        /// <param name="missedDependenciesMatrix">The dependency matrix of the missed modules.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, 
        /// or a <see langword="null"/> reference if no inner exception is specified.</param>
        public MissedModuleException(IDictionary<string, IEnumerable<string>> missedDependenciesMatrix, string message, Exception innerException)
            : base(string.Join(", ", missedDependenciesMatrix.Values.SelectMany(m => m).Distinct()), message, innerException)
        {
            MissedDependenciesMatrix = missedDependenciesMatrix;
        }
    }
}
