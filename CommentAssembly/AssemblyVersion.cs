//-----------------------------------------------------------------------
// <copyright file="AssemblyVersion.cs" company="hiLab">
//     Copyright (c) Francesco Iovine.
// </copyright>
// <author>Francesco Iovine iovinemeccanica@gmail.com</author>
//-----------------------------------------------------------------------
namespace CommentAssembly
{
    using System;

    /// <summary>
    /// This class models the version of the assembly following Microsoft rules.<para/>
    /// Microsoft uses versions described by four numbers, namely
    /// Major number<para/>
    /// Minor number<para/>
    /// Build number<para/>
    /// Revision number <para/>
    /// <para/>
    /// This represents and manipulates version identifiers of this structure.<para/>
    /// The objects of this class are immutable.
    /// </summary>
    public class AssemblyVersion
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyVersion" /> class.<para/>
        /// This constructor decodes a string representation of the version.
        /// </summary>
        /// <param name="version">String representation of the version, composed by four dot separated numbers.</param>
        public AssemblyVersion(string version)
        {
            string[] elements = version.Split('.');
            try
            {
                this.Major = uint.Parse(elements[0]);
                this.Minor = uint.Parse(elements[1]);
                this.Build = uint.Parse(elements[2]);
                this.Revision = uint.Parse(elements[3]);
            }
            catch (Exception e)
            {
                throw new FormatException("Unable to parse the version string" + e);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyVersion" /> class.<para/>
        /// Copy initializer version.
        /// </summary>
        /// <param name="other">Assembly version object to be duplicated.</param>
        public AssemblyVersion(AssemblyVersion other)
        {
            this.Major = other.Major;
            this.Minor = other.Minor;
            this.Build = other.Build;
            this.Revision = other.Revision;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyVersion" /> class.
        /// </summary>
        /// <param name="major">Major component of the version.</param>
        /// <param name="minor">Minor component of the version.</param>
        /// <param name="build">Build component of the version.</param>
        /// <param name="revision">Revision component of the version.</param>
        public AssemblyVersion(uint major, uint minor, uint build, uint revision)
        {
            this.Major = major;
            this.Minor = minor;
            this.Build = build;
            this.Revision = revision;
        }

        /// <summary>
        /// Gets the build number of the version.
        /// </summary>
        public uint Build
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the major number of the version.
        /// </summary>
        public uint Major
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the minor number of the version.
        /// </summary>
        public uint Minor
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the revision number of the version.
        /// </summary>
        public uint Revision
        {
            get;
            private set;
        }

        /// <summary>
        /// Increments the revision component of the current version.<para/>
        /// The revision component is the rightmost number.
        /// </summary>
        /// <returns>An object representing the current version with incremented revision number.</returns>
        public AssemblyVersion Next()
        {
            AssemblyVersion result = new AssemblyVersion(this);
            if (result.Revision == uint.MaxValue)
            {
                throw new OverflowException("The revision number is overflowing");
            }

            result.Revision++;
            return result;
        }

        /// <summary>
        /// Computes the version in a readable format composed by the four numbers separated by dots.
        /// </summary>
        /// <returns>The version in string form.</returns>
        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}.{3}", this.Major, this.Minor, this.Build, this.Revision);
        }
    }
}
