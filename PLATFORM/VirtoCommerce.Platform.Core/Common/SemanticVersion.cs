using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Common
{
	public class SemanticVersion : IComparable
	{
		private readonly Version _version;

		public static readonly Regex SemanticVersionStrictRegex = new Regex(@"^(?<Version>([0-9]|[1-9][0-9]*)(\.([0-9]|[1-9][0-9]*)){2})(?<Release>-([0]\b|[0]$|[0][0-9]*[A-Za-z-]+|[1-9A-Za-z-][0-9A-Za-z-]*)+(\.([0]\b|[0]$|[0][0-9]*[A-Za-z-]+|[1-9A-Za-z-][0-9A-Za-z-]*)+)*)?(?<Metadata>\+[0-9A-Za-z-]+(\.[0-9A-Za-z-]+)*)?$", RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

		public SemanticVersion(Version version)
		{
			if (version == null)
			{
				throw new ArgumentNullException("version");
			}

			_version = version;
		}

		/// <summary>
		/// Major version X (X.y.z)
		/// </summary>
		public int Major { get { return _version.Major; } }

		/// <summary>
		/// Minor version Y (x.Y.z)
		/// </summary>
		public int Minor { get { return _version.Minor; } }

		/// <summary>
		/// Patch version Z (x.y.Z)
		/// </summary>
		public int Patch { get { return _version.Build; } }

		public bool IsCompatibleWith(SemanticVersion other)
		{
			if (other == null)
				throw new ArgumentNullException("other");

			var comparisonResult = this.CompareTo(other);
			return comparisonResult <= 0;
		}

		public static SemanticVersion Parse(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				throw new ArgumentNullException("value");
			}

			var match = SemanticVersionStrictRegex.Match(value.Trim());

			Version versionValue;
			if (match.Success && Version.TryParse(match.Groups["Version"].Value, out versionValue))
			{
				var normalizedVersion = NormalizeVersionValue(versionValue);
				return new SemanticVersion(normalizedVersion);
			}

			throw new FormatException();
		}

	

		private static Version NormalizeVersionValue(Version version)
		{
			return new Version(version.Major,
							   version.Minor,
							   Math.Max(version.Build, 0),
							   Math.Max(version.Revision, 0));
		}

		#region IComparable Members

		public int CompareTo(object obj)
		{
			SemanticVersion other = (SemanticVersion)obj;

			if (other == null)
				throw new ArgumentException("obj");

			if (ReferenceEquals(this, null) && ReferenceEquals(other, null))
				return 0;

			if (ReferenceEquals(other, null))
				return 1;

			if (ReferenceEquals(this, null))
				return -1;

			var result = this.Major.CompareTo(other.Major);
			if (result != 0)
				return result;

			result = this.Minor.CompareTo(other.Minor);
			if (result != 0)
				return result;

			result = this.Patch.CompareTo(other.Patch);

			return result;
		}

		#endregion

		public override string ToString()
		{
			return String.Format("{0}.{1}.{2}", Major, Minor, Patch);
		}
	}
}
